using UnityEngine;

public class ChestEvent :MonoBehaviour
{
    public string iconName,showIconName;

    public virtual (Sprite,string) Execute()
    {
        string _iconName = "";
        if (!iconName.StartsWith("Icon/"))
            _iconName = "Icon/" + iconName;

        var sprite = Resources.Load<Sprite>(_iconName);
        return (sprite,showIconName);
    }
}
