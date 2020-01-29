using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderAPI
{
    public class DbResponse
    {
        public bool Result { get; set; }
        public string ExceptionMessage { get; set; }
        public DataSet DataResult { get; set; }
    }
}
