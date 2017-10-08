using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer
{
  class TypeSerilizerRepository
  {
    private readonly ConcurrentDictionary<Type, IClassSerilizerFactory> m_serilziers = new ConcurrentDictionary<Type, IClassSerilizerFactory>();

    public TypeSerilizerRepository()
    {
    }

    public void AddSerilizer(IClassSerilizerFactory serilizerFactory)
    {
      m_serilziers.TryAdd(serilizerFactory.GetSerilizerType(), serilizerFactory);
    }

    public bool TryGetSerilizerFactory(Type type, out IClassSerilizerFactory topLevelClassJsonSerilizer)
    {
      return m_serilziers.TryGetValue(type, out topLevelClassJsonSerilizer);
    }

    public bool TryGetSerilizer<T>(out IJSONSerilizer<T> serilizer)
    {
      IClassSerilizerFactory instance;
      if (m_serilziers.TryGetValue(typeof(T), out instance))
      {
        serilizer = (IJSONSerilizer<T>) (instance.GetFullSerilizer());
        return true;
      }

      serilizer = null;
      return false;
    }
  }
}
