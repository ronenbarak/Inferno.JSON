namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  static class ConstsString
  {
    public static readonly char[] True = "true".ToCharArray();
    public static readonly char[] False = "false".ToCharArray();
    public static readonly char[] Null = "null".ToCharArray();
    public static readonly char[] StartDayTime = "\"\\/Date(".ToCharArray();
    public static readonly char[] EndOfTimeProp = ")\\/\"".ToCharArray();
  }
}