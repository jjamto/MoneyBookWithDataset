using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyBookWithDataset
{
    public static class Pub
    {
        public static arUtil.Log log;
        public static CSetting setting;
        public static DataSet1 db;

        public static void init()
        {
            log = new arUtil.Log();

            setting = new CSetting();
            setting.Load();

            db = new DataSet1(); //database

            //loginDB
            var file_dblogin = Util.CurrentPath + "Database\\login.dat";
            if (System.IO.File.Exists(file_dblogin))
            {
                db.Login.ReadXml(file_dblogin);
            }

            //기본ID설정
            if (db.Login.Rows.Count == 0)
            {
                //admin 계정 생성
                var newdr = db.Login.NewLoginRow();
                newdr.ID = "admin";
                newdr.PW = "1234";
                db.Login.AddLoginRow(newdr);
                db.Login.AcceptChanges();
                db.Login.WriteXml(file_dblogin);
            }
        }
    }
}
