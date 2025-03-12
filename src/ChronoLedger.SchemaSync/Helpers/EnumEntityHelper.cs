using System.Reflection;
using System.Reflection.Emit;

namespace ChronoLedger.SchemaSync.Helpers;

/// <summary>
/// Helper class for generating entity types for enums.
/// </summary>
public static class EnumEntityHelper
{
    private static readonly Dictionary<Type, Type> EnumEntityTypes = new();

    /// <summary>
    /// Builds an entity type for the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to build the entity type for.</typeparam>
    /// <returns>The entity type for the specified enum type.</returns>   
    public static Type BuildEnumEntity<TEnum>() where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        
        if (EnumEntityTypes.TryGetValue(enumType, out var existingType))
        {
            return existingType;
        }
        
        var typeName = $"{enumType.Namespace}.{enumType.Name}Entity";
        
        var assemblyName = new AssemblyName($"{enumType.Assembly.GetName().Name}.DynamicEnumEntities");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        
        var typeBuilder = moduleBuilder.DefineType(typeName, 
            TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | 
            TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | 
            TypeAttributes.AutoLayout);
        
        AddProperty(typeBuilder, "Id", enumType);
        AddProperty(typeBuilder, "Name", typeof(string));
        
        var createdType = typeBuilder.CreateType();
        EnumEntityTypes[enumType] = createdType;
        
        return createdType;
    }
    
    /// <summary>
    /// Adds a property to the specified type builder.
    /// </summary>
    /// <param name="typeBuilder">The type builder to add the property to.</param>
    /// <param name="propertyName">The name of the property to add.</param>
    /// <param name="propertyType">The type of the property to add.</param>
    private static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
    {
        var fieldBuilder = typeBuilder.DefineField($"_{propertyName.ToLower()}", propertyType, FieldAttributes.Private);
        
        var propertyBuilder = typeBuilder.DefineProperty(propertyName, 
            PropertyAttributes.HasDefault, 
            propertyType, 
            null);
        
        var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            propertyType,
            Type.EmptyTypes);
        
        var getMethodIlGenerator = getMethodBuilder.GetILGenerator();
        getMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        getMethodIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
        getMethodIlGenerator.Emit(OpCodes.Ret);
        
        var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            null,
            [propertyType]);
        
        var setMethodIlGenerator = setMethodBuilder.GetILGenerator();
        setMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        setMethodIlGenerator.Emit(OpCodes.Ldarg_1);
        setMethodIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);
        setMethodIlGenerator.Emit(OpCodes.Ret);
        
        propertyBuilder.SetGetMethod(getMethodBuilder);
        propertyBuilder.SetSetMethod(setMethodBuilder);
    }

    /// <summary>
    /// Determines the optimal ID type based on the number of enum values.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to determine the optimal ID type for.</typeparam>
    /// <returns>The optimal ID type for the enum.</returns>
    public static Type GetOptimalIdType<TEnum>() where TEnum : Enum
    {
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

        var ignoredEnumValues = GetExcludedEnumValues();
        var enumCount = enumValues.Count(x => !ignoredEnumValues.Contains(x.ToString()));

        var maxValue = enumValues
            .Select(e => Convert.ToInt64(e))
            .Max();

        return maxValue switch
        {
            <= byte.MaxValue when enumCount <= 255 => typeof(byte),
            <= short.MaxValue when enumCount <= 32767 => typeof(short),
            _ => typeof(int)
        };
    }

    /// <summary>
    /// Gets the ignored enum values.
    /// </summary>
    /// <returns>The set of ignored enum values.</returns>
    public static HashSet<string> GetExcludedEnumValues()
    {
        return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "NotSpecified",
        };
    }
}