using UnityEngine;
using UnityEngine.Localization;

public static class LocalizeHelper
{
    public static LocaleIdentifier SystemLanguageToLocaleIdentifier()
    {
        return new LocaleIdentifier(Application.systemLanguage);
    }
}
