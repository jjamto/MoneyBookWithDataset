using System;
using System.Data;
using System.Windows.Forms;
using System.Linq;

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
            this.dataSet1.Data.Clear();

            lbMon.Text = mon;
            var dbfile = Util.CurrentPath + "Database\\" + mon + ".xml";
            Console.WriteLine("read file : " + dbfile);

            //불러오기
            if (System.IO.File.Exists(dbfile))
            {
                this.dataSet1.Data.ReadXml(dbfile);
                this.dataSet1.Data.AcceptChanges();
            }
            func_summary();
        }

        private void 월선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var od = new OpenFileDialog();
            od.Filter = "MoneyBoook Database File(*.xml)|*.xml";
            od.InitialDirectory = Util.CurrentPath + "Database";
            if (od.ShowDialog() == DialogResult.OK)
            {
                //월다시 로드
                var fi = new System.IO.FileInfo(od.FileName);
                func_LoadData(fi.Name.Substring(0, fi.Name.Length-4));
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
            //현재선택된 데이터의 마감작업
            var lockCount = this.dataSet1.Data.Where(t => t.RowState != DataRowState.Deleted && t.Lock == true).Count();
            if (lockCount > 0)
            {
                var dlg = Util.MsgQ("월 마감을 해제 하시겠습니까?");
                if (dlg == DialogResult.Yes)
                {
                    // 마감해제
                    func_Mon_Reject();
                }
            }
            else
            {
                var dlg = Util.MsgQ("월 마감을 하시겠습니까?");
                if (dlg == DialogResult.Yes)
                {
                    func_Mon_Commit();
                }
            }


        }

        void func_Mon_Commit()
        {
            //이번달의 잔액을 다음달 1일에 "이월" 항목으로 입금액에 추가한다
            var currentMon = DateTime.Parse(lbMon.Text + "-01");
            var nextMon = currentMon.AddMonths(1);
            var nextFile = Util.CurrentPath + "Database\\" + nextMon.ToString("yyyy-MM") + ".xml";

            var dsNext = new DataSet1();
            if (System.IO.File.Exists(nextFile) == false)           
            {
                var newdr = dsNext.Data.NewDataRow();
                newdr.PDate = nextMon;
                newdr.Grp = "이월";
                newdr.Dr = decimal.Parse(lbSumJan.Text.Replace(",", ""));
                newdr.EndEdit();
            }
            else
            {
                //파일이 존재할때 - 이월데이터를 변경 해준다(없으면 생성)
                dsNext.Data.ReadXml(nextFile);
                var sumDr = dsNext.Data.Where(t => t.Grp == "이월").FirstOrDefault();
                if (sumDr == null)
                {
                    var newdr = dsNext.Data.NewDataRow();
                    newdr.PDate = nextMon;
                    newdr.Grp = "이월";
                    newdr.Dr = decimal.Parse(lbSumJan.Text.Replace(",", ""));
                    newdr.EndEdit();
                    dsNext.Data.AddDataRow(newdr);

                }
                else
                {
                    sumDr.PDate = nextMon;
                    sumDr.Grp = "이월";
                    sumDr.Dr = decimal.Parse(lbSumJan.Text.Replace(",", ""));
                    sumDr.EndEdit();
                }         
            }
            dsNext.Data.AcceptChanges();
            dsNext.Data.WriteXml(nextFile);


            //모든 데이터의 lock을 설정한다
            for (int i = 0; i < this.dataSet1.Data.Rows.Count; i++)
            {
                var dr = this.dataSet1.Data.Rows[i] as DataSet1.DataRow;
                dr.Lock = true;
                dr.EndEdit();
            }
            this.dataSet1.Data.AcceptChanges();
            func_SaveData();

            //다음달 데이터 자동 로딩
            func_LoadData(nextMon.ToString("yyyy-MM"));
            Pub.log.Add("마감" + lbMon.Text + ",잔액 =" + lbSumJan.Text);

        }

        void func_Mon_Reject()
        {
            //다음달 1일의 이월 항목도 삭제를 하고,
            var currentMon = DateTime.Parse(lbMon.Text + "-01");
            var nextMon = currentMon.AddMonths(1);
            var nextFile = Util.CurrentPath + "Database\\" + nextMon.ToString("yyyy-MM") + ".xml";

            var dsNext = new DataSet1();
            if (System.IO.File.Exists(nextFile))
            {
                dsNext.Data.ReadXml(nextFile);
                var sumDr = dsNext.Data.Where(t => t.Grp == "이월").FirstOrDefault();
                if (sumDr != null)
                {
                    sumDr.Delete();
                }
                dsNext.Data.AcceptChanges();
                dsNext.Data.WriteXml(nextFile);

                //모든데이터의 lock를 해제한다
                foreach (DataSet1.DataRow dr in this.dataSet1.Data.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dr.Lock = false;
                    }
                    
                }
                this.dataSet1.Data.AcceptChanges();
                func_SaveData();
                Pub.log.Add("마감해제" + lbMon.Text);

            }

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
            if (curdr.Grp == "이월")
            {
                Util.MsgE("이월 데이터는 편집할 수 없습니다");
                return;
            }

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
            func_summary();
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
                newdr.EndEdit();
                if (newdr.Grp == "이월")
                {
                    Util.MsgE("이월 데이터는 생성할 수 없습니다");
                    newdr.Delete();
                }
                else
                {
                    this.dataSet1.Data.AddDataRow(newdr);
                }            
            }
            else
            {
                newdr.Delete();
            }
            func_summary();
        }
        void func_Delete()
        {
            var drv = bs.Current as DataRowView;
            if (drv == null)
            {
                Util.MsgE("삭제할 자료를 선택하세요");
            }

            var dr = drv.Row as DataSet1.DataRow;
            if (dr.Lock)
            {
                Util.MsgE("마간된 자료는 삭제할 수 없습니다");
                return;
            }

            if (dr.Grp == "이월")
            {
                Util.MsgE("이월 데이터는 삭제할 수 없습니다.1");
                return;
            }


            //현재 선택된 자료는 삭제 합니다.
            var dlg = Util.MsgQ("현재 자료를 삭제하시겠습니까?");
            if (dlg != DialogResult.Yes)
            {
                return;
            }


            //현재데이터 삭제
            bs.RemoveCurrent();
            func_summary();

        }
        void func_summary()
        {
            decimal prejan = this.dataSet1.Data.Where(t => t.RowState != DataRowState.Deleted && t.Grp == "이월").Sum(t => t.Dr); //이월잔액
            var sumDr = this.dataSet1.Data.Where(t => t.RowState != DataRowState.Deleted && t.Grp != "이월").Sum(t => t.Dr);
            var sumCr = this.dataSet1.Data.Where(t => t.RowState != DataRowState.Deleted && t.Grp != "이월").Sum(t => t.Cr);
            var jan = prejan + sumDr - sumCr;
            this.lbPreJan.Text = prejan.ToString("N0");
            this.lbSumDr.Text = sumDr.ToString("N0");
            this.lbSumCr.Text = sumCr.ToString("N0");
            this.lbSumJan.Text = jan.ToString("N0");
        }

        private void dataDataGridView_DoubleClick(object sender, EventArgs e)
        {
            func_Edit();
        }
    }
}
