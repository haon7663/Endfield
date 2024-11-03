using UnityEngine;

public class ParticleLoader
{
    public static GameObject Create(string path, Vector3 pos, Quaternion quaternion)
    {
        return InstantiatePrefab(path, pos, quaternion);
    }
    
    private static GameObject InstantiatePrefab(string path, Vector3 pos, Quaternion quaternion)
    {
        var prefab = Resources.Load<GameObject>(path);
        if (prefab == null) {
            Debug.LogError("No Prefab for name: " + path);
            return new GameObject(path);
        }

        prefab.transform.SetPositionAndRotation(pos, quaternion);
        
        var instance = Object.Instantiate(prefab);
        instance.name = instance.name.Replace("(Clone)", "");
        
        return prefab;
    }
}