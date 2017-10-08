using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityDoubleSerilier : IFullSerilizer<double>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDoubleSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(double obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(double obj, TextWriter textWriter, char[] writeBuffer)
    {
      GrisuDotNet.Grisu.DoubleToString(obj, textWriter, writeBuffer);
    }
  }

  class EntityDoubleNullSerilier : IFullSerilizer<double?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDoubleNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(double? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(double? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        GrisuDotNet.Grisu.DoubleToString(obj.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructDoubleSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, double> m_valueExtractor;

    public StructDoubleSerilizer(string properyName, RefValueExtractor<T, double> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GrisuDotNet.Grisu.DoubleToString(m_valueExtractor.Invoke(ref obj),textWriter,writeBuffer);
    }
  }

  class StructNullableDoubleSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, double?> m_valueExtractor;

    public StructNullableDoubleSerilizer(string properyName, RefValueExtractor<T, double?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        GrisuDotNet.Grisu.DoubleToString(value.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class DoubleSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, double> m_valueExtractor;

    public DoubleSerilizer(string properyName, Func<T, double> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GrisuDotNet.Grisu.DoubleToString(m_valueExtractor.Invoke(obj), textWriter, writeBuffer);
    }
  }

  class NullableDoubleSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, double?> m_valueExtractor;

    public NullableDoubleSerilizer(string properyName, Func<T, double?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        GrisuDotNet.Grisu.DoubleToString(value.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}