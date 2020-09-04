using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyBookWithDataset
{
    public partial class fMon : Form
    {
        public string selectedMonth = string.Empty;
        public fMon()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //현재 선택된 월을 설정한다
            selectedMonth = "";
            DialogResult = DialogResult.OK;
        }


    }
}
