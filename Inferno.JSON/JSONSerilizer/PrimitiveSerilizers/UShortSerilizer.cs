using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityUShortSerilizer : IFullSerilizer<ushort>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityUShortSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(ushort obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(ushort obj, TextWriter textWriter, char[] writeBuffer)
    {
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, (uint)obj, writeBuffer);
    }
  }

  class EntityUShortNullSerilizer : IFullSerilizer<ushort?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityUShortNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(ushort? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(ushort? obj, TextWriter textWriter, char[] writeBuffer)
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

  class StructUShortSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, ushort> m_valueExtractor;

    public StructUShortSerilizer(string properyName, RefValueExtractor<T, ushort> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableUShortSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, ushort?> m_valueExtractor;

    public StructNullableUShortSerilizer(string properyName, RefValueExtractor<T, ushort?> valueExtractor)
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
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class UShortSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, ushort> m_valueExtractor;

    public UShortSerilizer(string properyName, Func<T, ushort> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableUShortSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, ushort?> m_valueExtractor;

    public NullableUShortSerilizer(string properyName, Func<T, ushort?> valueExtractor)
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
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}