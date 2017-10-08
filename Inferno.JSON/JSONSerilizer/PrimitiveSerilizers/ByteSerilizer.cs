using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityByteSerilier : IFullSerilizer<byte>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityByteSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(byte obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(byte obj, TextWriter textWriter, char[] writeBuffer)
    {
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)obj, writeBuffer);
    }
  }

  class EntityByteNullSerilier : IFullSerilizer<byte?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityByteNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(byte? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(byte? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructByteSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, byte> m_valueExtractor;

    public StructByteSerilizer(string properyName, RefValueExtractor<T, byte> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableByteSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, byte?> m_valueExtractor;

    public StructNullableByteSerilizer(string properyName, RefValueExtractor<T, byte?> valueExtractor)
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
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }


  class ByteSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, byte> m_valueExtractor;

    public ByteSerilizer(string properyName, Func<T, byte> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableByteSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, byte?> m_valueExtractor;

    public NullableByteSerilizer(string properyName, Func<T, byte?> valueExtractor)
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
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}