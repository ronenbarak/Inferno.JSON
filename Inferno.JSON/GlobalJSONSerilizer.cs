using Inferno.JSONSerilizer;

namespace Inferno
{
  static class GlobalJSONSerilizer<T> where T :class
  {
    static GlobalJSONSerilizer()
    {
      Instance = JSON.GlobalJSON.CreateTypeSerilizer<T>();
    }

    public static readonly IJSONSerilizer<T> Instance;
  }
}