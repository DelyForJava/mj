using ILRuntime.Runtime;
using LitJson;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using WebSocketSharp;

[RequireComponent(typeof(AudioSource))]
public class Audiocontroller : MonoBehaviour {

    public static Audiocontroller Instance;
    private Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>();
    private Dictionary<int, AudioClip> ManMajiangDic = new Dictionary<int, AudioClip>();
    private Dictionary<int, AudioClip> WomanMajiangDic = new Dictionary<int, AudioClip>();
    public List<string> Scripname;
    public List<int> Majiangname;
    public List<AudioClip> ManCliplist;
    public List<AudioClip> WoManCliplist;
    public List<AudioClip> AClip;
    public AudioSource aso;

    private AudioClip redioclip;
    private AudioSource BGM;
    const int RECORD_TIME = 5;
    private float cliptTime;
    public int showChatTime;

    public int tableNum;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < Scripname.Count; i++)
        {
            AudioDic.Add(Scripname[i], AClip[i]);
        }
        for (int i = 0; i < Majiangname.Count; i++)
        {
            ManMajiangDic.Add(Majiangname[i], ManCliplist[i]);
           // WomanMajiangDic.Add(Majiangname[i], WoManCliplist[i]);
        }
        aso = this.GetComponent<AudioSource>();
        BGM = Camera.main.GetComponent<AudioSource>();

       // IsFullSampl = false;
    }

    //注册语音消息接收方法
    public void reginsVoice() {
        Debug.Log("注册语音接收=============消息");
        WebSoketCall._Instance.MicPhoneCallback += MicPhone;   //添加收到语音消息的方法
    }


    //删除语音消息接收方法
    public void delectVoice()
    {
        Debug.Log("删除语音接收=============消息");
        WebSoketCall._Instance.MicPhoneCallback -= MicPhone;   
    }
    void Start() {

        if (aso.clip == null)
        {
            aso.clip = AudioClip.Create("playRecordClip", 160000, 1, 6000, false);
        }     
    }

    //使用语音功能之前先看下是否打开了音效
    public static bool GetEffectAudio()
    {
        AudioSource currEffectAudio = Audiocontroller.Instance.aso;

        string isEfOp = PlayerPrefs.GetString("IsEffectOpen");

        bool isOpEff = isEfOp == "Open" || string.IsNullOrEmpty(isEfOp) ? true : false;

        return isOpEff;
    }


    /// <summary>
    /// 抬起手势 完成录音
    /// </summary>
    public void MicrophoneUp(float clipstime) {

        cliptTime = clipstime;

        if (cliptTime<1.0f)
        {
            Prefabs.PopBubble("录音时长过短!");
            SetPlayBgm();
            return;
        }
        Debug.Log("cliptTime======" + (int)cliptTime);

        Microphone.End(Microphone.devices[0]);   //关闭麦克风

        //把录音下来的音频保存转化为wav格式 返回路径
        string path =  SavWav.Save("suibian", redioclip);
        //获取音频的byt[] 
        byte[]data= WavToByte(path);

       // string str = Convert.ToBase64String(data);
        //byte[] bytes = Convert.FromBase64String(str);
        //AudioClip audioClip= GetAudioClipByByte(bytes);
       // aso.clip = audioClip;
       // aso.Play();
      
        // 将byte数组转换成json字符串文件
        string strclips = Convert.ToBase64String(data);
        Debug.Log("MicrophoneUp.strclips=====" + strclips.Length);

        JsonAudioData audioDate = new JsonAudioData();

        audioDate.tableNum = tableNum;
        audioDate.memberId = UserId.memberId;
        audioDate.clipTime = (int)cliptTime; 
        audioDate.audioData = strclips;
        
        string audio = JsonMapper.ToJson(audioDate);
       

        //发送语音消息 给其他三个人
         string audiurl = "http://" + Bridge.GetHostAndPort() + "/api/game/table/audio";
         JX.HttpCallSever.One().PostCallServer(audiurl, audio, Debug.Log);

        aso.clip = redioclip;
        aso.mute = false;
        aso.Play();
        GameObject.Find("Gold_Game").GetComponent<Game_Controller>().ShowChatTime(audioDate.memberId, audioDate.clipTime);

        Invoke("waitSecound", audioDate.clipTime);
     

    }

    //等待语音播放完成 播放bgm
    void waitSecound() {
        SetPlayBgm();
    }

    /// <summary>
    /// 按下手势 开始录音
    /// </summary>
    public void MicrophoneDown()
    {
        SetStopBgm();
        tableNum = GameObject.Find("Gold_Game").GetComponent<Game_Controller>().tableNum;
        redioclip = Microphone.Start(Microphone.devices[0], false, RECORD_TIME, 6000); //22050     
    }


    /// <summary>
    /// 接收到麦克风的消息
    /// </summary>
    /// <param name="audiodata"></param>
    public void MicPhone(JAData audioData)
    {
        //停止背景音乐
        SetStopBgm();

        Debug.Log("执行到这了========");
        AudioClip audioClip= GetAudioClipByByte(audioData.audioData);
        aso.clip = audioClip;
        //播放
        aso.Play();

        showChatTime = audioData.clipTime;
       
        StartCoroutine(PlayMicPho());
    }


    /// <summary>
    /// 获取byte[]文件转换为AudioClip
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public AudioClip GetAudioClipByByte(byte[] buffer)
    {
        AudioClip audioClip = NAudioPlayer.FromWavData(buffer);
        return audioClip;
    }

    //背景音乐的播放控制
    public void SetPlayBgm() {

       string IsMuOp = PlayerPrefs.GetString("IsMusicOpen");

        bool IsMuOpen = IsMuOp == "Open" || string.IsNullOrEmpty(IsMuOp) ? true : false;
        if (IsMuOpen)
        {         
            BGM.Play();
        }     
    }

    //停止播放背景音乐
    void SetStopBgm() {
        BGM = Camera.main.GetComponent<AudioSource>();
        BGM.Stop();
    }

    //等录音播放完毕 再去播放背景音乐
    IEnumerator PlayMicPho() {

        Debug.Log("showChatTime============" + showChatTime);
        yield return new WaitForSeconds(showChatTime+1);

        SetPlayBgm();                           
    }

    /// <summary>
    ///yl 2020.4.18 声音播放控制
    /// </summary>
    public static void GetSound()
    {
        AudioSource currAudio = Camera.main.GetComponent<AudioSource>();      
        AudioSource currEffectAudio = Audiocontroller.Instance.aso;

        string IsMuOp = PlayerPrefs.GetString("IsMusicOpen");
        string IsEffectOp = PlayerPrefs.GetString("IsEffectOpen");

        bool IsMuOpen= IsMuOp == "Open" || string.IsNullOrEmpty(IsMuOp) ? true : false;
        bool IsEffectOpen =IsEffectOp == "Open" || string.IsNullOrEmpty(IsEffectOp) ? true : false;

        if (IsMuOpen)
        {
            currAudio.Play();
            currAudio.volume = 1.0f;
        }
        else {
            currAudio.Stop(); 
            currAudio.volume = 0;
        }
           

        if (IsEffectOpen)
            currEffectAudio.volume = 1;
        else
            currEffectAudio.volume = 0;

    }

    public void PlayAudio(string Scripname)
    {
        aso.PlayOneShot(AudioDic[Scripname]);
    }
    public void MajiangManPlayer(int Scripname)
    {
        aso.PlayOneShot(ManMajiangDic[Scripname]);
    }
    public void MajiangWanPlayer(int Scripname)
    {
        aso.PlayOneShot(WomanMajiangDic[Scripname]);
    }


    //将路径中的WAV文件转换为byte[]数组并返回
    public byte[] WavToByte(string path)
    {
        Byte[] bs;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        bs = new Byte[fs.Length];
        fs.Read(bs, 0, (int)fs.Length);
        fs.Close();
        return bs;
    }
    //byte转wav

    public void ByteToWav(Byte[] bs)
    {
        FileStream fs = new FileStream(@"C:\2.wav", FileMode.OpenOrCreate, FileAccess.Write);
        fs.Write(bs, 0, bs.Length);
        fs.Close();
    }

}

