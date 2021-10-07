using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Const
{
    public class Function
    {
        public static string CheckDuplicatePeriodSalary => "SELECT Func_CheckDuplicatePeriodSalary(@Month,@Year,@PeriodSalaryId) as Result";
    }
}
