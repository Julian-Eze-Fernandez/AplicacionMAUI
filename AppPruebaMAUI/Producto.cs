using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPruebaMAUI
{
    public class Producto
    { 
		public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
		public string ImageFilePath { get; set; } // Usar la ruta del archivo

	}
}
