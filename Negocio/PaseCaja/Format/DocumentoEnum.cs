using System;

namespace Negocio.Repositorios.PaseCaja.Format
{
    public enum DocumentoEnum
    {
        [DocumentoInfoAttribute(typeof(Declaracion))]
        PaseCaja,
    }

    public sealed class DocumentoInfoAttribute : Attribute
    {
        private readonly Type _type;

        public DocumentoInfoAttribute(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }
    }

    public static class Extensions
    {
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribs = field.GetCustomAttributes(typeof(T), false);
            var result = default(T);

            if (attribs.Length > 0)
            {
                result = attribs[0] as T;
            }

            return result;
        }
    }
}