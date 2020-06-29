namespace YouFu
{
    public class Debug
    {
        private static bool closeLog = false;
        public static void Log(object msg)
        {
            if (closeLog == true)
                return;

            UnityEngine.Debug.Log(msg);
        }

        public static void LogWarning(object msg)
        {
            if (closeLog == true)
                return;

            UnityEngine.Debug.LogWarning(msg);
        }

        public static void LogError(object msg)
        {
            if (closeLog == true)
                return;

            UnityEngine.Debug.LogError(msg);
        }
    }

    public class Config
    {
        //public static bool loadFromLocal = true;
        public static bool loadFromLocal = false;
    }

}

