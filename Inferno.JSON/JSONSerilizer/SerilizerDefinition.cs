using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Inferno.Common;
using Inferno.JSONSerilizer.OtherSerilizers;
using Inferno.JSONSerilizer.OtherSerilizers.Class;
using Inferno.JSONSerilizer.OtherSerilizers.Struct;

namespace Inferno.JSONSerilizer
{
  class SerilizerDefinition<T> : ISerilizerDefinition<T>
  {
    private readonly List<PropertyInfo> m_ignores = new List<PropertyInfo>();
    private readonly List<Tuple<PropertyInfo,IProperySerilizer<T>>> m_include = new List<Tuple<PropertyInfo, IProperySerilizer<T>>>();
    private readonly List<Tuple<PropertyInfo, IStructProperySerilizer<T>>> m_includeStruct = new List<Tuple<PropertyInfo, IStructProperySerilizer<T>>>();

    private readonly JSONConfiguration m_jsonConfiguration;
    private bool m_camelCase = false;
    private bool m_noInterfaces = false;
    private bool m_explicit;
    private readonly GroupJson m_json;
    private SessionSerilizerPack m_sessionSerilizerPack;
    private bool m_isStruct = false;
    public SerilizerDefinition(JSONConfiguration jsonConfiguration,SessionSerilizerPack session,GroupJson json)
    {
      if (typeof(T).IsValueType)
      {
        m_isStruct = true;
      }

      m_sessionSerilizerPack = session;
      m_json = json;
      m_jsonConfiguration = jsonConfiguration;
    }

    public void AutoDefine()
    {
      if (m_jsonConfiguration.Explicit)
      {
        this.Explicit();
      }
      else
      {
        this.Implicit();
      }

      if (JSONConfiguration.Global.NoInterfaceProperties)
      {
        this.NoInterfaceProperties();
      }

      if (JSONConfiguration.Global.CamelCaseSerilizer)
      {
        this.CamelCase();
      }
    }

    public IFullSerilizer<T> CreateSerilizerNoCache()
    {
      if (m_isStruct)
      {
        return new TopLevelStructJsonSerilizer<T>(CreateStructSerilizerInternal().ToArray(), m_jsonConfiguration);
      }
      else
      {
        return new TopLevelClassJsonSerilizer<T>(CreateSerilizerInternal().ToArray(), m_jsonConfiguration);
      }
    }

    public IClassSerilizerFactory CreateSerilizerFactory()
    {
      IClassSerilizerFactory factory;
      if (m_sessionSerilizerPack.TryGetSerilizerFactory(typeof(T),out factory))
      {
        return factory;
      }

      if (m_isStruct)
      {
        var lazyInitSerilizer = new LazyTopLevelStructJsonSerilizer<T>();
        ClassSerilizerFactory<T> serilizerFactory = new ClassSerilizerFactory<T>(lazyInitSerilizer);
        m_sessionSerilizerPack.SetSerilizerFactoy(serilizerFactory);
        var properties = CreateStructSerilizerInternal().ToArray();
        lazyInitSerilizer.Init(properties, m_jsonConfiguration);
        var topLevelSerilizer = new TopLevelStructJsonSerilizer<T>(properties, m_jsonConfiguration);
        serilizerFactory.OverrideSerilizer(topLevelSerilizer);
        return serilizerFactory;
      }
      else
      {
        var lazyInitSerilizer = new LazyTopLevelClassJsonSerilizer<T>();
        ClassSerilizerFactory<T> serilizerFactory = new ClassSerilizerFactory<T>(lazyInitSerilizer);
        m_sessionSerilizerPack.SetSerilizerFactoy(serilizerFactory);
        var properties = CreateSerilizerInternal().ToArray();
        lazyInitSerilizer.Init(properties, m_jsonConfiguration);
        var topLevelSerilizer = new TopLevelClassJsonSerilizer<T>(properties, m_jsonConfiguration);
        serilizerFactory.OverrideSerilizer(topLevelSerilizer);
        return serilizerFactory;
      }
    }

