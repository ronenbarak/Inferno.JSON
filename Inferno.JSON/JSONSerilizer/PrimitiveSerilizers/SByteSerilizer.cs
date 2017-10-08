using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntitySByteSerilizer : IFullSerilizer<sbyte>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntitySByteSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(sbyte obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(sbyte obj, TextWriter textWriter, char[] writeBuffer)
    {
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)obj, writeBuffer);
    }
  }

  class EntitySByteNullSerilizer : IFullSerilizer<sbyte?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntitySByteNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(sbyte? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(sbyte? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, (int)obj, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructSByteSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, sbyte> m_valueExtractor;

    public StructSByteSerilizer(string properyName, RefValueExtractor<T, sbyte> valueExtractor)
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

  class StructNullableSByteSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, sbyte?> m_valueExtractor;

    public StructNullableSByteSerilizer(string properyName, RefValueExtractor<T, sbyte?> valueExtractor)
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

  class SByteSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, sbyte> m_valueExtractor;

    public SByteSerilizer(string properyName, Func<T, sbyte> valueExtractor)
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

  class NullableSByteSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, sbyte?> m_valueExtractor;

    public NullableSByteSerilizer(string properyName, Func<T, sbyte?> valueExtractor)
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