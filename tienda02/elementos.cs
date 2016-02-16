using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tienda02
{
    public class elementos
    {
        public string codigo { set; get; }
        public string nombre { set; get; }
        public string imagen { set; get; }
        public string precio { set; get; }

        public elementos()
        {
            this.codigo = "";
            this.nombre = "";
            this.imagen = "";
            this.precio = "";
        }

        public elementos(string codigo, string nombre, string imagen, string precio)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.imagen = imagen;
            this.precio = precio;

        }

        public IEnumerable<elementos> getElementoId(string codigo, List<elementos> elementosJson)
        {
            var elementoSeleccionado = from a in elementosJson where a.codigo == codigo select a;
           // var b = 2;

            return elementoSeleccionado;
        }
    }
}
