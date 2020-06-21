using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class PqQytDto: PqQyModel
    {
        private List<PqQyPjtcModel> _pjtclist = new List<PqQyPjtcModel>();
        public List<PqQyPjtcModel> pjtcList
        {
            get { return _pjtclist; }
            set { _pjtclist = value; }
        }
    }
}
