using System;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityDateTimeSerilizer : IFullSerilizer<DateTime>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDateTimeSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(DateTime obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(DateTime obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(ConstsString.StartDayTime);
      var ticks = (obj.Ticks - 621355968000000000L) / 10000L;
      LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
      textWriter.Write(ConstsString.EndOfTimeProp);
    }
  }

  class EntityDateTimeNullSerilizer : IFullSerilizer<DateTime?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityDateTimeNullSerilizer(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(DateTime? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(DateTime? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        textWriter.Write(ConstsString.StartDayTime);
        var ticks = (obj.Value.Ticks - 621355968000000000L) / 10000L;
        LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
        textWriter.Write(ConstsString.EndOfTimeProp);
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructMicrosoftDateTimeSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_endOfTimeProp = ")\\/\"".ToCharArray();
    private readonly RefValueExtractor<T, DateTime> m_valueExtractor;
    
    public StructMicrosoftDateTimeSerilizer(string properyName, RefValueExtractor<T, DateTime> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\\/Date(").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      var ticks = (value.Ticks - 621355968000000000L)/ 10000L;
      textWriter.Write(m_property);
      LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
      textWriter.Write(m_endOfTimeProp);
    }
  }

  class StructNullableMicrosoftDateTimeSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly char[] m_endOfTimeProp = ")\\/\"".ToCharArray();
    private readonly RefValueExtractor<T, DateTime?> m_valueExtractor;

    public StructNullableMicrosoftDateTimeSerilizer(string properyName, RefValueExtractor<T, DateTime?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\\/Date(").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value.HasValue)
      {
        var ticks = (value.Value.Ticks - 621355968000000000L) / 10000L;
        textWriter.Write(m_property);
        LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
        textWriter.Write(m_endOfTimeProp);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class MicrosoftDateTimeSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, DateTime> m_valueExtractor;

    public MicrosoftDateTimeSerilizer(string properyName, Func<T, DateTime> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\\/Date(").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      var ticks = (value.Ticks - 621355968000000000L) / 10000L;
      textWriter.Write(m_property);
      LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
      textWriter.Write(ConstsString.EndOfTimeProp);
    }
  }

  class NullableMicrosoftDateTimeSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, DateTime?> m_valueExtractor;

    public NullableMicrosoftDateTimeSerilizer(string properyName, Func<T, DateTime?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\\/Date(").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value.HasValue)
      {
        var ticks = (value.Value.Ticks - 621355968000000000L) / 10000L;
        textWriter.Write(m_property);
        LongCustomSerilizer._CustomWriteLong(textWriter, ticks, writeBuffer);
        textWriter.Write(ConstsString.EndOfTimeProp);
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }
}