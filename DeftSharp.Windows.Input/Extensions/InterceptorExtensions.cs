using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Extensions;

public static class InterceptorExtensions
{
    public static string ToNames(this IEnumerable<InterceptorInfo> interceptors)
    {
        var array = interceptors.ToArray();

        if (!array.Any())
            return string.Empty;

        return string.Join(", ", array.Select(i => i.Name));
    }
    
    public static IEnumerable<InterceptorType> ToTypes(this IEnumerable<InterceptorInfo> interceptors) 
        => interceptors.Select(i => i.Type);
}