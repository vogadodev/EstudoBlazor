using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.EnumService
{   
    /// <summary>
    /// Contém métodos de extensão modernos e performáticos para Enums,
    /// utilizando cache para otimizar o acesso aos atributos Description.
    /// </summary>
    public static class ConverterEnumService
    {
        private static readonly ConcurrentDictionary<string, string> DescriptionCache = new();
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, object>> ValueFromDescriptionCache = new();
       
        public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
        {
            string key = $"{typeof(TEnum).FullName}.{enumValue}";

            return DescriptionCache.GetOrAdd(key, _ =>
            {                
                var descriptionAttribute = enumValue
                    .GetType()
                    .GetField(enumValue.ToString())
                    ?.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description ?? enumValue.ToString();
            });
        }

    }
}
