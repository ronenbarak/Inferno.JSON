using System;
using System.IO;

namespace Inferno.JSONSerilizer.OtherSerilizers.Class
{
  class SubClassJsonSerilizer<T, TSubClass> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, TSubClass> m_valueExtractor;
    private readonly IEntityJSONSerilizer<TSubClass> m_subClassSerilizer;

    public SubClassJsonSerilizer(string properyName, IEntityJSONSerilizer<TSubClass> subClassSerilizer, Func<T, TSubClass> valueExtractor)
    {
      m_subClassSerilizer = subClassSerilizer;
      m_property = ("\"" + properyName + "\":").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
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