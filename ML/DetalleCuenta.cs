using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class DetalleCuenta
    {
        public int IdDetalle { get; set; }
        public ML.Cliente Cliente { get; set; }
        public int Saldo { get; set; }
    }
}
