using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityStringSerilizer : IFullSerilizer<string>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityStringSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(string obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(string value, TextWriter textWriter, char[] writeBuffer)
    {
      if (value == null)
      {
        textWriter.Write(ConstsString.Null);
      }
      else
      {
        textWriter.Write('\"');
        StringCustomSerilizer._WriteEncodedStringWithNullsInlineUnsafe(textWriter, value);
        textWriter.Write('\"');
      }
    }
  }

  class StructStringSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_propertyNull;
    private readonly char[] m_propertyWithQutes;
    private readonly RefValueExtractor<T, string> m_valueExtractor;

    public StructStringSerilizer(string properyName, RefValueExtractor<T, string> valueExtractor)
    {
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
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
        StringCustomSerilizer._WriteEncodedStringWithNullsInlineUnsafe(textWriter, value);
        textWriter.Write('\"');
      }
    }
  }

  class StringSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_propertyNull;
    private readonly char[] m_propertyWithQutes;
    private readonly Func<T, string> m_valueExtractor;

    public StringSerilizer(string properyName, Func<T, string> valueExtractor)
    {
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_propertyWithQutes = ("\"" + properyName + "\":\"").ToCharArray();
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
        StringCustomSerilizer._WriteEncodedStringWithNullsInlineUnsafe(textWriter, value);
        textWriter.Write('\"');
      }
    }
  }

  static class StringCustomSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void _WriteEncodedStringWithNullsInlineUnsafe(TextWriter writer, string strRef)
    {
      fixed (char* strFixed = strRef)
      {
        char* str = strFixed;
        char c;
        var len = strRef.Length;

        while (len > 0)
        {
          c = *str;
          str++;
          len--;

          if (c == '\\')
          {
            writer.Write(@"\\");
            continue;
          }

          if (c == '"')
          {
            writer.Write("\\\"");
            continue;
          }

          if (c == '\u2028')
          {
            writer.Write(@"\u2028");
            continue;
          }

          if (c == '\u2029')
          {
            writer.Write(@"\u2029");
            continue;
          }

          // This is converted into an IL switch, so don't fret about lookup times
          switch (c)
          {
            case '\u0000':
              writer.Write(@"\u0000");
              continue;
            case '\u0001':
              writer.Write(@"\u0001");
              continue;
            case '\u0002':
              writer.Write(@"\u0002");
              continue;
            case '\u0003':
              writer.Write(@"\u0003");
              continue;
            case '\u0004':
              writer.Write(@"\u0004");
              continue;
            case '\u0005':
              writer.Write(@"\u0005");
              continue;
            case '\u0006':
              writer.Write(@"\u0006");
              continue;
            case '\u0007':
              writer.Write(@"\u0007");
              continue;
            case '\u0008':
              writer.Write(@"\b");
              continue;
            case '\u0009':
              writer.Write(@"\t");
              continue;
            case '\u000A':
              writer.Write(@"\n");
              continue;
            case '\u000B':
              writer.Write(@"\u000B");
              continue;
            case '\u000C':
              writer.Write(@"\f");
              continue;
            case '\u000D':
              writer.Write(@"\r");
              continue;
            case '\u000E':
              writer.Write(@"\u000E");
              continue;
            case '\u000F':
              writer.Write(@"\u000F");
              continue;
            case '\u0010':
              writer.Write(@"\u0010");
              continue;
            case '\u0011':
              writer.Write(@"\u0011");
              continue;
            case '\u0012':
              writer.Write(@"\u0012");
              continue;
            case '\u0013':
              writer.Write(@"\u0013");
              continue;
            case '\u0014':
              writer.Write(@"\u0014");
              continue;
            case '\u0015':
              writer.Write(@"\u0015");
              continue;
            case '\u0016':
              writer.Write(@"\u0016");
              continue;
            case '\u0017':
              writer.Write(@"\u0017");
              continue;
            case '\u0018':
              writer.Write(@"\u0018");
              continue;
            case '\u0019':
              writer.Write(@"\u0019");
              continue;
            case '\u001A':
              writer.Write(@"\u001A");
              continue;
            case '\u001B':
              writer.Write(@"\u001B");
              continue;
            case '\u001C':
              writer.Write(@"\u001C");
              continue;
            case '\u001D':
              writer.Write(@"\u001D");
              continue;
            case '\u001E':
              writer.Write(@"\u001E");
              continue;
            case '\u001F':
              writer.Write(@"\u001F");
              continue;
            default:
              writer.Write(c);
              continue;
          }
        }
      }
    }
  }
}