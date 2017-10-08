using System;
using System.IO;
using System.Runtime.CompilerServices;
using GrisuDotNet;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityULongSerilizer : IFullSerilizer<ulong>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityULongSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(ulong obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(ulong obj, TextWriter textWriter, char[] writeBuffer)
    {
      ULongCustomSerilizer._CustomWriteULong(textWriter, obj, writeBuffer);
    }
  }

  class EntityULongNullSerilizer : IFullSerilizer<ulong?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityULongNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(ulong? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(ulong? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        ULongCustomSerilizer._CustomWriteULong(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructULongSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, ulong> m_valueExtractor;

    public StructULongSerilizer(string properyName, RefValueExtractor<T, ulong> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      ULongCustomSerilizer._CustomWriteULong(textWriter, m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableULongSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, ulong?> m_valueExtractor;

    public StructNullableULongSerilizer(string properyName, RefValueExtractor<T, ulong?> valueExtractor)
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
        ULongCustomSerilizer._CustomWriteULong(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class ULongSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, ulong> m_valueExtractor;

    public ULongSerilizer(string properyName, Func<T, ulong> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize( T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      ULongCustomSerilizer._CustomWriteULong(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableULongSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, ulong?> m_valueExtractor;

    public NullableULongSerilizer(string properyName, Func<T, ulong?> valueExtractor)
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
        ULongCustomSerilizer._CustomWriteULong(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  static class ULongCustomSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _CustomWriteULong(TextWriter writer, ulong number, char[] buffer)
    {
      var ptr = JSONConfiguration.SerilizerCharBufferSize - 1;

      var copy = number;

      do
      {
        byte ix = (byte)(copy % 100);
        copy /= 100;

        var chars = Utils.DigitPairs[ix];
        buffer[ptr--] = chars.Second;
        buffer[ptr--] = chars.First;
      } while (copy != 0);

      if (buffer[ptr + 1] == '0')
      {
        ptr++;
      }

      writer.Write(buffer, ptr + 1, JSONConfiguration.SerilizerCharBufferSize - 1 - ptr);
    }
  }
}