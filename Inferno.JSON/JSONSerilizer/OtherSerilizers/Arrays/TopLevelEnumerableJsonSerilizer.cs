using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno.JSONSerilizer.OtherSerilizers.Arrays
{
  class LazyTopLevelEnumerableJsonSerilizer<THost,T> : IFullSerilizer<THost> where THost : IEnumerable<T>
  {
    private IEntityJSONSerilizer<T> m_entityJsonSerilizer;
    private JSONConfiguration m_jsonConfiguration;

    public void Init(IEntityJSONSerilizer<T> subClassSerilizer, JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
      m_entityJsonSerilizer = subClassSerilizer;
    }

    public void Serilize(THost obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(THost obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('[');
      bool isFirst = true;
      foreach (var value in obj)
      {
        if (isFirst)
        {
          isFirst = false;
        }
        else
        {
          textWriter.Write(',');
        }

        if (value == null)
        {
          textWriter.Write(ConstsString.Null);
        }
        else
        {
          m_entityJsonSerilizer.Serilize(value, textWriter, writeBuffer);
        }
      }
      textWriter.Write(']');
    }
  }

  class TopLevelEnumerableJsonSerilizer<THost,T> : IFullSerilizer<THost> where THost : IEnumerable<T>
  {
    private readonly IEntityJSONSerilizer<T> m_entityJsonSerilizer;
    private readonly JSONConfiguration m_jsonConfiguration;

    public TopLevelEnumerableJsonSerilizer(IEntityJSONSerilizer<T> subClassSerilizer, JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
      m_entityJsonSerilizer = subClassSerilizer;
    }

    public void Serilize(THost obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(THost obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('[');
      bool isFirst = true;
      foreach (var value in obj)
      {
        if (isFirst)
        {
          isFirst = false;
        }
        else
        {
          textWriter.Write(',');
        }

        if (value == null)
        {
          textWriter.Write(ConstsString.Null);
        }
        else
        {
          m_entityJsonSerilizer.Serilize(value, textWriter, writeBuffer);
        }
      }
      textWriter.Write(']');
    }
  }
}