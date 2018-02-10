using SBC.VM;
using System;

namespace SBC.SmartContract
{
    internal abstract class Iterator : IDisposable, IInteropInterface
    {
        public abstract void Dispose();
        public abstract StackItem Key();
        public abstract bool Next();
        public abstract StackItem Value();
    }
}
