namespace Inferno.JSONSerilizer.OtherSerilizers.Class
{
  interface IFullSerilizer<T> : IJSONSerilizer<T> , IEntityJSONSerilizer<T>
  {
  }
}