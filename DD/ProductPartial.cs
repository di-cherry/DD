using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD
{
    public partial class Products
    {
        public bool IsDiscount
        {
            get
            {
                return Discount > 10;
            }
        }
    }
}
