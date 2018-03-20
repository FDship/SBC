﻿using SBC.Core;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SBC.UI
{
    public partial class TradeVerificationDialog : Form
    {
        public TradeVerificationDialog(IEnumerable<TransactionOutput> outputs)
        {
            InitializeComponent();
            txOutListBox1.SetItems(outputs);
        }
    }
}
