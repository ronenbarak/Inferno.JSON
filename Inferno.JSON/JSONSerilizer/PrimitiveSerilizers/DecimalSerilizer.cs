using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityDecimalSerilier : IFullSerilizer<decimal>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDecimalSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(decimal obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(decimal obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(obj.ToString(CultureInfo.InvariantCulture));
    }
  }

  class EntityDecimalNullSerilier : IFullSerilizer<decimal?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDecimalNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(decimal? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(decimal? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        textWriter.Write(obj.Value.ToString(CultureInfo.InvariantCulture));
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructDecimalSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, decimal> m_valueExtractor;

    public StructDecimalSerilizer(string properyName, RefValueExtractor<T, decimal> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      textWriter.Write(m_valueExtractor.Invoke(ref obj).ToString(CultureInfo.InvariantCulture));
    }
  }

  class StructNullableDecimalSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, decimal?> m_valueExtractor;

    public StructNullableDecimalSerilizer(string properyName, RefValueExtractor<T, decimal?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (!value.HasValue)
      {
        textWriter.Write(m_propertyNull);
      }
      else
      {
        textWriter.Write(m_property);
        textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
      }
    }
  }

  class DecimalSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, decimal> m_valueExtractor;

    public DecimalSerilizer(string properyName, Func<T, decimal> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      textWriter.Write(m_valueExtractor.Invoke(obj).ToString(CultureInfo.InvariantCulture));
    }
  }

  class NullableDecimalSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, decimal?> m_valueExtractor;

    public NullableDecimalSerilizer(string properyName, Func<T, decimal?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (!value.HasValue)
      {
        textWriter.Write(m_propertyNull);
      }
      else
      {
        textWriter.Write(m_property);
        textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
      }
    }
  }
}