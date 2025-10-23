using System;
using Unity.Collections.LowLevel.Unsafe;

namespace SandyToolkit.Utility
{
    public static class UnsafeHelper
    {
        public static T UnsafeAs<TU, T>(TU from)
            where T : class
            where TU : class
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            return from as T;
#else
            return UnsafeUtility.As<TU, T>(ref from);
#endif
        }

        public static int EnumToInt<T>(T enumValue) where T : struct, Enum, IConvertible
        {
            return UnsafeUtility.EnumToInt(enumValue);
        }
    }
}