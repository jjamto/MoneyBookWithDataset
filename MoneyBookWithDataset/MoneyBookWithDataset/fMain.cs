using System;
using System.Data;
using System.Windows.Forms;

namespace MoneyBookWithDataset
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();

            Pub.init();

            this.Text = ($"{Application.ProductName}_{Application.ProductVersion}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();

            //로그인 작업(실패시 종료)
            if (func_Login() == false)
            {
                this.Close();
            }
            //이전달 데이터 불러오기

            func_LoadData(DateTime.Now.ToString("yyyy-MM"));
        }
        Boolean func_Login()
        {
            var f = new fLogin();
            if (f.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        void func_LoadData(string mon)
        {
            //기존 목록 삭제
            lbMon.Text = mon;
            var dbfile = Util.CurrentPath + "Database\\" + mon + ".xml";
            Console.WriteLine("read file : " + dbfile);

            //불러오기
            if (System.IO.File.Exists(dbfile))
            {
                this.dataSet1.Data.ReadXml(dbfile);
            }
        }

        private void 월선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new fMon();
            if (f.ShowDialog() == DialogResult.OK)
            {
                //월다시 로드
                func_LoadData(f.selectedMonth);
            }
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void 마감ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.MsgE("준비중");
        }

        private void 로그임ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (func_Login() == false)
            {
                this.Close();
            }
        }

        private void dataBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            func_SaveData();
        }

        void func_SaveData()
        {
            var mon = lbMon.Text;
            var dbfile = Util.CurrentPath + "Database\\" + mon + ".xml";
            Console.WriteLine("read file : " + dbfile);
            this.dataSet1.Data.WriteXml(dbfile);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            func_Add();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            //삭제       
            func_Delete();
        }
        void func_Edit()
        {
            //새로운 데이터 생성
            var drv = this.bs.Current as DataRowView;
            if (drv == null)
            {
                Util.MsgE("선택된 자료가 없습니다");
                return;
            }

            var curdr = drv.Row as DataSet1.DataRow;
            curdr.PDate = DateTime.Now;

            var f = new fItem(curdr);
            if (f.ShowDialog() == DialogResult.OK)
            {
                //편집
                curdr.EndEdit();
            }
            else
            {
                curdr.RejectChanges();
            }
        }
        void func_Add()
        {
            //새로운 데이터 생성
            var newdr = this.dataSet1.Data.NewDataRow();
            newdr.PDate = DateTime.Now;

            var f = new fItem(newdr);
            if (f.ShowDialog() == DialogResult.OK)
            {
                //추가(입금 ? 출금)
                curdr.EndEdit();
                this.dataSet1.Data.AddDataRow(newdr);
            }
            else
            {
                newdr.Delete();
            }
        }
        void func_Delete()
        {
            //현재 선택된 자료는 삭제 합니다.
            var dlg = Util.MsgQ("현재 자료를 삭제하시겠습니까?");
            if (dlg != DialogResult.Yes)
            {
                return;
            }
            //현재데이터 삭제
            bs.RemoveCurrent();

        }
    }
}
