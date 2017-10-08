using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno.JSONSerilizer.OtherSerilizers.Enum
{
  interface IEnumPropertySerilizer
  {
    IProperySerilizer<T> CreateEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate);
    IStructProperySerilizer<T> CreateStructEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate);
    IProperySerilizer<T> CreateNullableEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate);
    IStructProperySerilizer<T> CreateStructNullableEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate);
  }

  class EnumPropertySerilizer<TProp> : IEnumPropertySerilizer where TProp:struct
  {
    public IProperySerilizer<T> CreateEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate)
    {
      Func<T, TProp> valueExtractor = (Func<T, TProp>)valueExtractorDelegate;

      if (configuration.SerilizeEnumAs == SerilizeEnumAs.UnderlinedType)
      {
        var underlinedType = typeof(TProp).GetEnumUnderlyingType();

        if (underlinedType == typeof(int))
        {
          var convFunc = EnumToUnderlinedType<TProp, int>.GetConvertionFunc();
          Func<T, int> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new IntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(byte))
        {
          var convFunc = EnumToUnderlinedType<TProp, byte>.GetConvertionFunc();
          Func<T, int> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new IntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(sbyte))
        {
          var convFunc = EnumToUnderlinedType<TProp, sbyte>.GetConvertionFunc();
          Func<T, int> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new IntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(short))
        {
          var convFunc = EnumToUnderlinedType<TProp, short>.GetConvertionFunc();
          Func<T, int> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new IntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ushort))
        {
          var convFunc = EnumToUnderlinedType<TProp, ushort>.GetConvertionFunc();
          Func<T, int> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new IntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(uint))
        {
          var convFunc = EnumToUnderlinedType<TProp, ushort>.GetConvertionFunc();
          Func<T, uint> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new UIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(long))
        {
          var convFunc = EnumToUnderlinedType<TProp, long>.GetConvertionFunc();
          Func<T, long> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new LongSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ulong))
        {
          var convFunc = EnumToUnderlinedType<TProp, ulong>.GetConvertionFunc();
          Func<T, ulong> underlinedValueFunction = (arg => {return convFunc.Invoke(valueExtractor.Invoke(arg));});
          return new ULongSerilizer<T>(propName, underlinedValueFunction);
        }
      }

      return null;
    }

    public IProperySerilizer<T> CreateNullableEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName, object valueExtractorDelegate)
    {
      Func<T, TProp?> valueExtractor = (Func<T, TProp?>)valueExtractorDelegate;

      if (configuration.SerilizeEnumAs == SerilizeEnumAs.UnderlinedType)
      {
        var underlinedType = typeof(TProp).GetEnumUnderlyingType();

        if (underlinedType == typeof(int))
        {
          var convFunc = EnumToUnderlinedType<TProp?, int?>.GetConvertionFunc();
          Func<T, int?> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(byte))
        {
          var convFunc = EnumToUnderlinedType<TProp?, byte?>.GetConvertionFunc();
          Func<T, int?> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(sbyte))
        {
          var convFunc = EnumToUnderlinedType<TProp?, sbyte?>.GetConvertionFunc();
          Func<T, int?> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(short))
        {
          var convFunc = EnumToUnderlinedType<TProp?, short?>.GetConvertionFunc();
          Func<T, int?> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ushort))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ushort?>.GetConvertionFunc();
          Func<T, int?> underlinedValueFunction = (arg => (int)convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(uint))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ushort?>.GetConvertionFunc();
          Func<T, uint?> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableUIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(long))
        {
          var convFunc = EnumToUnderlinedType<TProp?, long?>.GetConvertionFunc();
          Func<T, long?> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableLongSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ulong))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ulong?>.GetConvertionFunc();
          Func<T, ulong?> underlinedValueFunction = (arg => convFunc.Invoke(valueExtractor.Invoke(arg)));
          return new NullableULongSerilizer<T>(propName, underlinedValueFunction);
        }
      }

      return null;
    }

    public IStructProperySerilizer<T> CreateStructEnumPropertySerilizer<T>(JSONConfiguration configuration, string propName,object valueExtractorDelegate)
    {
      RefValueExtractor<T, TProp> valueExtractor = (RefValueExtractor<T, TProp>)valueExtractorDelegate;

      if (configuration.SerilizeEnumAs == SerilizeEnumAs.UnderlinedType)
      {
        var underlinedType = typeof(TProp).GetEnumUnderlyingType();

        if (underlinedType == typeof(int))
        {
          var convFunc = EnumToUnderlinedType<TProp, int>.GetConvertionFunc();
          RefValueExtractor<T, int> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(byte))
        {
          var convFunc = EnumToUnderlinedType<TProp, byte>.GetConvertionFunc();
          RefValueExtractor<T, int> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(sbyte))
        {
          var convFunc = EnumToUnderlinedType<TProp, sbyte>.GetConvertionFunc();
          RefValueExtractor<T, int> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(short))
        {
          var convFunc = EnumToUnderlinedType<TProp, short>.GetConvertionFunc();
          RefValueExtractor<T, int> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ushort))
        {
          var convFunc = EnumToUnderlinedType<TProp, ushort>.GetConvertionFunc();
          RefValueExtractor<T, int> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(uint))
        {
          var convFunc = EnumToUnderlinedType<TProp, ushort>.GetConvertionFunc();
          RefValueExtractor<T, uint> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructUIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(long))
        {
          var convFunc = EnumToUnderlinedType<TProp, long>.GetConvertionFunc();
          RefValueExtractor<T, long> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructLongSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ulong))
        {
          var convFunc = EnumToUnderlinedType<TProp, ulong>.GetConvertionFunc();
          RefValueExtractor<T, ulong> underlinedValueFunction = ((ref T arg) => { return convFunc.Invoke(valueExtractor.Invoke(ref arg)); });
          return new StructULongSerilizer<T>(propName, underlinedValueFunction);
        }
      }

      return null;
    }

    public IStructProperySerilizer<T> CreateStructNullableEnumPropertySerilizer<T>(JSONConfiguration configuration,string propName, object valueExtractorDelegate)
    {
      RefValueExtractor<T, TProp?> valueExtractor = (RefValueExtractor<T, TProp?>)valueExtractorDelegate;

      if (configuration.SerilizeEnumAs == SerilizeEnumAs.UnderlinedType)
      {
        var underlinedType = typeof(TProp).GetEnumUnderlyingType();

        if (underlinedType == typeof(int))
        {
          var convFunc = EnumToUnderlinedType<TProp?, int?>.GetConvertionFunc();
          RefValueExtractor<T, int?> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(byte))
        {
          var convFunc = EnumToUnderlinedType<TProp?, byte?>.GetConvertionFunc();
          RefValueExtractor<T, int?> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(sbyte))
        {
          var convFunc = EnumToUnderlinedType<TProp?, sbyte?>.GetConvertionFunc();
          RefValueExtractor<T, int?> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(short))
        {
          var convFunc = EnumToUnderlinedType<TProp?, short?>.GetConvertionFunc();
          RefValueExtractor<T, int?> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ushort))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ushort?>.GetConvertionFunc();
          RefValueExtractor<T, int?> underlinedValueFunction = ((ref T arg) => (int)convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(uint))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ushort?>.GetConvertionFunc();
          RefValueExtractor<T, uint?> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableUIntSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(long))
        {
          var convFunc = EnumToUnderlinedType<TProp?, long?>.GetConvertionFunc();
          RefValueExtractor<T, long?> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableLongSerilizer<T>(propName, underlinedValueFunction);
        }
        else if (underlinedType == typeof(ulong))
        {
          var convFunc = EnumToUnderlinedType<TProp?, ulong?>.GetConvertionFunc();
          RefValueExtractor<T, ulong?> underlinedValueFunction = ((ref T arg) => convFunc.Invoke(valueExtractor.Invoke(ref arg)));
          return new StructNullableULongSerilizer<T>(propName, underlinedValueFunction);
        }
      }

      return null;
    }
  }
}
