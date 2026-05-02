using System;
using System.Collections.Generic;

namespace EFCore.Migrations.AutoComments.Helpers;

internal class TypeHelper
{
    /// <summary>
    /// Gets all parent types.
    /// </summary>
    public static IEnumerable<Type> GetParentTypes(Type type)
    {
        if (type == null)
        {
            yield break;
        }

        if (IsSimpleType(type))
        {
            yield break;
        }

        // return all inherited types
        var currentType = type;
        while (currentType != null && currentType != typeof(object))
        {
            yield return currentType;
            currentType = currentType.BaseType;
        }

        // return all implemented or inherited interfaces
        foreach (var i in type.GetInterfaces())
        {
            yield return i;
        }
    }

    private static bool IsSimpleType(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;

        return actualType.IsPrimitive ||
               actualType.IsEnum ||
               actualType == typeof(string) ||
               actualType == typeof(decimal) ||
               actualType == typeof(DateTime) ||
               actualType == typeof(Guid);
    }
}