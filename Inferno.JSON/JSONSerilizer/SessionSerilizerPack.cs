using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer
{
  class SessionSerilizerPack
  {
    private readonly TypeSerilizerRepository m_typeSerilizerRepository;
    private readonly Dictionary<Type,IClassSerilizerFactory> m_classSerilizerFactories = new Dictionary<Type, IClassSerilizerFactory>();

    public SessionSerilizerPack(TypeSerilizerRepository repository)
    {
      m_typeSerilizerRepository = repository;
    }

    public void Flush()
    {
      foreach (var classSerilizerFactory in m_classSerilizerFactories.Values)
      {
        m_typeSerilizerRepository.AddSerilizer(classSerilizerFactory);
      }
    }

    public bool TryGetSerilizer<T>(out IFullSerilizer<T> fullSerilizer)
    {
      IClassSerilizerFactory factory;
      if (m_classSerilizerFactories.TryGetValue(typeof(T), out factory))
      {
        fullSerilizer = (IFullSerilizer<T>) factory.GetFullSerilizer();
        return true;
      }

      if (m_typeSerilizerRepository.TryGetSerilizerFactory(typeof(T), out factory))
      {
        fullSerilizer = (IFullSerilizer<T>)factory.GetFullSerilizer();
        return true;
      }

      fullSerilizer = null;
      return false;
    }

    public bool TryGetSerilizerFactory(Type type, out IClassSerilizerFactory serilizerFactory)
    {
      if (m_classSerilizerFactories.TryGetValue(type, out serilizerFactory))
      {
        return true;
      }

      if (m_typeSerilizerRepository.TryGetSerilizerFactory(type, out serilizerFactory))
      {
        return true;
      }

      serilizerFactory = null;
      return false;
    }

    public IClassSerilizerFactory GetOrCreateSerilizerFactory(Type subclassType)
    {
      IClassSerilizerFactory classDef;
      if (!m_classSerilizerFactories.TryGetValue(subclassType, out classDef))
      {

        if (!m_typeSerilizerRepository.TryGetSerilizerFactory(subclassType, out classDef))
        {
          var enumTypeDef = typeof(ClassSerilizerFactory<>);
          var genericType = enumTypeDef.MakeGenericType(subclassType);
          classDef = (IClassSerilizerFactory)Activator.CreateInstance(genericType);
          m_classSerilizerFactories.Add(subclassType, classDef);
        }
      }

      return classDef;
    }

    public void SetSerilizerFactoy(IClassSerilizerFactory serilizerFactory)
    {
      m_classSerilizerFactories.Add(serilizerFactory.GetSerilizerType(), serilizerFactory);
    }
  }
}
