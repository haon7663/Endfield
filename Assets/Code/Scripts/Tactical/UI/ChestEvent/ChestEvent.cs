using UnityEngine;

public class ChestEvent :MonoBehaviour
{
    public string iconName;

    public virtual (Sprite,string) Excute()
    {
        string _iconName = "";
        if (!iconName.StartsWith("Icon/"))
            _iconName = "Icon/" + iconName;

        var sprite = Resources.Load<Sprite>(_iconName);
        return (sprite,iconName);
    }
}
