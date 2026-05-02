using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Migrations.AutoComments.Tests.Helpers;

static internal class ModelAccessor
{
    public static IModel GetModel(DbContext context)
    {
        return context.GetService<IDesignTimeModel>().Model;
    }

    public static IRelationalModel GetRelationalModel(DbContext context) => GetModel(context).GetRelationalModel();
}