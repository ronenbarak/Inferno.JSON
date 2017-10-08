using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityFloatSerilier : IFullSerilizer<float>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityFloatSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(float obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(float obj, TextWriter textWriter, char[] writeBuffer)
    {
      GrisuDotNet.Grisu.DoubleToString((double)obj, textWriter, writeBuffer);
    }
  }

  class EntityFloatNullSerilier : IFullSerilizer<float?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityFloatNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(float? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(float? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        GrisuDotNet.Grisu.DoubleToString((double)obj.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructFloatSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, float> m_valueExtractor;

    public StructFloatSerilizer(string properyName, RefValueExtractor<T, float> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GrisuDotNet.Grisu.DoubleToString((double)m_valueExtractor.Invoke(ref obj), textWriter, writeBuffer);
    }
  }

  class StructNullableFloatSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, float?> m_valueExtractor;

    public StructNullableFloatSerilizer(string properyName, RefValueExtractor<T, float?> valueExtractor)
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
        GrisuDotNet.Grisu.DoubleToString((double)value.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class FloatSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, float> m_valueExtractor;

    public FloatSerilizer(string properyName, Func<T, float> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GrisuDotNet.Grisu.DoubleToString((double)m_valueExtractor.Invoke(obj), textWriter, writeBuffer);
    }
  }

  class NullableFloatSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, float?> m_valueExtractor;

    public NullableFloatSerilizer(string properyName, Func<T, float?> valueExtractor)
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
        GrisuDotNet.Grisu.DoubleToString((double)value.Value, textWriter, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}