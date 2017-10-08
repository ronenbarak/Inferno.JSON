using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.OtherSerilizers.Struct
{
  class StructSubClassJsonSerilizer<T, TSubClass> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, TSubClass> m_valueExtractor;
    private readonly IEntityJSONSerilizer<TSubClass> m_subClassSerilizer;

    public StructSubClassJsonSerilizer(string properyName, IEntityJSONSerilizer<TSubClass> subClassSerilizer, RefValueExtractor<T, TSubClass> valueExtractor)
    {
      m_subClassSerilizer = subClassSerilizer;
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value == null)
      {
        textWriter.Write(m_propertyNull);
      }
      else
      {
        textWriter.Write(m_property);
        m_subClassSerilizer.Serilize(value, textWriter, writeBuffer);
      }
    }
  }
}
