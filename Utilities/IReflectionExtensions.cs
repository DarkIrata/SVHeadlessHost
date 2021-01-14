using System;
using StardewModdingAPI;

namespace SVHeadlessHost.Utilities
{
    public static class IReflectionExtensions
    {
        public static T GetFieldValueEx<T>(this IReflectionHelper reflectionHelper, object obj, string name, T fallbackValue, IMonitor monitor = null)
        {
            try
            {
                return reflectionHelper.GetField<T>(obj, name).GetValue();
            }
            catch (Exception ex)
            {
                if (monitor != null)
                {
                    monitor.Log($"Error while executing {nameof(GetFieldValueEx)} for {nameof(IReflectionHelper)}{Environment.NewLine}" +
                        $"Field Name: {name}, Fallback Value: {fallbackValue?.ToString() ?? "NULL"}{Environment.NewLine}" +
                        $"{ex.Message}", LogLevel.Error);
                }

                return fallbackValue;
            }
        }

        public static IReflectedMethod GetMethodEx(this IReflectionHelper reflectionHelper, object obj, string name, bool require = true, IMonitor monitor = null)
        {
            try
            {
                return reflectionHelper.GetMethod(obj, name, require);
            }
            catch (Exception ex)
            {
                if (monitor != null)
                {
                    monitor.Log($"Error while executing {nameof(GetMethodEx)} for {nameof(IReflectionHelper)}{Environment.NewLine}" +
                        $"Method Name: {name}{Environment.NewLine}" +
                        $"{ex.Message}", LogLevel.Error);
                }

                return null;
            }
        }
    }
}
