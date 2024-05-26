using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : BaseMeshEffect
{
    public Gradient gradient;

    public override void ModifyMesh(VertexHelper vh)
    {
        List<UIVertex> ver = new List<UIVertex>();
        vh.GetUIVertexStream(ver);

        for(int i = 0; i < ver.Count; i++)
        {
            var v = ver[i];
            v.color = new Color();
            ver[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(ver); 
    }

}
