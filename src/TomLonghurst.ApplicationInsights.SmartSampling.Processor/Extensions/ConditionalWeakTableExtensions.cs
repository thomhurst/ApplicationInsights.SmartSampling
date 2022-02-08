using System.Runtime.CompilerServices;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Extensions;

public static class ConditionalWeakTableExtensions
{
    public static bool TryRemove<TKey, TValue>(this ConditionalWeakTable<TKey, TValue> conditionalWeakTable, TKey key,
        out TValue value)
        where TKey : class
        where TValue : class
    {
        if (!conditionalWeakTable.TryGetValue(key, out value))
        {
            return false;
        }

        conditionalWeakTable.Remove(key);
        return true;
    }
}