﻿using SBC.Core;
using SBC.IO.Caching;
using SBC.VM;

namespace SBC.SmartContract
{
    internal class CachedScriptTable : IScriptTable
    {
        private DataCache<UInt160, ContractState> contracts;

        public CachedScriptTable(DataCache<UInt160, ContractState> contracts)
        {
            this.contracts = contracts;
        }

        byte[] IScriptTable.GetScript(byte[] script_hash)
        {
            return contracts[new UInt160(script_hash)].Script;
        }

        public ContractState GetContractState(byte[] script_hash)
        {
            return contracts[new UInt160(script_hash)];
        }
    }
}
