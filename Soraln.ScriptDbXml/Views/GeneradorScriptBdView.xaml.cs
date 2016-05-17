using Microsoft.Win32;
using Soraln.Framework.Mvvm;
using Soraln.Framework.Mvvm.ViewModel;
using Soraln.ScriptDbXml.ViewModel;
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

namespace Soraln.ScriptDbXml.Views
{
    /// <summary>
    /// Lógica de interacción para GeneradorScriptBdView.xaml
    /// </summary>
    [AtributoViewModelAsociado(typeof(GeneradorScriptBdViewModel))]
    public partial class GeneradorScriptBdView : UserControl
    {
        public GeneradorScriptBdView()
        {
            InitializeComponent();
        }
        #region " ViewModel "
        private GeneradorScriptBdViewModel _viewModel;
        /// <summary>
        /// Instancia de ViewModel de la vista
        /// </summary>
        private GeneradorScriptBdViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = ViewModelHelper.ViewModel<GeneradorScriptBdViewModel>(this);
                return _viewModel;
            }
        }
        #endregion

        private void btnAbrirArchivo_Click(object sender, RoutedEventArgs e)
        {
            ctrlListaDeArchivos = new ListBox();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                List<string> vPaths = new List<string>();

                foreach (string filename in openFileDialog.FileNames)
                {
                    ListBoxItem vObjListBoxItem = new ListBoxItem();
                    vObjListBoxItem.Content = filename;
                    ctrlListaDeArchivos.Items.Add(vObjListBoxItem);
                    vPaths.Add(filename);
                }
                // 
                if (vPaths != null
                    && vPaths.Count > 0)
                    richTxt.Document = ViewModel.RecuperaDocumentosXml(vPaths);
            }
        }
    }
}