    public IFullSerilizer<T> CreateSerilizer()
    {
      IFullSerilizer<T> fullSerilizer;
      if (m_sessionSerilizerPack.TryGetSerilizer<T>(out fullSerilizer))
      {
        return fullSerilizer;
      }

      if (m_isStruct)
      {
        var lazyInitSerilizer = new LazyTopLevelStructJsonSerilizer<T>();
        ClassSerilizerFactory<T> serilizerFactory = new ClassSerilizerFactory<T>(lazyInitSerilizer);
        m_sessionSerilizerPack.SetSerilizerFactoy(serilizerFactory);
        var properties = CreateStructSerilizerInternal().ToArray();
        lazyInitSerilizer.Init(properties, m_jsonConfiguration);
        var topLevelSerilizer = new TopLevelStructJsonSerilizer<T>(properties, m_jsonConfiguration);
        serilizerFactory.OverrideSerilizer(topLevelSerilizer);
        return topLevelSerilizer;
      }
      else
      {
        var lazyInitSerilizer = new LazyTopLevelClassJsonSerilizer<T>();
        ClassSerilizerFactory<T> serilizerFactory = new ClassSerilizerFactory<T>(lazyInitSerilizer);
        m_sessionSerilizerPack.SetSerilizerFactoy(serilizerFactory);
        var properties = CreateSerilizerInternal().ToArray();
        lazyInitSerilizer.Init(properties, m_jsonConfiguration);
        var topLevelSerilizer = new TopLevelClassJsonSerilizer<T>(properties, m_jsonConfiguration);
        serilizerFactory.OverrideSerilizer(topLevelSerilizer);
        return topLevelSerilizer;
      }
    }

    private List<IProperySerilizer<T>> CreateSerilizerInternal()
    {
      HandleImplicit();

      List<IProperySerilizer<T>> finalProperties = m_include.Select(p => p.Item2).ToList();
      return finalProperties;
    }

    private List<IStructProperySerilizer<T>> CreateStructSerilizerInternal()
    {
      HandleImplicit();

      List<IStructProperySerilizer<T>> finalProperties = m_includeStruct.Select(p => p.Item2).ToList();
      return finalProperties;
    }

    private void HandleImplicit()
    {
      if (!m_explicit)
      {
        BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        if (m_noInterfaces)
        {
          bf = bf | BindingFlags.DeclaredOnly;
        }

        HashSet<PropertyInfo> usedProperties = new HashSet<PropertyInfo>();
        foreach (var propertyInfo in m_ignores)
        {
          usedProperties.Add(propertyInfo);
        }

        foreach (var propertyInfo in m_include)
        {
          usedProperties.Add(propertyInfo.Item1);
        }

        var properties = typeof(T).GetProperties(bf);
        foreach (var propertyInfo in properties)
        {
          if (propertyInfo.GetMethod != null)
          {
            if (!usedProperties.Contains(propertyInfo))
            {
              Include(propertyInfo);
            }
          }
        }
      }
    }

    public ISerilizerDefinition2<T> CamelCase()
    {
      m_camelCase = true;return this;
    }

    public ISerilizerDefinition2<T> NoInterfaceProperties()
    {
      m_noInterfaces = true;return this;
    }

    public ISerilizerDefinition3<T> Ignore<TProp>(Expression<Func<T, TProp>> prop)
    {
      var propertyInfo = PropertyExpressionExtractor.GetProperty(prop);
      m_ignores.Add(propertyInfo);
      return this;
    }

