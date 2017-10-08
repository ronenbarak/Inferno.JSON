using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityIntSerilier : IFullSerilizer<int>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityIntSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(int obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(int obj, TextWriter textWriter, char[] writeBuffer)
    {
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, obj, writeBuffer);
    }
  }

  class EntityIntNullSerilier : IFullSerilizer<int?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityIntNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(int? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(int? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class EntityIntSerilizer : IFullSerilizer<int>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityIntSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(int obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(int obj, TextWriter textWriter, char[] writeBuffer)
    {
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, obj, writeBuffer);
    }
  }

  class EntityIntNullSerilizer : IFullSerilizer<int?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityIntNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(int? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(int? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructIntSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, int> m_valueExtractor;

    public StructIntSerilizer(string properyName, RefValueExtractor<T,int> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableIntSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, int?> m_valueExtractor;

    public StructNullableIntSerilizer(string properyName, RefValueExtractor<T, int?> valueExtractor)
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
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class IntSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, int> m_valueExtractor;

    public IntSerilizer(string properyName, Func<T, int> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableIntSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, int?> m_valueExtractor;

    public NullableIntSerilizer(string properyName, Func<T, int?> valueExtractor)
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
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  static class IntCustomSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _CustomWriteIntUnrolledSigned(TextWriter writer, int num, char[] buffer)
    {
      // Why signed integers?
      // Earlier versions of this code used unsigned integers, 
      //   but it turns out that's not ideal and here's why.
      // 
      // The signed version of the relevant code gets JIT'd down to:
      // instr       operands                    latency/throughput (approx. worst case; Haswell)
      // ========================================================================================
      // mov         ecx,###                      2 /  0.5 
      // cdq                                      1 /  -
      // idiv        eax,ecx                     29 / 11
      // mov         ecx,###                      2 /  0.5
      // cdq                                      1 /  -
      // idiv        eax,ecx                     29 / 11
      // movsx       edx,dl                       - /  0.5
      //
      // The unsigned version gets JIT'd down to:
      // instr       operands                    latency/throughput (approx. worst case; Haswell)
      // ========================================================================================
      // mov         ecx,###                       2 /  0.5
      // xor         edx,edx                       1 /  0.25
      // div         eax,ecx                      29 / 11
      // mov         ecx,###                       2 /  0.5
      // xor         edx,edx                       1 /  0.25
      // div         eax,ecx                      29 / 11
      // and         edx,###                       1 /  0.25
      //
      // In theory div (usigned division) is faster than idiv, and it probably is *but* cdq + cdq + movsx is
      //   faster than xor + xor + and; in practice it's fast *enough* to make up the difference.
      // have to special case this, we can't negate it
      if (num == int.MinValue)
      {
        writer.Write("-2147483648");
        return;
      }

      int numLen;
      int number;

      if (num < 0)
      {
        writer.Write('-');
        number = -num;
      }
      else
      {
        number = num;
      }

      if (number < 1000)
      {
        if (number >= 100)
        {
          writer.Write(Utils.DigitTriplets, number * 3, 3);
        }
        else
        {
          if (number >= 10)
          {
            writer.Write(Utils.DigitTriplets, number * 3 + 1, 2);
          }
          else
          {
            writer.Write(Utils.DigitTriplets, number * 3 + 2, 1);
          }
        }
        return;
      }
      var d012 = number % 1000 * 3;

      int d543;
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

      int d876;
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
