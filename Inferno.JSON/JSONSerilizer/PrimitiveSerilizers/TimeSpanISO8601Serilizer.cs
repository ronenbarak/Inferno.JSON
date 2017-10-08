using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityTimeSpanSerilizer : IFullSerilizer<TimeSpan>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityTimeSpanSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(TimeSpan obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(TimeSpan obj, TextWriter textWriter, char[] writeBuffer)
    {
      TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, obj, writeBuffer);
    }
  }

  class EntityTimeSpanNullSerilizer : IFullSerilizer<TimeSpan?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityTimeSpanNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(TimeSpan? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(TimeSpan? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, obj.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructTimeSpanISO8601Serilizer<T> : IStructProperySerilizer<T>    
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, TimeSpan> m_valueExtractor;

    public StructTimeSpanISO8601Serilizer(string properyName, RefValueExtractor<T, TimeSpan> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, m_valueExtractor.Invoke(ref obj), writeBuffer);
    }
  }

  class StructNullableTimeSpanISO8601Serilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, TimeSpan?> m_valueExtractor;

    public StructNullableTimeSpanISO8601Serilizer(string properyName, RefValueExtractor<T, TimeSpan?> valueExtractor)
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
        TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class TimeSpanISO8601Serilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, TimeSpan> m_valueExtractor;

    public TimeSpanISO8601Serilizer(string properyName, Func<T, TimeSpan> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
    }
  }

  class NullableTimeSpanISO8601Serilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, TimeSpan?> m_valueExtractor;

    public NullableTimeSpanISO8601Serilizer(string properyName, Func<T, TimeSpan?> valueExtractor)
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
        TimeSpanISO8601CustomSerilizer._WriteTimeSpanISO8601(textWriter, value.Value, writeBuffer);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  static class TimeSpanISO8601CustomSerilizer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _WriteTimeSpanISO8601(TextWriter writer, TimeSpan ts, char[] buffer)
    {
      // can't negate this, have to handle it manually
      if (ts.Ticks == long.MinValue)
      {
        writer.Write("\"-P10675199DT2H48M5.4775808S\"");
        return;
      }

      writer.Write('"');

      if (ts.Ticks < 0)
      {
        writer.Write('-');
        ts = ts.Negate();
      }

      writer.Write('P');

      var days = ts.Days;
      var hours = ts.Hours;
      var minutes = ts.Minutes;
      var seconds = ts.Seconds;

      // days
      if (days > 0)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(writer, days, buffer);
        writer.Write('D');
      }

      // time separator
      writer.Write('T');

      // hours
      if (hours > 0)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(writer, hours, buffer);
        writer.Write('H');
      }

      // minutes
      if (minutes > 0)
      {
        IntCustomSerilizer._CustomWriteIntUnrolledSigned(writer, minutes, buffer);
        writer.Write('M');
      }

      // seconds
      IntCustomSerilizer._CustomWriteIntUnrolledSigned(writer, seconds, buffer);

      // fractional part
      {
        TwoDigits digits;
        int fracEnd;
        var endCount = 0;
        var remainingTicks = (ts - new TimeSpan(days, hours, minutes, seconds, 0)).Ticks;

        if (remainingTicks > 0)
        {
          buffer[0] = '.';

          var fracPart = remainingTicks % 100;
          remainingTicks /= 100;
          if (fracPart > 0)
          {
            digits = Utils.DigitPairs[fracPart];
            buffer[7] = digits.Second;
            buffer[6] = digits.First;
            fracEnd = 8;
          }
          else
          {
            fracEnd = 6;
          }

          fracPart = remainingTicks % 100;
          remainingTicks /= 100;
          if (fracPart > 0)
          {
            digits = Utils.DigitPairs[fracPart];
            buffer[5] = digits.Second;
            buffer[4] = digits.First;
          }
          else
          {
            if (fracEnd == 6)
            {
              fracEnd = 4;
            }
            else
            {
              buffer[5] = '0';
              buffer[4] = '0';
            }
          }

          fracPart = remainingTicks % 100;
          remainingTicks /= 100;
          if (fracPart > 0)
          {
            digits = Utils.DigitPairs[fracPart];
            buffer[3] = digits.Second;
            buffer[2] = digits.First;
          }
          else
          {
            if (fracEnd == 4)
            {
              fracEnd = 2;
            }
            else
            {
              buffer[3] = '0';
              buffer[2] = '0';
            }
          }

          fracPart = remainingTicks;
          buffer[1] = (char)('0' + fracPart);

          endCount = fracEnd;
        }

        writer.Write(buffer, 0, endCount);
      }

      writer.Write("S\"");
    }
  }
}