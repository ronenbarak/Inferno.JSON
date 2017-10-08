using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Inferno.JSONSerilizer.OtherSerilizers.Class;

namespace Inferno.JSONSerilizer.PrimitiveSerilizers
{
  class EntityGuidSerilier : IFullSerilizer<Guid>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityGuidSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }
    public void Serilize(Guid obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(Guid obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write('\"');
      GuidCustomSerilizer._WriteGuid(textWriter, obj, writeBuffer);
      textWriter.Write('\"');
    }
  }

  class EntityGuidNullSerilier : IFullSerilizer<Guid?>
  {
    private JSONConfiguration m_jsonConfiguration;

    public EntityGuidNullSerilier(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
    }

    public void Serilize(Guid? obj, TextWriter textWriter)
    {
      var bufferAllocator = m_jsonConfiguration.BufferAllocator;
      var buffer = bufferAllocator.Allocate();
      Serilize(obj, textWriter, buffer);
      bufferAllocator.Free(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serilize(Guid? obj, TextWriter textWriter, char[] writeBuffer)
    {
      if (obj.HasValue)
      {
        textWriter.Write('\"');
        GuidCustomSerilizer._WriteGuid(textWriter, obj.Value, writeBuffer);
        textWriter.Write('\"');
      }
      else
      {
        textWriter.Write(ConstsString.Null);
      }
    }
  }

  class StructGuidSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly RefValueExtractor<T, Guid> m_valueExtractor;

    public StructGuidSerilizer(string properyName, RefValueExtractor<T, Guid> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\"").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GuidCustomSerilizer._WriteGuid(textWriter,m_valueExtractor.Invoke(ref obj), writeBuffer);
      textWriter.Write('\"');
    }
  }

  class StructNullableGuidSerilizer<T> : IStructProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly RefValueExtractor<T, Guid?> m_valueExtractor;

    public StructNullableGuidSerilizer(string properyName, RefValueExtractor<T, Guid?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\"").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(ref T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(ref obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        GuidCustomSerilizer._WriteGuid(textWriter, value.Value, writeBuffer);
        textWriter.Write('\"');
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  class GuidSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly Func<T, Guid> m_valueExtractor;

    public GuidSerilizer(string properyName, Func<T, Guid> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\"").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      textWriter.Write(m_property);
      GuidCustomSerilizer._WriteGuid(textWriter, m_valueExtractor.Invoke(obj), writeBuffer);
      textWriter.Write('\"');
    }
  }

  class NullableGuidSerilizer<T> : IProperySerilizer<T>
  {
    private readonly char[] m_property;
    private readonly char[] m_propertyNull;
    private readonly Func<T, Guid?> m_valueExtractor;

    public NullableGuidSerilizer(string properyName, Func<T, Guid?> valueExtractor)
    {
      m_property = ("\"" + properyName + "\":\"").ToCharArray();
      m_propertyNull = ("\"" + properyName + "\":null").ToCharArray();
      m_valueExtractor = valueExtractor;
    }

    public void Serilize(T obj, TextWriter textWriter, char[] writeBuffer)
    {
      var value = m_valueExtractor.Invoke(obj);
      if (value.HasValue)
      {
        textWriter.Write(m_property);
        GuidCustomSerilizer._WriteGuid(textWriter, value.Value, writeBuffer);
        textWriter.Write('\"');
      }
      else
      {
        textWriter.Write(m_propertyNull);
      }
    }
  }

  static class GuidCustomSerilizer
  {
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    struct GuidStruct
    {
      [FieldOffset(0)]
      private Guid Value;

      [FieldOffset(0)]
      public readonly byte B00;
      [FieldOffset(1)]
      public readonly byte B01;
      [FieldOffset(2)]
      public readonly byte B02;
      [FieldOffset(3)]
      public readonly byte B03;
      [FieldOffset(4)]
      public readonly byte B04;
      [FieldOffset(5)]
      public readonly byte B05;

      [FieldOffset(6)]
      public readonly byte B06;
      [FieldOffset(7)]
      public readonly byte B07;
      [FieldOffset(8)]
      public readonly byte B08;
      [FieldOffset(9)]
      public readonly byte B09;

      [FieldOffset(10)]
      public readonly byte B10;
      [FieldOffset(11)]
      public readonly byte B11;

      [FieldOffset(12)]
      public readonly byte B12;
      [FieldOffset(13)]
      public readonly byte B13;
      [FieldOffset(14)]
      public readonly byte B14;
      [FieldOffset(15)]
      public readonly byte B15;

      public GuidStruct(Guid invisibleMembers)
        : this()
      {
        Value = invisibleMembers;
      }
    }

    static readonly char[] WriteGuidLookup = new char[] { '0', '0', '0', '1', '0', '2', '0', '3', '0', '4', '0', '5', '0', '6', '0', '7', '0', '8', '0', '9', '0', 'a', '0', 'b', '0', 'c', '0', 'd', '0', 'e', '0', 'f', '1', '0', '1', '1', '1', '2', '1', '3', '1', '4', '1', '5', '1', '6', '1', '7', '1', '8', '1', '9', '1', 'a', '1', 'b', '1', 'c', '1', 'd', '1', 'e', '1', 'f', '2', '0', '2', '1', '2', '2', '2', '3', '2', '4', '2', '5', '2', '6', '2', '7', '2', '8', '2', '9', '2', 'a', '2', 'b', '2', 'c', '2', 'd', '2', 'e', '2', 'f', '3', '0', '3', '1', '3', '2', '3', '3', '3', '4', '3', '5', '3', '6', '3', '7', '3', '8', '3', '9', '3', 'a', '3', 'b', '3', 'c', '3', 'd', '3', 'e', '3', 'f', '4', '0', '4', '1', '4', '2', '4', '3', '4', '4', '4', '5', '4', '6', '4', '7', '4', '8', '4', '9', '4', 'a', '4', 'b', '4', 'c', '4', 'd', '4', 'e', '4', 'f', '5', '0', '5', '1', '5', '2', '5', '3', '5', '4', '5', '5', '5', '6', '5', '7', '5', '8', '5', '9', '5', 'a', '5', 'b', '5', 'c', '5', 'd', '5', 'e', '5', 'f', '6', '0', '6', '1', '6', '2', '6', '3', '6', '4', '6', '5', '6', '6', '6', '7', '6', '8', '6', '9', '6', 'a', '6', 'b', '6', 'c', '6', 'd', '6', 'e', '6', 'f', '7', '0', '7', '1', '7', '2', '7', '3', '7', '4', '7', '5', '7', '6', '7', '7', '7', '8', '7', '9', '7', 'a', '7', 'b', '7', 'c', '7', 'd', '7', 'e', '7', 'f', '8', '0', '8', '1', '8', '2', '8', '3', '8', '4', '8', '5', '8', '6', '8', '7', '8', '8', '8', '9', '8', 'a', '8', 'b', '8', 'c', '8', 'd', '8', 'e', '8', 'f', '9', '0', '9', '1', '9', '2', '9', '3', '9', '4', '9', '5', '9', '6', '9', '7', '9', '8', '9', '9', '9', 'a', '9', 'b', '9', 'c', '9', 'd', '9', 'e', '9', 'f', 'a', '0', 'a', '1', 'a', '2', 'a', '3', 'a', '4', 'a', '5', 'a', '6', 'a', '7', 'a', '8', 'a', '9', 'a', 'a', 'a', 'b', 'a', 'c', 'a', 'd', 'a', 'e', 'a', 'f', 'b', '0', 'b', '1', 'b', '2', 'b', '3', 'b', '4', 'b', '5', 'b', '6', 'b', '7', 'b', '8', 'b', '9', 'b', 'a', 'b', 'b', 'b', 'c', 'b', 'd', 'b', 'e', 'b', 'f', 'c', '0', 'c', '1', 'c', '2', 'c', '3', 'c', '4', 'c', '5', 'c', '6', 'c', '7', 'c', '8', 'c', '9', 'c', 'a', 'c', 'b', 'c', 'c', 'c', 'd', 'c', 'e', 'c', 'f', 'd', '0', 'd', '1', 'd', '2', 'd', '3', 'd', '4', 'd', '5', 'd', '6', 'd', '7', 'd', '8', 'd', '9', 'd', 'a', 'd', 'b', 'd', 'c', 'd', 'd', 'd', 'e', 'd', 'f', 'e', '0', 'e', '1', 'e', '2', 'e', '3', 'e', '4', 'e', '5', 'e', '6', 'e', '7', 'e', '8', 'e', '9', 'e', 'a', 'e', 'b', 'e', 'c', 'e', 'd', 'e', 'e', 'e', 'f', 'f', '0', 'f', '1', 'f', '2', 'f', '3', 'f', '4', 'f', '5', 'f', '6', 'f', '7', 'f', '8', 'f', '9', 'f', 'a', 'f', 'b', 'f', 'c', 'f', 'd', 'f', 'e', 'f', 'f' };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void _WriteGuid(TextWriter writer, Guid guid, char[] buffer)
    {
      // 1314FAD4-7505-439D-ABD2-DBD89242928C
      // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
      //
      // Guid is guaranteed to be a 36 character string

      // get all the dashes in place
      buffer[8] = '-';
      buffer[13] = '-';
      buffer[18] = '-';
      buffer[23] = '-';

      // Bytes are in a different order than you might expect
      // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
      // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
      // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
      //
      // And we have to account for dashes
      //
      // So the map is like so:
      // bytes[0]  -> chars[3]  -> buffer[ 6, 7]
      // bytes[1]  -> chars[2]  -> buffer[ 4, 5]
      // bytes[2]  -> chars[1]  -> buffer[ 2, 3]
      // bytes[3]  -> chars[0]  -> buffer[ 0, 1]
      // bytes[4]  -> chars[5]  -> buffer[11,12]
      // bytes[5]  -> chars[4]  -> buffer[ 9,10]
      // bytes[6]  -> chars[7]  -> buffer[16,17]
      // bytes[7]  -> chars[6]  -> buffer[14,15]
      // bytes[8]  -> chars[8]  -> buffer[19,20]
      // bytes[9]  -> chars[9]  -> buffer[21,22]
      // bytes[10] -> chars[10] -> buffer[24,25]
      // bytes[11] -> chars[11] -> buffer[26,27]
      // bytes[12] -> chars[12] -> buffer[28,29]
      // bytes[13] -> chars[13] -> buffer[30,31]
      // bytes[14] -> chars[14] -> buffer[32,33]
      // bytes[15] -> chars[15] -> buffer[34,35]
      var visibleMembers = new GuidStruct(guid);

      // bytes[0]
      var b = visibleMembers.B00 * 2;
      buffer[6] = WriteGuidLookup[b];
      buffer[7] = WriteGuidLookup[b + 1];

      // bytes[1]
      b = visibleMembers.B01 * 2;
      buffer[4] = WriteGuidLookup[b];
      buffer[5] = WriteGuidLookup[b + 1];

      // bytes[2]
      b = visibleMembers.B02 * 2;
      buffer[2] = WriteGuidLookup[b];
      buffer[3] = WriteGuidLookup[b + 1];

      // bytes[3]
      b = visibleMembers.B03 * 2;
      buffer[0] = WriteGuidLookup[b];
      buffer[1] = WriteGuidLookup[b + 1];

      // bytes[4]
      b = visibleMembers.B04 * 2;
      buffer[11] = WriteGuidLookup[b];
      buffer[12] = WriteGuidLookup[b + 1];

      // bytes[5]
      b = visibleMembers.B05 * 2;
      buffer[9] = WriteGuidLookup[b];
      buffer[10] = WriteGuidLookup[b + 1];

      // bytes[6]
      b = visibleMembers.B06 * 2;
      buffer[16] = WriteGuidLookup[b];
      buffer[17] = WriteGuidLookup[b + 1];

      // bytes[7]
      b = visibleMembers.B07 * 2;
      buffer[14] = WriteGuidLookup[b];
      buffer[15] = WriteGuidLookup[b + 1];

      // bytes[8]
      b = visibleMembers.B08 * 2;
      buffer[19] = WriteGuidLookup[b];
      buffer[20] = WriteGuidLookup[b + 1];

      // bytes[9]
      b = visibleMembers.B09 * 2;
      buffer[21] = WriteGuidLookup[b];
      buffer[22] = WriteGuidLookup[b + 1];

      // bytes[10]
      b = visibleMembers.B10 * 2;
      buffer[24] = WriteGuidLookup[b];
      buffer[25] = WriteGuidLookup[b + 1];

      // bytes[11]
      b = visibleMembers.B11 * 2;
      buffer[26] = WriteGuidLookup[b];
      buffer[27] = WriteGuidLookup[b + 1];

      // bytes[12]
      b = visibleMembers.B12 * 2;
      buffer[28] = WriteGuidLookup[b];
      buffer[29] = WriteGuidLookup[b + 1];

      // bytes[13]
      b = visibleMembers.B13 * 2;
      buffer[30] = WriteGuidLookup[b];
      buffer[31] = WriteGuidLookup[b + 1];

      // bytes[14]
      b = visibleMembers.B14 * 2;
      buffer[32] = WriteGuidLookup[b];
      buffer[33] = WriteGuidLookup[b + 1];

      // bytes[15]
      b = visibleMembers.B15 * 2;
      buffer[34] = WriteGuidLookup[b];
      buffer[35] = WriteGuidLookup[b + 1];

      writer.Write(buffer, 0, 36);
    }
  }
}