using System.IO;

namespace Inferno.JSONSerilizer.OtherSerilizers.Class
{
  interface IEntityJSONSerilizer<T>
  {
    void Serilize(T obj, TextWriter textWriter, char[] writeBuffer);
  }
}