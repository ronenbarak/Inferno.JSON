using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityShortSerilizer : IFullSerilizer<short>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityShortSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(short obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(short obj, TextWriter textWriter, char[] writeBuffer)
    {
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)obj, writeBuffer);
    }
  }

  class EntityShortNullSerilizer : IFullSerilizer<short?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityShortNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(short? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(short? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructShortSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, short> m_valueExtractor;

    public StructShortSerilizer(string properyName, RefValueExtractor<T, short> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableShortSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, short?> m_valueExtractor;

    public StructNullableShortSerilizer(string properyName, RefValueExtractor<T, short?> valueExtractor)
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
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class ShortSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, short> m_valueExtractor;

    public ShortSerilizer(string properyName, Func<T, short> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableShortSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, short?> m_valueExtractor;

    public NullableShortSerilizer(string properyName, Func<T, short?> valueExtractor)
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
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}