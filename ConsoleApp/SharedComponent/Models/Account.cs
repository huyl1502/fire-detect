using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Account : BaseModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
