using Inferno.SerilizerBufferAllocator;

namespace Inferno
{
  public enum SerilizeEnumAs
  {
    String,
    UnderlinedType,
    Both,
  }

  public interface ISerilizerBufferAllocator
  {
    char[] Allocate();
    void Free(char[] buffer);
  }

  public class JSONConfiguration
  {
    public const int SerilizerCharBufferSize = 36;

    public static readonly JSONConfiguration Global = new JSONConfiguration();

    public bool Explicit { get; set; }
    public bool CamelCaseSerilizer { get; set; } = false;
    public bool NoInterfaceProperties { get; set; } = false;
    public SerilizeEnumAs SerilizeEnumAs { get; set; } = SerilizeEnumAs.UnderlinedType;
    public ISerilizerBufferAllocator BufferAllocator { get; set; } = ThreadStaticBuffer.Instance;
  }
}