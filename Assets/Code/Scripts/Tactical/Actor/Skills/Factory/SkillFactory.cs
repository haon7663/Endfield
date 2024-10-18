using UnityEngine;

public static class SkillFactory
{
    public static GameObject Create(string path)
    {
        if (!path.StartsWith("Skills/"))
            path = "Skills/" + path;

        return InstantiatePrefab(path);
    }
    
    public static GameObject InstantiatePrefab(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        if (prefab == null) {
            Debug.LogError("No Prefab for name: " + path);
            return new GameObject(path);
        }
        var instance = Object.Instantiate(prefab);
        instance.name = instance.name.Replace("(Clone)", "");

        return instance;
    }
}