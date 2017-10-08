using System;
using System.IO;
using System.Linq.Expressions;

namespace Inferno.JSONSerilizer
{
  public interface IJSONSerilizer<T>
  {
    void Serilize(T obj, TextWriter textWriter);
  }
}