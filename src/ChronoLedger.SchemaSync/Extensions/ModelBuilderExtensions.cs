using System.Linq.Expressions;
using System.Reflection;
using ChronoLedger.Common.Extensions;
using ChronoLedger.SchemaSync.Enums;
using ChronoLedger.SchemaSync.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChronoLedger.SchemaSync.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures an enum table for the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to configure the table for.</typeparam>
    /// <param name="modelBuilder">The model builder to configure the enum table on.</param>
    public static void ConfigureEnumTable<TEnum>(this ModelBuilder modelBuilder)
        where TEnum : Enum
    {
        Console.WriteLine($"Configuring enum table for {typeof(TEnum).Name}...");

        var entityType = EnumEntityHelper.BuildEnumEntity<TEnum>();
        var idType = entityType.GetProperty("Id")!.PropertyType;

        Console.WriteLine($"Created entity type {entityType.Name} with Id type {idType.Name}");

        var enumTypeName = typeof(TEnum).Name;
        var excludedEnumValues = EnumEntityHelper.GetExcludedEnumValues();
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
            .Where(x => !excludedEnumValues.Contains(x.ToString())).ToList();

        var maxNameLength = enumValues
            .Select(e => e.ToString().Length)
            .Max();

        var builder = modelBuilder.Entity(entityType);
        
        var tableName = (enumTypeName + "s").ToSnakeCase();
        var idColumnName = $"{enumTypeName}Id".ToSnakeCase();
        builder.HasKey(nameof(EnumEntity<TEnum>.Id))
            .HasName($"pk_{tableName}");
    
        var idPropertyBuilder = builder.Property(nameof(EnumEntity<TEnum>.Id))
            .HasColumnName(idColumnName)
            .ValueGeneratedNever();
        
        var nameColumnName = $"{enumTypeName}Name".ToSnakeCase();
        builder.Property(nameof(EnumEntity<TEnum>.Name))
            .HasColumnName(nameColumnName)
            .HasMaxLength(maxNameLength)
            .IsRequired();
        
        builder.ToTable(tableName);
        
        var optimalIdType = EnumEntityHelper.GetOptimalIdType<TEnum>();
        if (optimalIdType == typeof(byte))
        {
            idPropertyBuilder.HasConversion<byte>();
        }
        else if (optimalIdType == typeof(short))
        {
            idPropertyBuilder.HasConversion<short>();
        }
        else
        {
            idPropertyBuilder.HasConversion<int>();
        }
        
        Console.WriteLine($"Created table {tableName} with columns {idColumnName} ({idType.Name}) and {nameColumnName} (nvarchar({maxNameLength}))");

        var ignoredEnumValues = EnumEntityHelper.GetExcludedEnumValues();
        var seedData = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Where(x => !ignoredEnumValues.Contains(x.ToString()))
            .Select(e =>
            {
                var instance = Activator.CreateInstance(entityType)!;
            
                object idValue;
                if (idType == typeof(byte))
                {
                    idValue = Convert.ToByte(e);
                }
                else if (idType == typeof(short))
                {
                    idValue = Convert.ToInt16(e);
                }
                else
                {
                    idValue = Convert.ToInt32(e);
                }
            
                entityType.GetProperty(nameof(EnumEntity<TEnum>.Id))!.SetValue(instance, idValue);
                entityType.GetProperty(nameof(EnumEntity<TEnum>.Name))!.SetValue(instance, e.ToString());
            
                return instance;
            })
            .ToList();

        Console.WriteLine($"Seeding {seedData.Count} records into {tableName} table ...");

        builder.HasData(seedData);

        Console.WriteLine($"Seeded {seedData.Count} records into {tableName} table");

        builder.HasIndex(nameof(EnumEntity<TEnum>.Name)).IsUnique();
        
        Console.WriteLine($"Created unique index on {nameColumnName} column in {tableName} table");
    }

    
    /// <summary>
    /// Configures a relationship between an entity and an enum table.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to configure the relationship for.</typeparam>
    /// <typeparam name="TEnum">The enum type to configure the relationship for.</typeparam>
    /// <param name="modelBuilder">The model builder to configure the relationship on.</param>
    /// <param name="propertyExpression">The property expression representing the enum property on the entity.</param>
    public static void ConfigureEnumRelationship<TEntity, TEnum>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntity, TEnum>> propertyExpression) 
        where TEntity : class
        where TEnum : Enum
    {
        Console.WriteLine($"Configuring enum relationship for {typeof(TEntity).Name}. and {typeof(TEnum).Name}...");

        var entityType = EnumEntityHelper.BuildEnumEntity<TEnum>();

        var memberExpression = propertyExpression.Body as MemberExpression;
        var propertyInfo = memberExpression!.Member as PropertyInfo;
        var propertyName = propertyInfo!.Name;
    
        var propertyBuilder = modelBuilder
            .Entity<TEntity>()
            .Property(propertyExpression);
    
        var optimalIdType = EnumEntityHelper.GetOptimalIdType<TEnum>();
        if (optimalIdType == typeof(byte))
        {
            propertyBuilder.HasConversion<byte>();
        }
        else if (optimalIdType == typeof(short))
        {
            propertyBuilder.HasConversion<short>();
        }
        else
        {
            propertyBuilder.HasConversion<int>();
        }

        var efEntityType = modelBuilder.Model.FindEntityType(typeof(TEntity));
        var entityTableName = efEntityType!.GetTableName();
        var enumTableName = typeof(TEnum).Name + "s";

        modelBuilder.Entity<TEntity>()
            .HasOne(entityType)
            .WithMany()
            .HasForeignKey(propertyName)
            .HasPrincipalKey(nameof(EnumEntity<TEnum>.Id))
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName($"fk_{entityTableName}_{enumTableName.ToSnakeCase()}_{propertyName.ToSnakeCase()}");

        Console.WriteLine($"Created relationship between {typeof(TEntity).Name} and {typeof(TEnum).Name} using property {propertyName}");
    }
}