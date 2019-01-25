using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayMode
{
    linear,
    Catmul,

}



[ExecuteInEditMode]
public class Rail : MonoBehaviour
{

    public Transform[] nodes;
    float dot;

    private void Start()
    {
        nodes = GetComponentsInChildren<Transform>();
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < nodes.Length - 1; i++)
        {
            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
    }

    public Vector3 PositionOnRail (int nodeIndex, float dot, PlayMode mode)
    {
        switch (mode)
        {
            default:
            case PlayMode.linear:
                return LinearTranslation(nodeIndex, dot);
            case PlayMode.Catmul:
                return CatmullTranslation(nodeIndex, dot);
            
        }
    }

    public Vector3 LinearTranslation (int nodeIndex, float dot)
    {
        Vector3 q1 = nodes[nodeIndex].position;
        Vector3 q2 = nodes[nodeIndex + 1].position;

        return Vector3.Lerp(q1, q2, dot);
    }

    public Quaternion Orientation (int nodeIndex, float dot)
    {
        Quaternion q1 = nodes[nodeIndex].rotation;
        Quaternion q2 = nodes[nodeIndex + 1].rotation;

        return Quaternion.Lerp(q1, q2, dot);
    }

    public Vector3 CatmullTranslation (int nodeIndex, float dot)
    {
        Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3(), p4 = new Vector3();

        if(nodeIndex == 0)
        {
            p1 = nodes[nodeIndex].position;
            p2 = p1;                                //Small hack to go around array index, this is our last node
            p3 = nodes[nodeIndex + 1].position;
            p4 = nodes[nodeIndex + 2].position;
        }

        else if(nodeIndex == nodes.Length - 2)
        {
            p1 = nodes[nodeIndex - 1].position;
            p2 = nodes[nodeIndex].position;
            p3 = nodes[nodeIndex + 1].position;
            p4 = p3;
        }

        else
        {
            p1 = nodes[nodeIndex - 1].position;
            p2 = nodes[nodeIndex].position;
            p3 = nodes[nodeIndex + 1].position;
            p4 = nodes[nodeIndex + 2].position;
        }

        float t2 = dot * dot;
        float t3 = t2 * dot;

        float x = 
            0.5f * ((2.0f * p2.x)
            + (-p1.x + p3.x)
            * dot + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x)
            * t2 + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x)
            * t3);

        float y =
            0.5f * ((2.0f * p2.y)
            + (-p1.y + p3.y)
            * dot + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y)
            * t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y)
            * t3);

        float z =
            0.5f * ((2.0f * p2.z)
            + (-p1.z + p3.z)
            * dot + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z)
            * t2 + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z)
            * t3);

        return new Vector3(x, y, z);
    }
}
