using System;
using Newtonsoft.Json.Serialization;

public class SkillComponentSerializationBinder : ISerializationBinder
{
    DefaultSerializationBinder defaultBinder = new DefaultSerializationBinder();

    void ISerializationBinder.BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        if (serializedType == typeof(SkillComponent))
        {
            assemblyName = "Global";
            typeName = serializedType.Name;
        }
        else
        {
            defaultBinder.BindToName(serializedType, out assemblyName, out typeName);
        }
    }

    Type ISerializationBinder.BindToType(string assemblyName, string typeName)
    {
        if (typeName == nameof(SkillComponent) && assemblyName == "Global")
        {
            return typeof(SkillComponent);
        }
        else
        {
            return defaultBinder.BindToType(assemblyName, typeName);
        }
    }
}