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
using System.Windows.Shapes;

namespace test2
{
    /// <summary>
    /// Логика взаимодействия для ArchiveWindow.xaml
    /// </summary>
    public partial class ArchiveWindow : Window
    {
        private ArchiveControl ac = new ArchiveControl();
        public ReportWindow1 reportWindow = null;

        public ArchiveWindow()
        {
            InitializeComponent();
            ac.archiveWindow = this;
            ac.Fist_TreeData();
            ac.count();
            Label1.Content = "";
            Label2.Content = "";
            Label3.Content = "";
            Label4.Content = "";
            Label5.Content = "";
            Label6.Content = "";
            Label7.Content = "";
            Label8.Content = "";
            Label9.Content = "";
        }
        //==============================================
        private void trw_Expanded(object sender, RoutedEventArgs e)
        {
            ac.Expander(e);
        }
        //=======================================
        private void TreeView_arc_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                ac.info_router((TreeViewItem)TreeView_arc.SelectedItem);
            }
            catch
            {

            }
        }
        //======================================================
        private void Button_Otchet_Click(object sender, RoutedEventArgs e)
        {
            reportWindow = new ReportWindow1((TreeViewItem)TreeView_arc.SelectedItem);
            reportWindow.Show();
        }
    }
}
