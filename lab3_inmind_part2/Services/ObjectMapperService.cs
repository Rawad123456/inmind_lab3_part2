using System.Reflection;

namespace lab3_inmind_part2.Services
{
    public class ObjectMapperService
    {
        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null) return default;

            var destination = new TDestination();

            var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var destProp in destProps)
            {
                var sourceProp = sourceProps.FirstOrDefault(p =>
                    p.Name == destProp.Name &&
                    p.PropertyType == destProp.PropertyType);

                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    destProp.SetValue(destination, value);
                }
            }

            return destination;
        }
    }
}