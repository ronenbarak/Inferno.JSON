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
  class EntityBoolSerilier : IFullSerilizer<bool>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityBoolSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(bool obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(bool obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj)
      {
        textWriter.Write(ConstsString.True);
      }
      else
      {
        textWriter.Write(ConstsString.False);
      }
    }
  }

  class EntityBoolNullSerilier : IFullSerilizer<bool?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityBoolNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(bool? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(bool? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        if (obj.Value)
        {
          textWriter.Write(ConstsString.True);
        }
        else
        {
          textWriter.Write(ConstsString.False);
        }
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructBoolSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_propertyTrue;
    private readonly char[] m_propertyFalse;
    private readonly RefValueExtractor<T, bool> m_valueExtractor;

    public StructBoolSerilizer(string properyName, RefValueExtractor<T, bool> valueExtractor)
    {
      m_propertyTrue = ("\"" + properyName + "\":" + "true").ToCharArray();
      m_propertyTrue = ("\"" + properyName + "\":" + "false").ToCharArray();
      m_valueExtractor = valueExtractor;
    }
    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (m_valueExtractor.Invoke(ref obj))
      {
        textWriter.Write(m_propertyTrue);
      }
      else
      {
        textWriter.Write(m_propertyFalse);
      }
    }
  }

  class StructNullBoolSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_propertyTrue;
    private readonly char[] m_propertyNull;
    private readonly char[] m_propertyFalse;
    private readonly RefValueExtractor<T, bool?> m_valueExtractor;

    public StructNullBoolSerilizer(string properyName, RefValueExtractor<T, bool?> valueExtractor)
    {
      m_propertyNull = ("\"" + properyName + "\":" + "null").ToCharArray();
      m_propertyTrue = ("\"" + properyName + "\":" + "true").ToCharArray();
      m_propertyTrue = ("\"" + properyName + "\":" + "false").ToCharArray();
      m_valueExtractor = valueExtractor;
    }
    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value.HasValue)
      {
        if (value.Value)
        {
          textWriter.Write(m_propertyTrue);
        }
        else
        {
          textWriter.Write(m_propertyFalse);
        }
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
      
    }
  }

  class BoolSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_propertyTrue;
    private readonly char[] m_propertyFalse;
    
    private readonly Func<T, bool> m_valueExtractor;

    public BoolSerilizer(string properyName, Func<T, bool> valueExtractor)
    {
      m_propertyTrue = ("\"" + properyName + "\":" + "true").ToCharArray();
      m_propertyFalse = ("\"" + properyName + "\":" + "false").ToCharArray();
      m_valueExtractor = valueExtractor;
    }
    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (m_valueExtractor.Invoke(obj))
      {
        textWriter.Write(m_propertyTrue);
      }
      else
      {
        textWriter.Write(m_propertyFalse);
      }
    }
  }

  class NullBoolSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_propertyTrue;
    private readonly char[] m_propertyNull;
    private readonly char[] m_propertyFalse;
    private readonly Func<T, bool?> m_valueExtractor;

    public NullBoolSerilizer(string properyName, Func<T, bool?> valueExtractor)
    {
      m_propertyNull = ("\"" + properyName + "\":" + "null").ToCharArray();
      m_propertyTrue = ("\"" + properyName + "\":" + "true").ToCharArray();
      m_propertyFalse = ("\"" + properyName + "\":" + "false").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value.HasValue)
      {
        if (value.Value)
        {
          textWriter.Write(m_propertyTrue);
        }
        else
        {
          textWriter.Write(m_propertyFalse);
        }
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }

    }
  }
}
