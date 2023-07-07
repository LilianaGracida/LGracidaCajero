using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Denominacion
    {
        public int IdDenominacion { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }

        public int Billete1000 { get; set; }    
        public int Billete500 { get; set; }    
        public int Billete200 { get; set; }    
        public int Billete100 { get; set; }    
        public int Billete50 { get; set; }    
        public int Billete20 { get; set; }    
        public int Sobrante { get; set; }

        public List<Object> Denominaciones { get; set; }

    }
}
