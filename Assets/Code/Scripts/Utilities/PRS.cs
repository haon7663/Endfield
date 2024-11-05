using UnityEngine;

public class PRS
{
    public Vector3 Pos;
    public Quaternion Rot;
    public Vector3 Scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        Pos = pos;
        Rot = rot;
        Scale = scale;
    }
}