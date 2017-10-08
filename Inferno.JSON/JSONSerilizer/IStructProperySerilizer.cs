using System.IO;

namespace Inferno.JSONSerilizer
{
  delegate TReturn RefValueExtractor<T, out TReturn>(ref T obj);

  interface IStructProperySerilizer<T>
  {
    void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer);
  }
}