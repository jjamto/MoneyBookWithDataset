using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyBookWithDataset
{
    public class CSetting : arUtil.Setting
    {
       public string id { get; set; }
        public override void AfterLoad()
        {
           // throw new NotImplementedException();    
        }
        public override void AfterSave()
        {
           // throw new NotImplementedException();    
        }
    }
}
