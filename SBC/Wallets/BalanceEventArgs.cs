using SBC.Core;
using System;

namespace SBC.Wallets
{
    public class BalanceEventArgs : EventArgs
    {
        public Transaction Transaction;
        public UInt160[] RelatedAccounts;
        public uint? Height;
        public uint Time;
    }
}
