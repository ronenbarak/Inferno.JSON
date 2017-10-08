using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Inferno.JSONPrefTests
{
  public class SampleObject
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public DateTime BirthDay { get; set; }
    public double Balance { get; set; }
    public bool IsAbove18 { get; set; }

    public IEnumerable<SampleObject3> SubObject { get; set; }
  }

  public class SampleObject2
  {
    public Guid Id2 { get; set; }
    public string FirstName2 { get; set; }
    public string LastName2 { get; set; }
    public int Age2 { get; set; }
    public DateTime BirthDay2 { get; set; }
    public Decimal Balance2 { get; set; }
    public bool IsAbove182 { get; set; }

    public List<SampleObject4> SubObject { get; set; }
  }

  public class SampleObject3
  {
    public float Id2 { get; set; }
  }

  public class SampleObject4
  {
    public int Id6 { get; set; }
  }


  class Program
  {
    static void Main(string[] args)
    {
      SampleObject o = new SampleObject()
      {
        Age = 31,
        Balance = -158333.2,
        BirthDay = new DateTime(1986,11,06),
        FirstName = "Ronen",
        LastName = "Barak",
        Id = Guid.NewGuid(),
        IsAbove18 = true,
        SubObject = new List<SampleObject3>() {  new SampleObject3(){Id2 = 215236}}
      };

      SampleObject2 o2 = new SampleObject2()
      {
        Age2 = 31,
        Balance2 = -158333.2M,
        BirthDay2 = new DateTime(1986, 11, 06),
        FirstName2 = "Ronen",
        LastName2 = "Barak",
        Id2 = Guid.NewGuid(),
        IsAbove182 = true,
        SubObject = new List<SampleObject4>() { new SampleObject4() }
      };

      TextWriter textWriter = new StreamWriter(new MemoryStream());
      Stopwatch newtonsoft1 = Stopwatch.StartNew();
      Newtonsoft.Json.JsonSerializer jsonSerializer = new JsonSerializer();
      jsonSerializer.Serialize(textWriter,o);
      newtonsoft1.Stop();

      textWriter = new StreamWriter(new MemoryStream());
      Stopwatch newtonsoft2 = Stopwatch.StartNew();
      jsonSerializer.Serialize(textWriter, o2);
      newtonsoft2.Stop();

      textWriter = new StreamWriter(new MemoryStream());
      Stopwatch jil1 = Stopwatch.StartNew();
      Jil.JSON.Serialize(o, textWriter);
      jil1.Stop();
      
      textWriter = new StreamWriter(new MemoryStream());
      Stopwatch jil2 = Stopwatch.StartNew();
      Jil.JSON.Serialize(o2, textWriter);
      jil2.Stop();
      
      textWriter = new StreamWriter(new MemoryStream());
      Stopwatch inferno1 = Stopwatch.StartNew();
      Inferno.JSON.Serialize(o, textWriter);
      inferno1.Stop();

      textWriter = new StreamWriter(new MemoryStream());
      Stopwatch inferno2 = Stopwatch.StartNew();
      Inferno.JSON.Serialize(o2, textWriter);
      inferno2.Stop();

      Stopwatch newtwonsoftLoop = Stopwatch.StartNew();
      for (int i = 0; i < 100000; i++)
      {
        textWriter = new StreamWriter(new MemoryStream());
        jsonSerializer.Serialize(textWriter,o);
      }
      newtwonsoftLoop.Stop();

      Stopwatch infernoLoop = Stopwatch.StartNew();
      for (int i =0;i < 100000;i++)
      {
        textWriter = new StreamWriter(new MemoryStream());
        Inferno.JSON.Serialize(o, textWriter);
      }
      infernoLoop.Stop();

      Stopwatch JilLoop = Stopwatch.StartNew();
      for (int i = 0; i < 100000; i++)
      {
        textWriter = new StreamWriter(new MemoryStream());
        Jil.JSON.Serialize(o, textWriter);
      }
      JilLoop.Stop();


      Console.WriteLine($@"
                           newtonsoft1: {newtonsoft1.ElapsedMilliseconds}  Loop: {newtwonsoftLoop.ElapsedMilliseconds}
                           newtonsoft2: {newtonsoft2.ElapsedMilliseconds}
                           jil1: {jil1.ElapsedMilliseconds}  Loop: {JilLoop.ElapsedMilliseconds}
                           jil2: {jil2.ElapsedMilliseconds}
                           inferno1: {inferno1.ElapsedMilliseconds} Loop: {infernoLoop.ElapsedMilliseconds}
                           inferno2: {inferno2.ElapsedMilliseconds}");



      Console.ReadLine();



    }
  }
}
