using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inferno.JSONSerilizer.OtherSerilizers.Enum
{
  static class EnumToUnderlinedType<TEnum,TValue>
  {
    static Func<S, TTarget> Get<S, TTarget>()
    {
      var p = Expression.Parameter(typeof(S));
      var c = Expression.ConvertChecked(p, typeof(TTarget));
      return Expression.Lambda<Func<S, TTarget>>(c, p).Compile();
    }

    private readonly static Func<TEnum, TValue> m_convertToUnderlinedValue = Get<TEnum, TValue>();

    public static Func<TEnum, TValue> GetConvertionFunc()
    {
      return m_convertToUnderlinedValue;
    }
  }
}
