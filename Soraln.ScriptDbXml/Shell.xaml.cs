using MahApps.Metro.Controls;
using Soraln.Framework.Mvvm.DTO;
using Soraln.Framework.Mvvm.Helper;
using Soraln.Framework.Mvvm.View;
using Soraln.ScriptDbXml.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soraln.ScriptDbXml
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }
        #region " Eventos "


        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsOpen = true;
            border.Visibility = Visibility.Visible;

        }
        #endregion

        private void Menu_ClosingFinished(object sender, RoutedEventArgs e)
        {
            border.Visibility = Visibility.Collapsed;


        }

        

        #region " Métodos privados "
        /// <summary>
        /// Obtiene el nombre de la vista
        /// </summary>
        /// <param name="pNombreVista">Nombre de la vista </param>
        /// <returns></returns>
        public string ObtieneFullNameVista(string pNombreVista)
        {
            string vFullName = string.Empty;
            if (!string.IsNullOrEmpty(pNombreVista))
            {
                string v = Type.GetType(pNombreVista).FullName;
                vFullName = string.Format("{0},{1}", Type.GetType(pNombreVista).FullName, Type.GetType(pNombreVista).Assembly.FullName);
            }
            return vFullName;

        }
        #endregion

        #region " Eventos "
        private void menuGeneradorScript_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserControl vObjVista = null;
            RecursoDTO vObjRecursoDTO = new RecursoDTO()
            {
                 Ejecutable = "Soraln.ScriptDbXml.Views.GeneradorScriptBdView"
            };
            if (vObjRecursoDTO != null)
            {
                string vNombre = ObtieneFullNameVista(vObjRecursoDTO.Ejecutable);
                vObjVista = ViewFactory.CrearComponenteVisual(vNombre);

                InfoVista vObjInfoVista = new InfoVista();
                vObjInfoVista.Vista = vObjVista;
                vObjInfoVista.Vista = VistaHelper.CreaVista(vObjInfoVista);

                if (vObjVista != null)
                {
                    vObjVista.VerticalAlignment = VerticalAlignment.Stretch;
                    vObjVista.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ContenedorVista.Children.Add(vObjVista);
                    Menu.IsOpen = false;
                }
            }
        }
        #endregion
    }
}