    private void Include(PropertyInfo propertyInfo)
    {
      if (m_isStruct)
      {
        var property = propertyInfo;
        var funcTypeDefinition = typeof(RefValueExtractor<,>);
        var funcType = funcTypeDefinition.MakeGenericType(typeof(T), propertyInfo.PropertyType);

        var valueExtractor = Delegate.CreateDelegate(funcType, property.GetMethod);
        IStructProperySerilizer<T> serilizer;
        if (PropertyFactory.TryCreateStructPropertySerilizer(m_jsonConfiguration, m_json, propertyInfo.PropertyType, property.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
        {
          m_includeStruct.Add(Tuple.Create(property, serilizer));
        }
      }
      else
      {
        var property = propertyInfo;
        var funcTypeDefinition = typeof(Func<,>);
        var funcType = funcTypeDefinition.MakeGenericType(typeof(T), propertyInfo.PropertyType);

        var valueExtractor = Delegate.CreateDelegate(funcType, property.GetMethod);
        IProperySerilizer<T> serilizer;
        if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, propertyInfo.PropertyType, property.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
        {
          m_include.Add(Tuple.Create(property, serilizer));
        }
      }
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop) where TProp : struct
    {
      if (m_isStruct)
      {
        var property = PropertyExpressionExtractor.GetProperty(prop);
        var valueExtractor = Delegate.CreateDelegate(typeof(RefValueExtractor<T, TProp>), property.GetMethod);
        IStructProperySerilizer<T> serilizer;
        if (PropertyFactory.TryCreateStructPropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
        {
          m_includeStruct.Add(Tuple.Create(property, serilizer));
        }
      }
      else
      {
        var property = PropertyExpressionExtractor.GetProperty(prop);
        var valueExtractor = Delegate.CreateDelegate(typeof(Func<T, TProp>), property.GetMethod);
        IProperySerilizer<T> serilizer;
        if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
        {
          m_include.Add(Tuple.Create(property, serilizer));
        }
      }
      
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp, TPropConv>(Expression<Func<T, TProp>> prop, Func<T, TPropConv> convert) where TPropConv : struct
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = convert;
      IProperySerilizer<T> serilizer;
      if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
      {
        m_include.Add(Tuple.Create(property, serilizer));
      }
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, Func<T, string> convert)
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = convert;
      IProperySerilizer<T> serilizer;
      if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
      {
        m_include.Add(Tuple.Create(property, serilizer));
      }
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, Action<T, TextWriter> serilizer)
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      IProperySerilizer<T> delegateSerilizer = new DelegateSerilizer<T>(property.Name, serilizer);
      m_include.Add(Tuple.Create(property,delegateSerilizer));
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, string propName, Action<T, TextWriter> serilizer)
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      IProperySerilizer<T> delegateSerilizer = new DelegateSerilizer<T>(propName, serilizer);
      m_include.Add(Tuple.Create(property, delegateSerilizer));
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, string propName) where TProp : struct
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = (Func<T, TProp>)Delegate.CreateDelegate(typeof(Func<T, TProp>), property.GetMethod);
      IProperySerilizer<T> serilizer;
      if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property,m_sessionSerilizerPack, out serilizer))
      {
        m_include.Add(Tuple.Create(property, serilizer));
      }
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp, TPropConv>(Expression<Func<T, TProp>> prop, string propName, Func<T, TPropConv> convert) where TPropConv : struct
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = convert;
      IProperySerilizer<T> serilizer;
      if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property, m_sessionSerilizerPack, out serilizer))
      {
        m_include.Add(Tuple.Create(property, serilizer));
      }
      return this;
    }

    public ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, string propName, Func<T, string> convert)
    {
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = convert;
      IProperySerilizer<T> serilizer;
      if (PropertyFactory.TryCreatePropertySerilizer(m_jsonConfiguration, m_json, typeof(TProp), prop.Name, valueExtractor, property,m_sessionSerilizerPack, out serilizer))
      {
        m_include.Add(Tuple.Create(property, serilizer));
      }
      return this;
    }

    public ISerilizerDefinition3<T> SubClass<TSubClass>(Expression<Func<T, TSubClass>> prop, Action<ISerilizerDefinition<TSubClass>> action)
    {
      SerilizerDefinition<TSubClass> serilizerDefinition = new SerilizerDefinition<TSubClass>(m_jsonConfiguration,m_sessionSerilizerPack,m_json);
      action.Invoke(serilizerDefinition);
      var serilizer = serilizerDefinition.CreateSerilizerNoCache();
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = (Func<T, TSubClass>)Delegate.CreateDelegate(typeof(RefValueExtractor<T, TSubClass>), property.GetMethod);
      IProperySerilizer<T> subClass = new SubClassJsonSerilizer<T,TSubClass>(property.Name, serilizer, valueExtractor);
      m_include.Add(Tuple.Create(property, subClass));
      return this;
    }

    public ISerilizerDefinition3<T> SubClass<TSubClass>(Expression<Func<T, TSubClass>> prop, string propName, Action<ISerilizerDefinition<TSubClass>> action)
    {
      SerilizerDefinition<TSubClass> serilizerDefinition = new SerilizerDefinition<TSubClass>(m_jsonConfiguration, m_sessionSerilizerPack,m_json);
      action.Invoke(serilizerDefinition);
      var serilizer = serilizerDefinition.CreateSerilizerNoCache();
      var property = PropertyExpressionExtractor.GetProperty(prop);
      var valueExtractor = (Func<T, TSubClass>)Delegate.CreateDelegate(typeof(RefValueExtractor<T, TSubClass>), property.GetMethod);
      IProperySerilizer<T> subClass = new SubClassJsonSerilizer<T, TSubClass>(propName, serilizer, valueExtractor);
      m_include.Add(Tuple.Create(property, subClass));
      return this;
    }

    public ISerilizerDefinition2<T> Explicit()
    {
      m_explicit = true;return this;
    }

    public ISerilizerDefinition2<T> Implicit()
    {
      m_explicit = false; return this;
    }
  }
}
