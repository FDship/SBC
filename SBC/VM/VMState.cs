﻿using System;

namespace SBC.VM
{
    [Flags]
    public enum VMState : byte
    {
        NONE = 0,

        HALT = 1 << 0,
        FAULT = 1 << 1,
        BREAK = 1 << 2,
    }
}
