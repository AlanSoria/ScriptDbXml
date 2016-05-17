using Soraln.Framework.Mvvm.ViewModel;
using Soraln.ScriptDbXml.DTO;
using Soraln.ScriptDbXml.Enumerador;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;

namespace Soraln.ScriptDbXml.ViewModel
{
    public class GeneradorScriptBdViewModel : ViewModelBase
    {
        #region " Campos de clase "
        private ObservableCollection<string> vColTablas = null;
        #endregion

        public GeneradorScriptBdViewModel()
        {
            vColTablas = ObtieneTablas();
        }


        #region " Métodos internal "
        internal FlowDocument RecuperaDocumentosXml(List<string> pColRutaArchivoXml)
        {
            FlowDocument vObjFlowDocument = new FlowDocument();
            XmlDocument vObjXmlDocument = new XmlDocument();
            // Archivo XML generado
            XmlDocument vRootXml = new XmlDocument();
            XmlNode vRootNode = vRootXml.CreateElement("bloqueAnonimo");
            vRootXml.AppendChild(vRootNode);
            XmlNode vNodoHijo = null;

            if (pColRutaArchivoXml != null
                && pColRutaArchivoXml.Count > 0)
            {
                pColRutaArchivoXml.ForEach(vPath =>
                {
                    vObjXmlDocument.Load(vPath);
                    if (vObjXmlDocument != null)
                    {
                        foreach (XmlNode vNode in vObjXmlDocument.FirstChild)
                        {
                            vNodoHijo = vRootXml.CreateElement(vNode.Name);
                            vNodoHijo.InnerText = vNode.InnerText;
                            vRootNode.AppendChild(vNodoHijo);
                        }
                    }

                });
            }
            vObjFlowDocument = LeeArchivo(vRootXml);
            return vObjFlowDocument;
        }
        #endregion
        #region " Métodos privados "
        public FlowDocument LeeArchivo(XmlDocument pXmlDocument)
        {

            FlowDocument vObjFlowDocument = new FlowDocument();
            ///
            ScriptDTO vObjScriptDTO = null;
            List<ScriptDTO> vColScriptDTO = new List<ScriptDTO>();


            XmlDocument vObjXmlDocument = new XmlDocument();
            vObjXmlDocument = pXmlDocument;
            //vObjXmlDocument.Load(@"G:\Alan\Utilitario (VS)\Generador de script de Deploy\ScriptTest.xml");
            if (vObjXmlDocument != null)
            {
                XmlNodeList vNodoBloqueAnonimo = vObjXmlDocument.GetElementsByTagName("bloqueAnonimo");
                if (vNodoBloqueAnonimo != null)
                {
                    XmlNodeList vNodoTabla;
                    XmlNodeList vNodoComentario;
                    XmlNodeList vNodoScript;

                    foreach (XmlElement vNodo in vNodoBloqueAnonimo)
                    {
                        vNodoTabla = vNodo.GetElementsByTagName("tabla");
                        vNodoScript = vNodo.GetElementsByTagName("script");
                        vNodoComentario = vNodo.GetElementsByTagName("comentario");

                        for (int vPosicion = 0; vPosicion < vNodoTabla.Count; vPosicion++)
                        {
                            vObjScriptDTO = new ScriptDTO();
                            vObjScriptDTO.Tabla = Regex.Replace(vNodoTabla[vPosicion].InnerText, @"\t|\n|\r", "").Replace(" ", "");
                            vObjScriptDTO.Script = Regex.Replace(vNodoScript[vPosicion].InnerText, @"\t|\n|\r", " ");
                            vObjScriptDTO.Comentario = Regex.Replace(vNodoComentario[vPosicion].InnerText, @"\t|\n|\r", "");
                            vColScriptDTO.Add(vObjScriptDTO);
                        }
                        ObservableCollection<ScriptDTO> vColScript = new ObservableCollection<ScriptDTO>(vColScriptDTO);
                        if (vColScript != null)
                        {
                            if (vColTablas != null)
                            {
                                vColTablas.ToList().ForEach(vTabla =>
                                {
                                    var vScript = vColScript.Where(x => x.Tabla == vTabla);
                                    if (vScript != null
                                        && vScript.ToList().Count > 0)
                                    {
                                        Paragraph vObjParagraph = null;
                                        // Cabecera del Script
                                        vObjParagraph = new Paragraph();
                                        vObjParagraph = ConstruyeParrafoScript(vTabla, null, eComponenteScript.Cabecera);
                                        vScript.ToList().ToList().ForEach(vScriptDto =>
                                        {
                                            vObjFlowDocument.Blocks.Add(vObjParagraph);
                                            // Comentario del script
                                            vObjParagraph = new Paragraph();
                                            vObjParagraph = ConstruyeParrafoScript(null, vScriptDto, eComponenteScript.Comentario);
                                            vObjFlowDocument.Blocks.Add(vObjParagraph);
                                            // Cuerpo del script
                                            vObjParagraph = new Paragraph();
                                            vObjParagraph = ConstruyeParrafoScript(null, vScriptDto, eComponenteScript.Script);
                                            vObjFlowDocument.Blocks.Add(vObjParagraph);

                                        });
                                    }
                                });
                            }
                        }
                    }
                }

            }
            return vObjFlowDocument;
        }
        private Paragraph ConstruyeParrafoScript(string pNombreTabla, ScriptDTO pScriptDTO, eComponenteScript pComponenteScript)
        {
            Paragraph vObjParagraph = null;

            switch (pComponenteScript)
            {
                case eComponenteScript.Cabecera:
                    string vCabecera = string.Empty;
                    vCabecera = string.Format("/******************************************* {0} *******************************************/ {1}"
                                                                                                , pNombreTabla, "");
                    vObjParagraph = new Paragraph(new Run(vCabecera));
                    vObjParagraph.FontStyle = FontStyles.Italic;
                    vObjParagraph.Foreground = Brushes.OrangeRed;
                    break;
                case eComponenteScript.Comentario:
                    string vComentario = string.Empty;
                    vComentario = string.Format("{0} {1}", pScriptDTO.Comentario, "");
                    vObjParagraph = new Paragraph(new Run(vComentario));
                    vObjParagraph.FontStyle = FontStyles.Italic;
                    vObjParagraph.Foreground = Brushes.Green;
                    break;
                case eComponenteScript.Script:
                    string vCuerpoScript = string.Empty;
                    vCuerpoScript = pScriptDTO.Script;
                    vObjParagraph = new Paragraph(new Run(vCuerpoScript));
                    vObjParagraph.Foreground = Brushes.White;
                    break;
                default:
                    break;
            }
            return vObjParagraph;
        }

        public ObservableCollection<string> ObtieneTablas()
        {
            List<string> vColTabla = new List<string>();
            XmlDocument vObjXmlDocument = new XmlDocument();
            XmlNodeList vNodoTabla;
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Origen de datos\Tablas.xml");
            vObjXmlDocument.Load(path);
            if (vObjXmlDocument != null)
            {
                XmlNodeList vNodoBaseDeDatos = vObjXmlDocument.GetElementsByTagName("BaseDeDatos");
                if (vNodoBaseDeDatos != null)
                {
                    foreach (XmlElement vNodo in vNodoBaseDeDatos)
                    {
                        vNodoTabla = vNodo.GetElementsByTagName("Tabla");
                        if (vNodoTabla != null)
                        {
                            for (int vIndice = 0; vIndice < vNodoTabla.Count; vIndice++)
                            {
                                if (vNodoTabla[vIndice].InnerText != null)
                                    vColTabla.Add(vNodoTabla[vIndice].InnerText);
                            }
                            if (vColTabla != null
                                && vColTabla.Count > 0)
                                vColTablas = new ObservableCollection<string>(vColTabla);
                        }
                    }
                }
            }
            return vColTablas;


        }
        #endregion
    }
}
