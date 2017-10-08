using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inferno.JSONSerilizer.OtherSerilizers;
using Inferno.JSONSerilizer.OtherSerilizers.Arrays;
using Inferno.JSONSerilizer.OtherSerilizers.Class;
using Inferno.JSONSerilizer.OtherSerilizers.Enum;
using Inferno.JSONSerilizer.OtherSerilizers.Struct;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno.JSONSerilizer
{
  static class PropertyFactory
  {
    private readonly static ConcurrentDictionary<Type, IEnumPropertySerilizer> m_enumSerilizerFactories = new ConcurrentDictionary<Type, IEnumPropertySerilizer>();

    public static bool TryCreatePropertySerilizer<T>(JSONConfiguration configuration,GroupJson json,Type propType, string propName, object valueExtractor, PropertyInfo propertyInfo,SessionSerilizerPack sessionSerilizerPack, out IProperySerilizer<T> serilizer)
    {
      if (propType == typeof(bool))
      {
        serilizer = new BoolSerilizer<T>(propName, (Func<T,bool>)valueExtractor);
      }
      else if (propType == typeof(bool?))
      {
        serilizer = new NullBoolSerilizer<T>(propName, (Func<T, bool?>)valueExtractor);
      }
      else if (propType == typeof(byte))
      {
        serilizer = new ByteSerilizer<T>(propName, (Func<T, byte>)valueExtractor);
      }
      else if (propType == typeof(byte?))
      {
        serilizer = new NullableByteSerilizer<T>(propName, (Func<T, byte?>)valueExtractor);
      }
      else if (propType == typeof(sbyte))
      {
        serilizer = new SByteSerilizer<T>(propName, (Func<T, sbyte>)valueExtractor);
      }
      else if (propType == typeof(sbyte?))
      {
        serilizer = new NullableSByteSerilizer<T>(propName, (Func<T, sbyte?>)valueExtractor);
      }
      else if (propType == typeof(char))
      {
        serilizer = new CharSerilizer<T>(propName, (Func<T, char>)valueExtractor);
      }
      else if (propType == typeof(char?))
      {
        serilizer = new NullableCharSerilizer<T>(propName, (Func<T, char?>)valueExtractor);
      }
      else if (propType == typeof(decimal))
      {
        serilizer = new DecimalSerilizer<T>(propName, (Func<T, decimal>)valueExtractor);
      }
      else if (propType == typeof(decimal?))
      {
        serilizer = new NullableDecimalSerilizer<T>(propName, (Func<T, decimal?>)valueExtractor);
      }
      else if (propType == typeof(double))
      {
        serilizer = new DoubleSerilizer<T>(propName, (Func<T, double>)valueExtractor);
      }
      else if (propType == typeof(double?))
      {
        serilizer = new NullableDoubleSerilizer<T>(propName, (Func<T, double?>)valueExtractor);
      }
      else if (propType == typeof(float))
      {
        serilizer = new FloatSerilizer<T>(propName, (Func<T, float>)valueExtractor);
      }
      else if (propType == typeof(float?))
      {
        serilizer = new NullableFloatSerilizer<T>(propName, (Func<T, float?>)valueExtractor);
      }
      else if (propType == typeof(int))
      {
        serilizer = new IntSerilizer<T>(propName, (Func<T, int>)valueExtractor);
      }
      else if (propType == typeof(int?))
      {
        serilizer = new NullableIntSerilizer<T>(propName, (Func<T, int?>)valueExtractor);
      }
      else if (propType == typeof(uint))
      {
        serilizer = new UIntSerilizer<T>(propName, (Func<T, uint>)valueExtractor);
      }
      else if (propType == typeof(uint?))
      {
        serilizer = new NullableUIntSerilizer<T>(propName, (Func<T, uint?>)valueExtractor);
      }
      else if (propType == typeof(long))
      {
        serilizer = new LongSerilizer<T>(propName, (Func<T, long>)valueExtractor);
      }
      else if (propType == typeof(long?))
      {
        serilizer = new NullableLongSerilizer<T>(propName, (Func<T, long?>)valueExtractor);
      }
      else if (propType == typeof(ulong))
      {
        serilizer = new ULongSerilizer<T>(propName, (Func<T, ulong>)valueExtractor);
      }
      else if (propType == typeof(ulong?))
      {
        serilizer = new NullableULongSerilizer<T>(propName, (Func<T, ulong?>)valueExtractor);
      }
      else if (propType == typeof(short))
      {
        serilizer = new ShortSerilizer<T>(propName, (Func<T, short>)valueExtractor);
      }
      else if (propType == typeof(short?))
      {
        serilizer = new NullableShortSerilizer<T>(propName, (Func<T, short?>)valueExtractor);
      }
      else if (propType == typeof(ushort))
      {
        serilizer = new UShortSerilizer<T>(propName, (Func<T, ushort>)valueExtractor);
      }
      else if (propType == typeof(ushort?))
      {
        serilizer = new NullableUShortSerilizer<T>(propName, (Func<T, ushort?>)valueExtractor);
      }
      else if (propType == typeof(string))
      {
        serilizer = new StringSerilizer<T>(propName, (Func<T, string>)valueExtractor);
      }
      else if (propType.IsEnum)
      {
        var enumSerilizerFactory = m_enumSerilizerFactories.GetOrAdd(propType, type =>
        {
          var enumTypeDef = typeof(EnumPropertySerilizer<>);
          var genericType = enumTypeDef.MakeGenericType(propType);
          return (IEnumPropertySerilizer) Activator.CreateInstance(genericType);
        });

        serilizer = enumSerilizerFactory.CreateEnumPropertySerilizer<T>(configuration, propName, valueExtractor);
        return true;
      }
      else if (propType.IsValueType)
      {
        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          var enumtype = typeof(T).GetGenericArguments().Single();
          if (enumtype.IsEnum)
          {
            var enumSerilizerFactory = m_enumSerilizerFactories.GetOrAdd(enumtype, type =>
            {
              var enumTypeDef = typeof(EnumPropertySerilizer<>);
              var genericType = enumTypeDef.MakeGenericType(propType);
              return (IEnumPropertySerilizer) Activator.CreateInstance(genericType);
            });

            serilizer = enumSerilizerFactory.CreateNullableEnumPropertySerilizer<T>(configuration, propName,
              valueExtractor);
            return true;
          }
        }

        serilizer = HandleClassType(propType, sessionSerilizerPack, configuration, json).CreateSubClassSerilizer<T>(propName, valueExtractor);

        return true;
      }
      else // This is class
      {
        serilizer = HandleClassType(propType, sessionSerilizerPack, configuration, json).CreateSubClassSerilizer<T>(propName, valueExtractor);
        return true;
      }

      return serilizer != null;
    }

    public static bool TryCreateStructPropertySerilizer<T>(JSONConfiguration configuration, GroupJson json, Type propType, string propName, object valueExtractor, PropertyInfo propertyInfo, SessionSerilizerPack sessionSerilizerPack, out IStructProperySerilizer<T> serilizer)
    {
      if (propType == typeof(bool))
      {
        serilizer = new StructBoolSerilizer<T>(propName, (RefValueExtractor<T, bool>)valueExtractor);
      }
      else if (propType == typeof(bool?))
      {
        serilizer = new StructNullBoolSerilizer<T>(propName, (RefValueExtractor<T, bool?>)valueExtractor);
      }
      else if (propType == typeof(byte))
      {
        serilizer = new StructByteSerilizer<T>(propName, (RefValueExtractor<T, byte>)valueExtractor);
      }
      else if (propType == typeof(byte?))
      {
        serilizer = new StructNullableByteSerilizer<T>(propName, (RefValueExtractor<T, byte?>)valueExtractor);
      }
      else if (propType == typeof(sbyte))
      {
        serilizer = new StructSByteSerilizer<T>(propName, (RefValueExtractor<T, sbyte>)valueExtractor);
      }
      else if (propType == typeof(sbyte?))
      {
        serilizer = new StructNullableSByteSerilizer<T>(propName, (RefValueExtractor<T, sbyte?>)valueExtractor);
      }
      else if (propType == typeof(char))
      {
        serilizer = new StructCharSerilizer<T>(propName, (RefValueExtractor<T, char>)valueExtractor);
      }
      else if (propType == typeof(char?))
      {
        serilizer = new StructNullableCharSerilizer<T>(propName, (RefValueExtractor<T, char?>)valueExtractor);
      }
      else if (propType == typeof(decimal))
      {
        serilizer = new StructDecimalSerilizer<T>(propName, (RefValueExtractor<T, decimal>)valueExtractor);
      }
      else if (propType == typeof(decimal?))
      {
        serilizer = new StructNullableDecimalSerilizer<T>(propName, (RefValueExtractor<T, decimal?>)valueExtractor);
      }
      else if (propType == typeof(double))
      {
        serilizer = new StructDoubleSerilizer<T>(propName, (RefValueExtractor<T, double>)valueExtractor);
      }
      else if (propType == typeof(double?))
      {
        serilizer = new StructNullableDoubleSerilizer<T>(propName, (RefValueExtractor<T, double?>)valueExtractor);
      }
      else if (propType == typeof(float))
      {
        serilizer = new StructFloatSerilizer<T>(propName, (RefValueExtractor<T, float>)valueExtractor);
      }
      else if (propType == typeof(float?))
      {
        serilizer = new StructNullableFloatSerilizer<T>(propName, (RefValueExtractor<T, float?>)valueExtractor);
      }
      else if (propType == typeof(int))
      {
        serilizer = new StructIntSerilizer<T>(propName, (RefValueExtractor<T, int>)valueExtractor);
      }
      else if (propType == typeof(int?))
      {
        serilizer = new StructNullableIntSerilizer<T>(propName, (RefValueExtractor<T, int?>)valueExtractor);
      }
      else if (propType == typeof(uint))
      {
        serilizer = new StructUIntSerilizer<T>(propName, (RefValueExtractor<T, uint>)valueExtractor);
      }
      else if (propType == typeof(uint?))
      {
        serilizer = new StructNullableUIntSerilizer<T>(propName, (RefValueExtractor<T, uint?>)valueExtractor);
      }
      else if (propType == typeof(long))
      {
        serilizer = new StructLongSerilizer<T>(propName, (RefValueExtractor<T, long>)valueExtractor);
      }
      else if (propType == typeof(long?))
      {
        serilizer = new StructNullableLongSerilizer<T>(propName, (RefValueExtractor<T, long?>)valueExtractor);
      }
      else if (propType == typeof(ulong))
      {
        serilizer = new StructULongSerilizer<T>(propName, (RefValueExtractor<T, ulong>)valueExtractor);
      }
      else if (propType == typeof(ulong?))
      {
        serilizer = new StructNullableULongSerilizer<T>(propName, (RefValueExtractor<T, ulong?>)valueExtractor);
      }
      else if (propType == typeof(short))
      {
        serilizer = new StructShortSerilizer<T>(propName, (RefValueExtractor<T, short>)valueExtractor);
      }
      else if (propType == typeof(short?))
      {
        serilizer = new StructNullableShortSerilizer<T>(propName, (RefValueExtractor<T, short?>)valueExtractor);
      }
      else if (propType == typeof(ushort))
      {
        serilizer = new StructUShortSerilizer<T>(propName, (RefValueExtractor<T, ushort>)valueExtractor);
      }
      else if (propType == typeof(ushort?))
      {
        serilizer = new StructNullableUShortSerilizer<T>(propName, (RefValueExtractor<T, ushort?>)valueExtractor);
      }
      else if (propType == typeof(string))
      {
        serilizer = new StructStringSerilizer<T>(propName, (RefValueExtractor<T, string>)valueExtractor);
      }
      else if (propType.IsEnum)
      {
        var enumSerilizerFactory = m_enumSerilizerFactories.GetOrAdd(propType, type =>
        {
          var enumTypeDef = typeof(EnumPropertySerilizer<>);
          var genericType = enumTypeDef.MakeGenericType(propType);
          return (IEnumPropertySerilizer)Activator.CreateInstance(genericType);
        });

        serilizer = enumSerilizerFactory.CreateStructEnumPropertySerilizer<T>(configuration, propName, valueExtractor);
        return true;
      }
      else if (propType.IsValueType)
      {
        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          var enumtype = typeof(T).GetGenericArguments().Single();
          if (enumtype.IsEnum)
          {
            var enumSerilizerFactory = m_enumSerilizerFactories.GetOrAdd(enumtype, type =>
            {
              var enumTypeDef = typeof(EnumPropertySerilizer<>);
              var genericType = enumTypeDef.MakeGenericType(propType);
              return (IEnumPropertySerilizer)Activator.CreateInstance(genericType);
            });

            serilizer = enumSerilizerFactory.CreateStructNullableEnumPropertySerilizer<T>(configuration, propName,valueExtractor);
            return true;
          }
        }

        // We handle struct as a class
        serilizer = HandleClassType(propType, sessionSerilizerPack, configuration, json).CreateStructSubClassSerilizer<T>(propName, valueExtractor);
        return true;
      }
      else // This is class
      {
        serilizer = HandleClassType(propType, sessionSerilizerPack, configuration, json).CreateStructSubClassSerilizer<T>(propName, valueExtractor);
        return true;
      }

      return serilizer != null;
    }

    private static IClassSerilizerFactory HandleClassType(Type subclassType, SessionSerilizerPack sessionSerilizerPack,JSONConfiguration configuration,GroupJson groupJson)
    {
      IClassSerilizerFactory factory;
      if (!sessionSerilizerPack.TryGetSerilizerFactory(subclassType, out factory))
      {
        if (subclassType.IsArray)
        {
          var genericMethod = _handleArrayTypeInternalMethod.MakeGenericMethod(subclassType.GetElementType());
          factory = (IClassSerilizerFactory)genericMethod.Invoke(null, new object[] { sessionSerilizerPack, configuration, groupJson });
        }
        else if (typeof(IEnumerable).IsAssignableFrom(subclassType))
        {
          if (subclassType.IsGenericType && subclassType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
          {
            var genericMethod = _handleEnumerableTypeInternalMethod.MakeGenericMethod(subclassType, subclassType.GetGenericArguments()[0]);
            factory = (IClassSerilizerFactory)genericMethod.Invoke(null, new object[] { sessionSerilizerPack, configuration, groupJson });
          }
          else
          {
            var interfaces = subclassType.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
              if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
              {
                var genericMethod = _handleEnumerableTypeInternalMethod.MakeGenericMethod(subclassType, subclassType.GetGenericArguments()[0]);
                factory = (IClassSerilizerFactory)genericMethod.Invoke(null, new object[] { sessionSerilizerPack, configuration, groupJson });
                break;
              }
            }
          }
        }
        else // Simple class
        {
          var genericMethod = _handleClassTypeInternalMethod.MakeGenericMethod(subclassType);
          factory = (IClassSerilizerFactory)genericMethod.Invoke(null, new object[] { sessionSerilizerPack, configuration, groupJson });
        }
      }

      return factory;
    }

    private static readonly MethodInfo _handleClassTypeInternalMethod = typeof(PropertyFactory).GetMethod("HandleClassTypeInternal",BindingFlags.NonPublic | BindingFlags.Static);
    private static IClassSerilizerFactory HandleClassTypeInternal<TSubClass>(SessionSerilizerPack sessionSerilizerPack, JSONConfiguration configuration, GroupJson groupJson)
    {
      SerilizerDefinition<TSubClass> serilizerDefinition = new SerilizerDefinition<TSubClass>(configuration, sessionSerilizerPack, groupJson);
      serilizerDefinition.AutoDefine();
      IClassSerilizerFactory factory = serilizerDefinition.CreateSerilizerFactory();
      return factory;
    }

    private static readonly MethodInfo _handleArrayTypeInternalMethod = typeof(PropertyFactory).GetMethod("HandleArrayTypeInternal", BindingFlags.NonPublic | BindingFlags.Static);
    private static IClassSerilizerFactory HandleArrayTypeInternal<TSubClass>(SessionSerilizerPack sessionSerilizerPack, JSONConfiguration configuration, GroupJson groupJson)
    {
      IClassSerilizerFactory factory ;
      if (!sessionSerilizerPack.TryGetSerilizerFactory(typeof(TSubClass[]), out factory))
      {
        var lazySerilizer = new LazyTopLevelArrayJsonSerilizer<TSubClass>();
        var lazyFactory = new ClassSerilizerFactory<TSubClass[]>(lazySerilizer);
        factory = lazyFactory;
        sessionSerilizerPack.SetSerilizerFactoy(lazyFactory);
        var subTypeFactory = (IFullSerilizer<TSubClass>)HandleClassType(typeof(TSubClass), sessionSerilizerPack, configuration, groupJson).GetFullSerilizer();
        var topLevelSerilizer = new TopLevelArrayJsonSerilizer<TSubClass>( subTypeFactory,configuration);
        lazySerilizer.Init(subTypeFactory, configuration);
        lazyFactory.OverrideSerilizer(topLevelSerilizer);
      }

      return factory;
    }

    private static readonly MethodInfo _handleEnumerableTypeInternalMethod = typeof(PropertyFactory).GetMethod("HandleEnumerableTypeInternal", BindingFlags.NonPublic | BindingFlags.Static);
    private static IClassSerilizerFactory HandleEnumerableTypeInternal<THost,TSubClass>(SessionSerilizerPack sessionSerilizerPack, JSONConfiguration configuration, GroupJson groupJson) where THost : IEnumerable<TSubClass>
    {
      IClassSerilizerFactory factory;
      if (!sessionSerilizerPack.TryGetSerilizerFactory(typeof(THost), out factory))
      {
        var lazySerilizer = new LazyTopLevelEnumerableJsonSerilizer<THost,TSubClass>();
        var lazyFactory = new ClassSerilizerFactory<THost>(lazySerilizer);
        factory = lazyFactory;
        sessionSerilizerPack.SetSerilizerFactoy(lazyFactory);

        var subTypeFactory = (IFullSerilizer<TSubClass>)HandleClassType(typeof(TSubClass), sessionSerilizerPack, configuration, groupJson).GetFullSerilizer();
        
        var topLevelSerilizer = new TopLevelEnumerableJsonSerilizer<THost,TSubClass>(subTypeFactory, configuration);
        lazySerilizer.Init(subTypeFactory, configuration);
        lazyFactory.OverrideSerilizer(topLevelSerilizer);
      }

      return factory;
    }
  }
}
