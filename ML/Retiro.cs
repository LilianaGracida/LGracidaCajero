using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Retiro
    {
        public int IdRetiro { get; set; }
        public ML.Cliente Cliente { get; set; }
        public int Cantidad { get; set; }
        public int SaldoRetenido { get; set; }
        public ML.Denominacion Denominacion { get; set; }
    }
}
