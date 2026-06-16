using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Runtime.CompilerServices;

//namespace DeftSharp.Windows.Input{ }

/// <summary>
///  net472_polyFill
/// </summary>
public static partial class PolyFillExtensions
{

    //conditinal compile to get best performance.. 
#if NETFRAMEWORK

    /// <summary>
    /// net472_polyFill
    /// </summary>
    public static IEnumerable<T> TakeLast<T>(this Queue<T> queue, int count) => TakeLast_internal(queue, count);

    /// <summary>
    ///  net472_polyFill
    /// </summary>
    public static TValue GetValueOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) 
        => dictionary.TryGetValue(key, out var value) ? value : defaultValue;

    /// <summary>
    ///  net472_polyFill 
    /// </summary>
    public static bool TryRemove<TK, TValue>(this ConcurrentDictionary<TK, TValue> dictionary, KeyValuePair<TK, TValue> item) 
        => dictionary.TryRemove(item.Key, out _);

#endif

    /// <summary>
    /// net472_polyFill - nint.Zero
    /// </summary>
    public static nint nint_Zero => (nint)0;


}



/// <summary>
///  net472_polyFill - not for public use,  intented to be consumed in "Unit Tests".
/// </summary>
public static partial class PolyFillExtensions
{


    /// <summary>
    /// net472_polyFill - internal  ---  Ex: collection.TakeLast(5);
    /// </summary>
    internal static IEnumerable<T> TakeLast_internal_viaSO<T>(this IEnumerable<T> source, int N)
    {
        return source.Skip(Math.Max(0, source.Count() - N));
    }

    /// <summary>
    /// net472_polyFill - internal
    /// </summary>
    internal static IEnumerable<T> TakeLast_internal<T>(Queue<T> queue, int count)
    {
        if (queue is null)
            throw new ArgumentNullException("source is null ");

        if (count <= 0 || queue.Count == 0)
            return [];

        if (count <= 0)
            return Enumerable.Empty<T>();
        if (count >= queue.Count)
            return queue.ToList();

        //  isStartIndexFromEnd: true, startIndex: count,
        //  isEndIndexFromEnd: true, endIndex: 0);
        //return queue.Take(queue.Count - count).ToList();

        var startIndex = queue.Count - count;
        var take = count;

        return queue.Skip(startIndex).Take(take).ToList();

    }
}
