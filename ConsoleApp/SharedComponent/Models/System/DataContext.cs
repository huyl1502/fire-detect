using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Models
{
    public class DataContext
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public object Value { get; set; }
    }
}
