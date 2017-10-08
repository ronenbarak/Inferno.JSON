using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inferno.Common
{
  static class PropertyExpressionExtractor
  {
    public static PropertyInfo GetProperty<T,T2>(Expression<Func<T,T2>> propertyLambda)
    {
      MemberExpression member = propertyLambda.Body as MemberExpression;
      if (member == null)
        throw new ArgumentException(string.Format(
          "Expression '{0}' refers to a method, not a property.",
          propertyLambda.ToString()));

      PropertyInfo propInfo = member.Member as PropertyInfo;
      if (propInfo == null)
        throw new ArgumentException(string.Format(
          "Expression '{0}' refers to a field, not a property.",
          propertyLambda.ToString()));

      return propInfo;
    }
  }
}
