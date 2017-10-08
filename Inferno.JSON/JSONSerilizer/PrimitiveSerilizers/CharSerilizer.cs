using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityCharSerilier : IFullSerilizer<char>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityCharSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(char obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(char obj, TextWriter textWriter, char[] writeBuffer)
    {
      CharSerilizer.WriteChar(textWriter, obj);
    }
  }

  class EntityCharNullSerilier : IFullSerilizer<char?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityCharNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(char? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(char? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        CharSerilizer.WriteChar(textWriter, obj.Value);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructCharSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_propertyWithQutes;
    private readonly RefValueExtractor<T, char> m_valueExtractor;

    public StructCharSerilizer(string properyName, RefValueExtractor<T, char> valueExtractor)
    {
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_propertyWithQutes);
      CharSerilizer.WriteChar(textWriter, m_valueExtractor.Invoke(ref obj));
      textWriter.Write('\"');
    }
  }

  class StructNullableCharSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_propertyWithQutes;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, char?> m_valueExtractor;

    public StructNullableCharSerilizer(string properyName, RefValueExtractor<T, char?> valueExtractor)
    {
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value == null)
      {
        textWriter.Write(m_propertyNull);
      }
      else
      {
        textWriter.Write(m_propertyWithQutes);
        CharSerilizer.WriteChar(textWriter, value.Value);
        textWriter.Write('\"');
      }
    }
  }

  class CharSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_propertyWithQutes;
    private readonly Func<T, char> m_valueExtractor;

    public CharSerilizer(string properyName, Func<T, char> valueExtractor)
    {
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_propertyWithQutes);
      CharSerilizer.WriteChar(textWriter, m_valueExtractor.Invoke(obj));
      textWriter.Write('\"');
    }
  }

  class NullableCharSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_propertyWithQutes;
    private readonly char[] m_propertyNull;
    private readonly Func<T, char?> m_valueExtractor;

    public NullableCharSerilizer(string properyName, Func<T, char?> valueExtractor)
    {
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value == null)
      {
        textWriter.Write(m_propertyNull);
      }
      else
      {
        textWriter.Write(m_propertyWithQutes);
        CharSerilizer.WriteChar(textWriter, value.Value);
        textWriter.Write('\"');
      }
    }
  }

  static class CharSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteChar(TextWriter writer, char c)
    {
      if (c == '\\')
      {
        writer.Write(@"\\");
        return;
      }

      if (c == '"')
      {
        writer.Write("\\\"");
        return;
      }

      if (c == '\u2028')
      {
        writer.Write(@"\u2028");
        return;
      }

      if (c == '\u2029')
      {
        writer.Write(@"\u2029");
        return;
      }

      // This is converted into an IL switch, so don't fret about lookup times
      switch (c)
      {
        case '\u0000': writer.Write(@"\u0000"); return;
        case '\u0001': writer.Write(@"\u0001"); return;
        case '\u0002': writer.Write(@"\u0002"); return;
        case '\u0003': writer.Write(@"\u0003"); return;
        case '\u0004': writer.Write(@"\u0004"); return;
        case '\u0005': writer.Write(@"\u0005"); return;
        case '\u0006': writer.Write(@"\u0006"); return;
        case '\u0007': writer.Write(@"\u0007"); return;
        case '\u0008': writer.Write(@"\b"); return;
        case '\u0009': writer.Write(@"\t"); return;
        case '\u000A': writer.Write(@"\n"); return;
        case '\u000B': writer.Write(@"\u000B"); return;
        case '\u000C': writer.Write(@"\f"); return;
        case '\u000D': writer.Write(@"\r"); return;
        case '\u000E': writer.Write(@"\u000E"); return;
        case '\u000F': writer.Write(@"\u000F"); return;
        case '\u0010': writer.Write(@"\u0010"); return;
        case '\u0011': writer.Write(@"\u0011"); return;
        case '\u0012': writer.Write(@"\u0012"); return;
        case '\u0013': writer.Write(@"\u0013"); return;
        case '\u0014': writer.Write(@"\u0014"); return;
        case '\u0015': writer.Write(@"\u0015"); return;
        case '\u0016': writer.Write(@"\u0016"); return;
        case '\u0017': writer.Write(@"\u0017"); return;
        case '\u0018': writer.Write(@"\u0018"); return;
        case '\u0019': writer.Write(@"\u0019"); return;
        case '\u001A': writer.Write(@"\u001A"); return;
        case '\u001B': writer.Write(@"\u001B"); return;
        case '\u001C': writer.Write(@"\u001C"); return;
        case '\u001D': writer.Write(@"\u001D"); return;
        case '\u001E': writer.Write(@"\u001E"); return;
        case '\u001F': writer.Write(@"\u001F"); return;
        default: writer.Write(c); return;
      }
    }
  }
}