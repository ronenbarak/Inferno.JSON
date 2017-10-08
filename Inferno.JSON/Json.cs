using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer;

namespace Inferno
{
  public interface IJSON
  {
    IJSONSerilizer<T> CreateTypeSerilizer<T>(Action<ISerilizerDefinition<T>> action);
    IJSONSerilizer<T> CreateTypeSerilizer<T>();
    string Serilize<T>(T obj);
  }

  public static class JSON
  {
    public static readonly IJSON GlobalJSON = new GroupJson(JSONConfiguration.Global);

    public static void Serialize<T>(T obj, TextWriter textWriter) where T: class
    {
      GlobalJSONSerilizer<T>.Instance.Serilize(obj,textWriter);
    }

    public static string Serialize<T>(T obj) where T : class
    {
      StringWriter textWriter = new StringWriter();
      GlobalJSONSerilizer<T>.Instance.Serilize(obj, textWriter);
      var s = textWriter.ToString();
      return s;
    }

    public static IJSONSerilizer<T> CreateTypeSerilizer<T>(Action<ISerilizerDefinition<T>> action) where T : class
    {
      return GlobalJSON.CreateTypeSerilizer(action);
    }

    public static IJSON CreateSerilizer(JSONConfiguration configuration)
    {
      return new GroupJson(configuration);
    }

  }
}
