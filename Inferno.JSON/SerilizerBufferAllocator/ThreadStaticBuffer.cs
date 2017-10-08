using System;

namespace Inferno.SerilizerBufferAllocator
{
  public class ThreadStaticBuffer : ISerilizerBufferAllocator
  {
    [ThreadStatic]
    private static char[] m_buffer;

    public static readonly ThreadStaticBuffer Instance = new ThreadStaticBuffer();
    
  
    private ThreadStaticBuffer()
    {
    }

    public char[] Allocate()
    {
      if (m_buffer == null)
      {
        return m_buffer = new char[JSONConfiguration.SerilizerCharBufferSize];
      }
      return m_buffer;
    }

    public void Free(char[] buffer)
    {

    }
  }
}