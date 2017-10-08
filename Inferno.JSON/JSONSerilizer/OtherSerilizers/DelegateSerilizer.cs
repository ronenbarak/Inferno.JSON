using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferno.JSONSerilizer.OtherSerilizers
{
  class DelegateSerilizer<T> : IProperySerilizer<T>
  {
    private readonly Action<T, TextWriter> m_serilizeFunc;
    private readonly char[] m_property;

    public DelegateSerilizer(string properyName, Action<T, TextWriter> serilizeFunc)
    {
      m_serilizeFunc = serilizeFunc;
      m_property = ("\"" + properyName + "\":").ToCharArray();
    }
    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      m_serilizeFunc.Invoke(obj,textWriter);
    }
  }
}
