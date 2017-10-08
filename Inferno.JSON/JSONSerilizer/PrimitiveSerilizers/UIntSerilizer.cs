using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityUIntSerilizer : IFullSerilizer<uint>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityUIntSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(uint obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(uint obj, TextWriter textWriter, char[] writeBuffer)
    {
      UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, obj, writeBuffer);
    }
  }

  class EntityUIntNullSerilizer : IFullSerilizer<uint?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityUIntNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(uint? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(uint? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        UIntCustomSerilizer._CustomWriteUIntUnrolled(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructUIntSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, uint> m_valueExtractor;

    public StructUIntSerilizer(string properyName, RefValueExtractor<T, uint> valueExtractor)
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

  class StructNullableUIntSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, uint?> m_valueExtractor;

    public StructNullableUIntSerilizer(string properyName, RefValueExtractor<T, uint?> valueExtractor)
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

  class UIntSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, uint> m_valueExtractor;

    public UIntSerilizer(string properyName, Func<T, uint> valueExtractor)
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

  class NullableUIntSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, uint?> m_valueExtractor;

    public NullableUIntSerilizer(string properyName, Func<T, uint?> valueExtractor)
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

  static class UIntCustomSerilizer
  {

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _CustomWriteUIntUnrolled(TextWriter writer, uint number, char[] buffer)
    {
      int numLen;

      if (number < 1000)
      {
        if (number >= 100)
        {
          writer.Write(Utils.DigitTriplets, (int)(number * 3), 3);
        }
        else
        {
          if (number >= 10)
          {
            writer.Write(Utils.DigitTriplets, (int)(number * 3 + 1), 2);
          }
          else
          {
            writer.Write(Utils.DigitTriplets, (int)(number * 3 + 2), 1);
          }
        }
        return;
      }
      var d012 = number % 1000 * 3;

      uint d543;
      if (number < 1000000)
      {
        d543 = (number / 1000) * 3;
        if (number >= 100000)
        {
          numLen = 6;
          goto digit5;
        }
        else
        {
          if (number >= 10000)
          {
            numLen = 5;
            goto digit4;
          }
          else
          {
            numLen = 4;
            goto digit3;
          }
        }
      }
      d543 = (number / 1000) % 1000 * 3;

      uint d876;
      if (number < 1000000000)
      {
        d876 = (number / 1000000) * 3;
        if (number >= 100000000)
        {
          numLen = 9;
          goto digit8;
        }
        else
        {
          if (number >= 10000000)
          {
            numLen = 8;
            goto digit7;
          }
          else
          {
            numLen = 7;
            goto digit6;
          }
        }
      }
      d876 = (number / 1000000) % 1000 * 3;

      numLen = 10;

      // uint is between 0 & 4,294,967,295 (in practice we only get to int.MaxValue, but that's the same # of digits)
      // so 1 to 10 digits

      // [01,]000,000-[99,]000,000
      var d9 = number / 1000000000;
      buffer[0] = (char)('0' + d9);

      digit8:
      buffer[1] = Utils.DigitTriplets[d876];
      digit7:
      buffer[2] = Utils.DigitTriplets[d876 + 1];
      digit6:
      buffer[3] = Utils.DigitTriplets[d876 + 2];

      digit5:
      buffer[4] = Utils.DigitTriplets[d543];
      digit4:
      buffer[5] = Utils.DigitTriplets[d543 + 1];
      digit3:
      buffer[6] = Utils.DigitTriplets[d543 + 2];

      buffer[7] = Utils.DigitTriplets[d012];
      buffer[8] = Utils.DigitTriplets[d012 + 1];
      buffer[9] = Utils.DigitTriplets[d012 + 2];

      writer.Write(buffer, 10 - numLen, numLen);
    }
  }
}