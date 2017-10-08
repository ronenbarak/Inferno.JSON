using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Inferno.JSONSerilizer.OtherSerilizers
{
  /*class LazyTopLevelClassJsonSerilizer<T> : IFullSerilizer<T>
  {
    private IProperySerilizer<T>[] m_properySerilizers;
    private JSONConfiguration m_jsonConfiguration;

    public void Init(IProperySerilizer<T>[] properties, JSONConfiguration config)
    {
      m_jsonConfiguration = config;
      m_properySerilizers = properties;
    }

    public void Serialize(T obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serialize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('{');
      for (int i = 0; i < m_properySerilizers.Length; i++)
      {
        m_properySerilizers[i].Serialize(obj, textWriter, writeBuffer);
        if ((i + 1) != m_properySerilizers.Length)
        {
          textWriter.Write(',');
        }
      }
      textWriter.Write('}');
    }
  }

  class TopLevelClassJsonSerilizer<T> : IFullSerilizer<T>
  {
    private readonly IProperySerilizer<T>[] m_properySerilizers;
    private readonly JSONConfiguration m_jsonConfiguration;

    public TopLevelClassJsonSerilizer(IProperySerilizer<T>[] properties, JSONConfiguration config)
    {
      m_jsonConfiguration = config;
      m_properySerilizers = properties;
    }

    public void Serialize(T obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serialize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('{');
      for (int i = 0; i < m_properySerilizers.Length; i++)
      {
        m_properySerilizers[i].Serialize(obj, textWriter, writeBuffer);
        if ((i + 1) != m_properySerilizers.Length)
        {
          textWriter.Write(',');
        }
      }
      textWriter.Write('}');
    }
  }*/
}