/// <summary>
/// 发送语音类
/// </summary>
public class JsonAudioData
{
    public int tableNum;
    public int memberId;
    public string audioData;
    public int clipTime;  
}

/// <summary>
/// 接收语音类
/// </summary>
public class JAData
{
    public int memberId;
    public byte[] audioData;
    public int clipTime;
}

public static class NAudioPlayer
{
    //wav byte[]转换AudioClip的方法
    public static AudioClip FromWavData(byte[] buffer)
    {
        // 转换mp3格式的代码
        //MemoryStream mp3stream = new MemoryStream(buffer);
        // Convert the data in the stream to WAV format
        //Mp3FileReader mp3audio = new Mp3FileReader(mp3stream);

        //转换wave格式的代码
        MemoryStream wavstream = new MemoryStream(buffer);
        WaveFileReader waveAudio = new WaveFileReader(wavstream);

        WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(waveAudio);

        // Convert to WAV data
        WAV wav = new WAV(AudioMemStream(waveStream).ToArray());

        Debug.Log(wav);
        AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
        audioClip.SetData(wav.LeftChannel, 0);
        // Return the clip

        return audioClip;
    }

    private static MemoryStream AudioMemStream(WaveStream waveStream)
    {
        MemoryStream outputStream = new MemoryStream();
        using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
        {
            byte[] bytes = new byte[waveStream.Length];
            waveStream.Position = 0;
            waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
            waveFileWriter.Write(bytes, 0, bytes.Length);
            waveFileWriter.Flush();
        }
        return outputStream;
    }
}

