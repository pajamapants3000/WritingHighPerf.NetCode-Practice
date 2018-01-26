using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class DisposableClass : IDisposable
    {
        ~DisposableClass() { Dispose(false); }

        public DisposableClass()
        {
            var memoryStream = new MemoryStream();
            var segment = new ArraySegment<byte>(memoryStream.GetBuffer(), 100, 1024);
            var blockStream = new MemoryStream(segment.Array, segment.Offset, segment.Count);
        }

        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.managedResource.Dispose();
            }

            // Cleanup unmanaged resources
            UnsafeClose(this.handle);

            // If the base class is IDisposable object
            // make sure you call:
            // base.Dispose( disposing);
        }

        public IDisposable managedResource { get; set; }
        public int handle { get; set; }

        public void UnsafeClose(int handle) { }
    }
}
