using System;
using Inferno.JSONSerilizer.OtherSerilizers.Struct;

namespace Inferno.JSONSerilizer.OtherSerilizers.Class
{
  interface IClassSerilizerFactory
  {
    Type GetSerilizerType();
    object GetFullSerilizer();
    IProperySerilizer<T> CreateSubClassSerilizer<T>(string propName, object valueExtractorDelegate);
    IStructProperySerilizer<T> CreateStructSubClassSerilizer<T>(string propName, object valueExtractorDelegate);
  }

  class ClassSerilizerFactory<TProp> : IClassSerilizerFactory
  {
    private IFullSerilizer<TProp> m_serilizer;

    public ClassSerilizerFactory(IFullSerilizer<TProp> serilizer)
    {
      m_serilizer = serilizer;
    }

    public void OverrideSerilizer(IFullSerilizer<TProp> topLevelSerilizer)
    {
      m_serilizer = topLevelSerilizer;
    }

    public Type GetSerilizerType()
    {
      return typeof(TProp);
    }

    public object GetFullSerilizer()
    {
      return m_serilizer;
    }

    public IProperySerilizer<T> CreateSubClassSerilizer<T>(string propName, object valueExtractorDelegate)
    {
      return new SubClassJsonSerilizer<T, TProp>(propName, m_serilizer, (Func<T, TProp>) valueExtractorDelegate);
    }

    public IStructProperySerilizer<T> CreateStructSubClassSerilizer<T>(string propName, object valueExtractorDelegate)
    {
      return new StructSubClassJsonSerilizer<T,TProp>(propName, m_serilizer, (RefValueExtractor<T, TProp>)valueExtractorDelegate);
    }
  }
}