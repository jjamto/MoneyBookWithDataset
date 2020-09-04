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
    public partial class fItem : Form
    {
        public fItem(DataSet1.DataRow dr)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //데이터 검증 (binding source에 걸려있을때는 검증 필수 : 아래 두가지 명령어는 필수)
            this.Validate();
            this.bs.EndEdit();
            DialogResult = DialogResult.OK;
        }
    }
}
