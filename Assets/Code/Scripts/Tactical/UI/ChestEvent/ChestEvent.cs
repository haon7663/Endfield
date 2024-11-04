using UnityEngine;

public class ChestEvent :MonoBehaviour
{
    public string iconName;

    public virtual Sprite Excute()
    {
        if (!iconName.StartsWith("Icon/"))
            iconName = "Icon/" + iconName;

        var sprite = Resources.Load<Sprite>(iconName);
        return sprite;
    }
}
