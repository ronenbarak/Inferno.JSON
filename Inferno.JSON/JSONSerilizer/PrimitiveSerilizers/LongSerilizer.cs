using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityLongSerilizer : IFullSerilizer<long>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityLongSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(long obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(long obj, TextWriter textWriter, char[] writeBuffer)
    {
      LongCustomSerilizer._CustomWriteLong(textWriter, obj, writeBuffer);
    }
  }

  class EntityLongNullSerilizer : IFullSerilizer<long?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityLongNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(long? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(long? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        LongCustomSerilizer._CustomWriteLong(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructLongSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, long> m_valueExtractor;

    public StructLongSerilizer(string properyName, RefValueExtractor<T, long> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      LongCustomSerilizer._CustomWriteLong(textWriter, m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableLongSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, long?> m_valueExtractor;

    public StructNullableLongSerilizer(string properyName, RefValueExtractor<T, long?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_property = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        LongCustomSerilizer._CustomWriteLong(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class LongSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, long> m_valueExtractor;

    public LongSerilizer(string properyName, Func<T, long> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      LongCustomSerilizer._CustomWriteLong(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableLongSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, long?> m_valueExtractor;

    public NullableLongSerilizer(string properyName, Func<T, long?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_property = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        LongCustomSerilizer._CustomWriteLong(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  static class LongCustomSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _CustomWriteLong(TextWriter writer, long number, char[] buffer)
    {
      // Gotta special case this, we can't negate it
      if (number == long.MinValue)
      {
        writer.Write("-9223372036854775808");
        return;
      }

      var ptr = JSONConfiguration.SerilizerCharBufferSize - 1;

      ulong copy;
      if (number < 0)
      {
        writer.Write('-');
        copy = (ulong)(-number);
      }
      else
      {
        copy = (ulong)number;
      }

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