public class WAV
{
    // convert two bytes to one float in the range -1 to 1
    static float bytesToFloat(byte firstByte, byte secondByte)
    {
        // convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    static int bytesToInt(byte[] bytes, int offset = 0)
    {
        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value |= ((int)bytes[offset + i]) << (i * 8);
        }
        return value;
    }
    // properties
    public float[] LeftChannel { get; internal set; }
    public float[] RightChannel { get; internal set; }
    public int ChannelCount { get; internal set; }
    public int SampleCount { get; internal set; }
    public int Frequency { get; internal set; }

    public WAV(byte[] wav)
    {

        // Determine if mono or stereo
        ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

        // Get the frequency
        Frequency = bytesToInt(wav, 24);

        // Get past all the other sub chunks to get to the data subchunk:
        int pos = 12;   // First Subchunk ID from 12 to 16

        // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
        while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
        {
            pos += 4;
            int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
            pos += 4 + chunkSize;
        }
        pos += 8;

        // Pos is now positioned to start of actual sound data.
        SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
        if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

        // Allocate memory (right will be null if only mono sound)
        LeftChannel = new float[SampleCount];
        if (ChannelCount == 2) RightChannel = new float[SampleCount];
        else RightChannel = null;

        // Write to double array/s:
        int i = 0;
        while (pos < wav.Length)
        {
            LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
            pos += 2;
            if (ChannelCount == 2)
            {
                RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
            }
            i++;
        }
    }

    public override string ToString()
    {
        return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
    }
}

/// <summary>
/// AudioClip转换为WAV类
/// </summary>
public static class SavWav
{
    const int HEADER_SIZE = 44;

    //将AudioClip文件转换为WAV格式存放本地
    public static string Save(string filename, AudioClip clip)
    {
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        var filepath = Path.Combine(Application.persistentDataPath, filename);
   
        Debug.Log(filepath);

        // Make sure directory exists if user is saving to sub dir.
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (var fileStream = CreateEmpty(filepath))
        {

            ConvertAndWrite(fileStream, clip);

            WriteHeader(fileStream, clip);
        }

        return filepath; // TODO: return false if there's a failure saving the file
    }

    public static AudioClip TrimSilence(AudioClip clip, float min)
    {
        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
    }

    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz)
    {
        return TrimSilence(samples, min, channels, hz, false, false);
    }

    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream)
    {
        int i;

        for (i = 0; i < samples.Count; i++)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }
        }

        samples.RemoveRange(0, i);

        for (i = samples.Count - 1; i > 0; i--)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }
        }

        samples.RemoveRange(i, samples.Count - i);

        var clip = AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);

        clip.SetData(samples.ToArray(), 0);

        return clip;
    }

    static FileStream CreateEmpty(string filepath)
    {
        var fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

        Byte[] bytesData = new Byte[samples.Length * 2];
        //bytesData array is twice the size of
        //dataSource array because a float converted in Int16 is 2 bytes.

        int rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }

    static void WriteHeader(FileStream fileStream, AudioClip clip)
    {
        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);
        // fileStream.Close();
    }
}



