using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soraln.ScriptDbXml.DTO
{
    public class ScriptDTO
    {
        public string Tabla { get; set; }
        public string Script { get; set; }
        public string Comentario { get; set; }
        public int OrdenTabla { get; set; }
    }
}
