using System.IO;
using System.Runtime.CompilerServices;

namespace Inferno.JSONSerilizer.OtherSerilizers.Class
{
  class LazyTopLevelClassJsonSerilizer<T> : IFullSerilizer<T>
  {
    private IProperySerilizer<T>[] m_properySerilizers;
    private JSONConfiguration m_jsonConfiguration;

    public void Init(IProperySerilizer<T>[] properties, JSONConfiguration config)
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
        m_properySerilizers[i].Serilize(obj, textWriter, writeBuffer);
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
        m_properySerilizers[i].Serilize(obj, textWriter, writeBuffer);
        if ((i + 1) != m_properySerilizers.Length)
        {
          textWriter.Write(',');
        }
      }
      textWriter.Write('}');
    }
  }
}
