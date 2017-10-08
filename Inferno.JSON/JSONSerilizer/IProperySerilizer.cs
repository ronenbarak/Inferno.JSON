using System.IO;

namespace Inferno.JSONSerilizer
{
  interface IProperySerilizer<T>
  {
    void Serilize(T obj, TextWriter textWriter,char[] writeBuffer);
  }
}