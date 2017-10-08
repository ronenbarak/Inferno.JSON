using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno.SerilizerBufferAllocator
{
  class NewMemoryAllocationPerCall : ISerilizerBufferAllocator
  {
    public static readonly NewMemoryAllocationPerCall Instance = new NewMemoryAllocationPerCall();

    private NewMemoryAllocationPerCall()
    {
      
    }
    public char[] Allocate()
    {
      return new char[JSONConfiguration.SerilizerCharBufferSize];
    }

    public void Free(char[] buffer)
    {
    }
  }
}
