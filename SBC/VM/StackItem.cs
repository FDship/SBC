﻿using SBC.VM.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Array = SBC.VM.Types.Array;
using Boolean = SBC.VM.Types.Boolean;

namespace SBC.VM
{
    public abstract class StackItem : IEquatable<StackItem>
    {
        public abstract bool Equals(StackItem other);

        public static StackItem FromInterface(IInteropInterface value)
        {
            return new InteropInterface(value);
        }

        public virtual BigInteger GetBigInteger()
        {
            return new BigInteger(GetByteArray());
        }

        public virtual bool GetBoolean()
        {
            return GetByteArray().Any(p => p != 0);
        }

        public abstract byte[] GetByteArray();

        public virtual string GetString()
        {
            return Encoding.UTF8.GetString(GetByteArray());
        }

        public static implicit operator StackItem(int value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(uint value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(long value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(ulong value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(BigInteger value)
        {
            return new Integer(value);
        }

        public static implicit operator StackItem(bool value)
        {
            return new Boolean(value);
        }

        public static implicit operator StackItem(byte[] value)
        {
            return new ByteArray(value);
        }

        public static implicit operator StackItem(StackItem[] value)
        {
            return new Array(value);
        }

        public static implicit operator StackItem(List<StackItem> value)
        {
            return new Array(value);
        }
    }
}
