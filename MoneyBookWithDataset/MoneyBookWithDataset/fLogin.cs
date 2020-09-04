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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        private void fLogin_Load(object sender, EventArgs e)
        {
            //마지막 사용 id 불러오기
            tbID.Text = Pub.setting.id;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //아이디와 암호를 검증
            var id = tbID.Text.Trim();
            var pw = tbPW.Text.Trim();

            var dr = Pub.db.Login.Where(t => t.ID == id && t.PW == pw).FirstOrDefault();
            if (dr != null)
            {
                //로그인 id 저장
                if (Pub.setting.id != id)
                {
                    Pub.setting.id = id;
                    Pub.setting.Save();
                }
                
                
                // 맞으면 ok
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                Util.MsgE("해당 사용자가 존재하지 않습니다.");
                tbPW.Focus();
                tbPW.SelectAll();
            }
         
   
 
        }

       
    }
}
