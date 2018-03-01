namespace SBC.Core
{
    public class SpentCoin
    {
        public TransactionOutput Output;
        /// <summary>
        /// 产生块高度
        /// </summary>
        public uint StartHeight;
        /// <summary>
        /// 消耗块高度
        /// </summary>
        public uint EndHeight;

        public Fixed8 Value => Output.Value;
    }
}
