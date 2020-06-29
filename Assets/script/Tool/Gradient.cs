using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Gradient : BaseMeshEffect{
    [SerializeField] private Color32 topColor = Color.white;

    [SerializeField] private Color32 bottomColor = Color.black;
    [SerializeField]
    private float spacing_x;
    [SerializeField]
    private float spacing_y;

    private List<UIVertex> mVertexList;
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        int count = vertexList.Count;

        ApplyGradient(vertexList, 0, count);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);



        if (spacing_x == 0 && spacing_y == 0) { return; }
        if (!IsActive()) { return; }
        int count1 = vh.currentVertCount;
        if (count1 == 0) { return; }
        if (mVertexList == null) { mVertexList = new List<UIVertex>(); }
        vh.GetUIVertexStream(mVertexList);
        int row = 1;
        int column = 2;
        List<UIVertex> sub_vertexs = mVertexList.GetRange(0, 6);
        float min_row_left = sub_vertexs.Min(v => v.position.x);
        int vertex_count = mVertexList.Count;
        for (int i = 6; i < vertex_count;)
        {
            if (i % 6 == 0)
            {
                sub_vertexs = mVertexList.GetRange(i, 6);
                float tem_row_left = sub_vertexs.Min(v => v.position.x);
                if (tem_row_left <= min_row_left)
                {
                    min_row_left = tem_row_left;
                    ++row;
                    column = 1;
                    //continue;
                }
            }
            for (int j = 0; j < 6; j++)
            {
                UIVertex vertex = mVertexList[i];
                vertex.position += Vector3.right * (column - 1) * spacing_x;
                vertex.position += Vector3.down * (row - 1) * spacing_y;
                mVertexList[i] = vertex;
                ++i;
            }
            ++column;
        }
        vh.Clear();
        vh.AddUIVertexTriangleStream(mVertexList);
    }

    private void ApplyGradient(List<UIVertex> vertexList, int start, int end)
    {
        float bottomY = vertexList[0].position.y;
        float topY = vertexList[0].position.y;
        for (int i = start; i < end; ++i)
        {
            float y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float uiElementHeight = topY - bottomY;
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            uiVertex.color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
            vertexList[i] = uiVertex;
        }
    }
}
