using UnityEngine;

public static class Sprite2DMaterial
{
    public static Material GetDefaultMaterial()
    {
        var material = Resources.Load<Material>("Sprite-Lit-Shadow");
        return material;
    }
    public static Material GetWhiteMaterial()
    {
        var material = Resources.Load<Material>("White");
        return material;
    }
}