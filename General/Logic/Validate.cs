using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Logic
{
    internal class Validate
    {
        public bool ValidateField(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            else
            {
                return true;
            }
        }
    }
}
