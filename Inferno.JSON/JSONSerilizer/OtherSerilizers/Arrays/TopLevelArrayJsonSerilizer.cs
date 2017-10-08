using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno.JSONSerilizer.OtherSerilizers.Arrays
{
  class LazyTopLevelArrayJsonSerilizer<T> : IFullSerilizer<T[]>
  {
    private IEntityJSONSerilizer<T> m_entityJsonSerilizer;
    private JSONConfiguration m_jsonConfiguration;

    public void Init(IEntityJSONSerilizer<T> subClassSerilizer, JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
      m_entityJsonSerilizer = subClassSerilizer;
    }

    public void Serilize(T[] obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(T[] obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('[');
      for (int i = 0; i < obj.Length; i++)
      {
        if (i > 0)
        {
          textWriter.Write(',');
        }

        var value = obj[i];
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
  class TopLevelArrayJsonSerilizer<T> : IFullSerilizer<T[]>
  {
    private readonly IEntityJSONSerilizer<T> m_entityJsonSerilizer;
    private readonly JSONConfiguration m_jsonConfiguration;

    public TopLevelArrayJsonSerilizer(IEntityJSONSerilizer<T> subClassSerilizer,JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
      m_entityJsonSerilizer = subClassSerilizer;
    }

    public void Serilize(T[] obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(T[] obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('[');
      for (int i = 0; i < obj.Length; i++)
      {
        if (i > 0)
        {
          textWriter.Write(',');
        }

        var value = obj[i];
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
