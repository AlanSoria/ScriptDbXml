using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soraln.ScriptDbXml.DTO
{
    public class RecursoDTO
    {
        public long IdRecurso { get; set; }
        public string Etiqueta { get; set; }
        public string Ejecutable { get; set; }
        public int Orden { get; set; }
        public long IdRecursoSuperior { get; set; }
    }
}
