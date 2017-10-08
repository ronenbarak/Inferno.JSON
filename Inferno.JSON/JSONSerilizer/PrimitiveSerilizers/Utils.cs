using System;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  public struct TwoDigits
  {
    public readonly char First;
    public readonly char Second;

    public TwoDigits(char first, char second)
    {
      First = first;
      Second = second;
    }
  }

  class Utils
  {
    public static T[] CreateArray<T>(int count, Func<int, T> generator)
    {
      if (count < 0)
      {
        throw new ArgumentOutOfRangeException("count");
      }
      else if (generator == null)
      {
        throw new ArgumentNullException("generator");
      }
      //Contract.EndContractBlock();

      var arr = new T[count];
      for (var i = 0; i < arr.Length; i++)
      {
        arr[i] = generator(i);
      }
      return arr;
    }

    public static readonly char[] DigitTriplets = CreateArray(3 * 1000, i =>
      {
        var ibase = i / 3;
        switch (i % 3)
        {
          case 0:
            return (char)('0' + ibase / 100 % 10);
          case 1:
            return (char)('0' + ibase / 10 % 10);
          case 2:
            return (char)('0' + ibase % 10);
          default:
            throw new InvalidOperationException("Unexpectedly reached default case in switch block.");
        }
      });

    public static readonly TwoDigits[] DigitPairs = CreateArray(100, i => new TwoDigits((char)('0' + (i / 10)), (char)+('0' + (i % 10))));
  }
}