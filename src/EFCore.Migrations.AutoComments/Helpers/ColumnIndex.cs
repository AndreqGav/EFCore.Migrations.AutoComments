using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Migrations.AutoComments.Helpers;

/// <summary>
/// Index of model properties grouped by physical column (table + column name).
/// Lets <c>SetColumnComments</c> look up in O(1) all <see cref="IConventionProperty"/> instances that map to the
/// same column (TPH descendants, table splitting, complex types), instead of rescanning every property of every
/// entity per call. Turns the quadratic scans into a linear pass plus dictionary lookups.
/// </summary>
internal class ColumnIndex
{
    /// <summary>
    /// Properties grouped by (table, column name) key. A group contains every property EF Core maps to the
    /// same physical column.
    /// </summary>
    private readonly Dictionary<(StoreObjectIdentifier Store, string Column), List<IConventionProperty>> _byColumn = new();

    /// <summary>
    /// Build the index from all model entity types. Each property (including those inside complex types) is
    /// visited once and bucketed by its (StoreObjectIdentifier, column name) key.
    /// </summary>
    public static ColumnIndex Build(IEnumerable<IConventionEntityType> entityTypes)
    {
        var index = new ColumnIndex();

        foreach (var entityType in entityTypes)
        {
            foreach (var property in GetFlattenedProperties(entityType))
            {
                var store = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);

                // Property is not mapped to a table (e.g. shadow on keyless or view-backed type) — skip.
                if (store == null) continue;

                var key = (store.Value, property.GetColumnName(store.Value));

                if (!index._byColumn.TryGetValue(key, out var list))
                {
                    index._byColumn[key] = list = new List<IConventionProperty>();
                }

                list.Add(property);
            }
        }

        return index;
    }

    /// <summary>
    /// Returns every property mapped to the same physical column as the given one (including the property
    /// itself). When the index has no entry for that column, or the property is not mapped to a table, returns
    /// a single-element list containing the property so callers can iterate uniformly.
    /// </summary>
    public List<IConventionProperty> GetSiblings(IConventionProperty property)
    {
        var store = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);

        if (store == null) return new List<IConventionProperty> { property };

        return _byColumn.TryGetValue((store.Value, property.GetColumnName(store.Value)), out var list)
            ? list
            : new List<IConventionProperty> { property };
    }

    /// <summary>
    /// Recursively yields all properties of a type, including properties of nested complex types.
    /// </summary>
    private static IEnumerable<IConventionProperty> GetFlattenedProperties(IConventionTypeBase entityType)
    {
        if (entityType is IConventionEntityType entity)
        {
            foreach (var property in entity.GetProperties())
            {
                yield return property;
            }
        }
    }
}
