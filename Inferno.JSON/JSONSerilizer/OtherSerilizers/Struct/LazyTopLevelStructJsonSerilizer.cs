using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.OtherSerilizers.Struct
{
  class LazyTopLevelStructJsonSerilizer<T> : IFullSerilizer<T>
  {
    private IStructProperySerilizer<T>[] m_properySerilizers;
    private JSONConfiguration m_jsonConfiguration;

    public void Init(IStructProperySerilizer<T>[] properties, JSONConfiguration config)
    {
      m_jsonConfiguration = config;
      m_properySerilizers = properties;
    }

    public void Serilize(T obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('{');
      for (int i = 0; i < m_properySerilizers.Length; i++)
      {
        m_properySerilizers[i].Serilize(ref obj, textWriter, writeBuffer);
        if ((i + 1) != m_properySerilizers.Length)
        {
          textWriter.Write(',');
        }
      }
      textWriter.Write('}');
    }
  }

  class TopLevelStructJsonSerilizer<T> : IFullSerilizer<T>
  {
    private readonly IStructProperySerilizer<T>[] m_properySerilizers;
    private readonly JSONConfiguration m_jsonConfiguration;

    public TopLevelStructJsonSerilizer(IStructProperySerilizer<T>[] properties, JSONConfiguration config)
    {
      m_jsonConfiguration = config;
      m_properySerilizers = properties;
    }

    public void Serilize(T obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('{');
      for (int i = 0; i < m_properySerilizers.Length; i++)
      {
        m_properySerilizers[i].Serilize(ref obj, textWriter, writeBuffer);
        if ((i + 1) != m_properySerilizers.Length)
        {
          textWriter.Write(',');
        }
      }
      textWriter.Write('}');
    }
  }
}
