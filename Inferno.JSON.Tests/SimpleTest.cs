using System;
using System.Collections.Generic;
using Jil;
using NUnit.Framework;

namespace Inferno.JSONTests
{
  public enum SomeEnum
  {
    FirstValue,
    SecondValue,
  }

  public enum EmptyEnum
  {
    
  }

  public class EmptyClass
  {
  
  }
  public class TestClass02
  {
    public TestClass02()
    {
      TestClass02A = 4;
    }
    public int TestClass02A { get; set; }
  }

  public class TestClass03
  {
    public TestClass03()
    {
    }
    public List<TestClass03> TestClass02A { get; set; }
  }

  public class GenericType<T>
  {
    public T Item1 { get; set; }
  }
  public class TestClass01
  {
    public string StringValue { get; set; }
    public int? IntValue { get; set; }
    public TestClass01 SubClass { get; set; }
    public SomeOtherType OtherValue1 { get; set; }
    public SomeOtherType OtherValue2 { get; set; }
    public TestStruct Struct { get; set; }
    [JilDirective(TreatEnumerationAs = typeof(int))]
    public SomeEnum MyEnum { get; set; }
    public int[] MyArray { get; set; }
    public List<GenericType<IEnumerable<TestClass02>>> ComplexList1 { get; set; }
    public List<TestClass03> ComplexList2 { get; set; }
    public EmptyEnum EmptyEnum { get; set; }
    public EmptyClass EmptyClass { get; set; }
  }

  public struct TestStruct
  {
    public int StructValue { get; set; }
    public string StructValue2 { get; set; }
    public TestStruct2 DeepStruct { get; set; }
  }

  public struct TestStruct2
  {
    public SomeOtherType ClassType { get; set; }
    public int StructValue { get; set; }
    public string StructValue2 { get; set; }
  }
  public class SomeOtherType
  {
    public string MyValue { get; set; }
    public int MyOthreValue { get; set; }
  }

  [TestFixture]
  public class SimpleTest
  {
    [Test]
    public void SimpleClassIsSerilized()
    {
      var testData = new TestClass01() { IntValue = 5, StringValue = "ronen", SubClass = new TestClass01(), OtherValue1 = new SomeOtherType() { MyOthreValue = 2 }, Struct = new TestStruct() { StructValue = 8, StructValue2 = "barak",DeepStruct = new TestStruct2(){ClassType = new SomeOtherType(){MyValue = "VeryDeep"}}} , MyEnum =SomeEnum.SecondValue , MyArray  = new int[]{5,6}, ComplexList1 = new List<GenericType<IEnumerable<TestClass02>>>(){new GenericType<IEnumerable<TestClass02>>(){Item1 = new List<TestClass02>(){new TestClass02(), new TestClass02() }}} , ComplexList2 = new List<TestClass03>(){new TestClass03()}, EmptyClass  = new EmptyClass()};
      var json = Inferno.JSON.Serialize(testData);
      var desData = Newtonsoft.Json.JsonConvert.DeserializeObject<TestClass01>(json);
      DeepEquals.Assert(testData, desData);
      //desData = Jil.JSON.Deserialize<TestClass01>(json);      
      //DeepEquals.Assert(testData, desData);
    }
  }
}
