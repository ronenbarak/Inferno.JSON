using System;
using System.IO;
using System.Linq.Expressions;

namespace Inferno.JSONSerilizer
{
  public interface ISerilizerDefinition3<T>
  {
    ISerilizerDefinition3<T> Ignore<TProp>(Expression<Func<T, TProp>> prop);
    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop) where TProp : struct;
    ISerilizerDefinition3<T> Include<TProp, TPropConv>(Expression<Func<T, TProp>> prop, Func<T, TPropConv> convert) where TPropConv : struct;
    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, Func<T, string> convert);

    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, Action<T, TextWriter> serilizer);
    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, string propName, Action<T, TextWriter> serilizer);

    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop,string propName) where TProp : struct;
    ISerilizerDefinition3<T> Include<TProp, TPropConv>(Expression<Func<T, TProp>> prop, string propName, Func<T, TPropConv> convert) where TPropConv : struct;
    ISerilizerDefinition3<T> Include<TProp>(Expression<Func<T, TProp>> prop, string propName, Func<T, string> convert);

    ISerilizerDefinition3<T> SubClass<TSubClass>(Expression<Func<T, TSubClass>> prop, Action<ISerilizerDefinition<TSubClass>> action);
    ISerilizerDefinition3<T> SubClass<TSubClass>(Expression<Func<T, TSubClass>> prop,string propName, Action<ISerilizerDefinition<TSubClass>> action);
  }

  public interface ISerilizerDefinition2<T>
  {
    ISerilizerDefinition2<T> CamelCase();
    ISerilizerDefinition2<T> NoInterfaceProperties();
  }

  public interface ISerilizerDefinition<T> : ISerilizerDefinition2<T>, ISerilizerDefinition3<T>
  {
    ISerilizerDefinition2<T> Explicit();
    ISerilizerDefinition2<T> Implicit();
  }
}