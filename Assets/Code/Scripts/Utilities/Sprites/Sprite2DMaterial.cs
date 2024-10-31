using UnityEngine;

public static class Sprite2DMaterial
{
    public static Material GetOutlineMaterial()
    {
        var material = Resources.Load<Material>("Sprite-Outline");
        return material;
    }
    public static Material GetDefaultMaterial()
    {
        var material = Resources.Load<Material>("Sprite-Shadow");
        return material;
    }
    public static Material GetWhiteMaterial()
    {
        var material = Resources.Load<Material>("White");
        return material;
    }
}