using KellermanSoftware.CompareNetObjects;

namespace Inferno.JSONTests
{
  public static class DeepEquals
  {
    public static  void Assert<T>(T expected, T actual)
    {
      var logic = new CompareLogic();
      var compare = logic.Compare(expected, actual);
      NUnit.Framework.Assert.IsTrue(compare.AreEqual,compare.DifferencesString);
    }
  }
}