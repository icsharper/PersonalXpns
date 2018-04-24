using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MahApps.Metro.Controls;
using System.Data;
/*
* 
* + = Income when using fillbyType=(True) it's in so yes true
* - = spending          fillbyType=(False) no it's spending 
* 
*/
namespace PersonalXpns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsItIncome = true;
        persxpsdbDataSet persxpsdbDataSet;
        persxpsdbDataSetTableAdapters.TableOneMTableAdapter persxpsdbDataSetTableOneMTableAdapter;

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            persxpsdbDataSet = ((persxpsdbDataSet)(FindResource("persxpsdbDataSet")));
            // Load data into the table TableOneM. You can modify this code as needed.
            persxpsdbDataSetTableOneMTableAdapter = new persxpsdbDataSetTableAdapters.TableOneMTableAdapter();
            persxpsdbDataSetTableOneMTableAdapter.Fill(persxpsdbDataSet.TableOneM);
            CollectionViewSource tableOneMViewSource = ((CollectionViewSource)(FindResource("tableOneMViewSource")));
            tableOneMViewSource.View.MoveCurrentToFirst();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            InsertEntry();
        }

        #region personalXpnsHelpers
        private void Refreshtbl()
        {
            try {
                persxpsdbDataSetTableOneMTableAdapter.Fill(persxpsdbDataSet.TableOneM);
            }
            catch { }
        }
        private long GetSelectedRow()
        {
            long selectedRowID = 0;
            // from datagrid 
            //https://stackoverflow.com/questions/5121186/datagrid-get-selected-rows-column-values
            DataRowView rowview = tableOneMDataGrid.SelectedItem as DataRowView;
            selectedRowID = Convert.ToInt16(rowview.Row[0]);
            return selectedRowID;
        }
        private bool ConvertSelectionToBIT() {
            if (listboxINOUT.SelectedIndex == 0)
            {
               return IsItIncome = true;
            }
            else {
                return IsItIncome = false;
            }
        }
        private double ConvertTextToDouble(string textBoxToWork)
        {
            double ValueDouble = 0;
            bool justdoit = Double.TryParse(textBoxToWork, out ValueDouble);
            return ValueDouble;
        }
        private void InsertEntry() {
            // do your work then refresh
            if (txtboxDescription.Text == "" || txtboxValue.Text == "")
            {
                MessageBox.Show("Can't be empty!");
                return;
            }
            try
            {
                persxpsdbDataSetTableOneMTableAdapter.Insert(txtboxDescription.Text, ConvertTextToDouble(txtboxValue.Text), ConvertSelectionToBIT(), DateTime.Now.ToShortDateString());
            }
            catch { }
            Refreshtbl();
        }
        private void DeleteEntry()
        {
            // do your work then refresh
            try
            {
                persxpsdbDataSetTableOneMTableAdapter.Delete(GetSelectedRow(), txtboxDescription.Text, ConvertTextToDouble(txtboxValue.Text), IsItIncome, DateTime.Now.ToShortDateString());
            }
            catch { }
            Refreshtbl();
        }
        private void EditEntry() {
            // move it to another window or dialog
            // do your work then refresh
            //try
            //{
            //    persxpsdbDataSetTableOneMTableAdapter.Update(txtboxDescription.Text, ConvertTextToDouble(txtboxValue.Text), IsItIncome, DateTime.Now);
            //}
            //catch { }
            
            Refreshtbl();
        }
        /// <summary>
        /// if 0 = will show only income
        /// if 1 = will show only spent
        /// if 9 = will show all
        /// </summary>
        /// <param name="selectedValue"></param>
        private void FilterByType(int selectedValue) {
            switch (selectedValue) {
                case 0:
                    persxpsdbDataSetTableOneMTableAdapter.FillByType(persxpsdbDataSet.TableOneM, true);
                    break;
                case 1:
                    persxpsdbDataSetTableOneMTableAdapter.FillByType(persxpsdbDataSet.TableOneM, false);
                    break;
                case 9:
                    Refreshtbl();
                    break;
                default:
                    Refreshtbl();
                    break;
            }
        }
        #endregion

        private void listboxINOUT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertSelectionToBIT();
            try {
                FilterByType(listboxINOUT.SelectedIndex);
            }
            catch { }
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    DataRowView rowview = tableOneMDataGrid.SelectedItem as DataRowView;
            //    btnDelete.Content += rowview.Row[0].ToString();
            //    btnEdit.Content += rowview.Row[0].ToString();
            //}
            //catch { }
        }

        private void btnChartViewer_Click(object sender, RoutedEventArgs e)
        {            
            var incomingOf = persxpsdbDataSetTableOneMTableAdapter.MonthReport(true, "2018-04");
            var OutgoOf = persxpsdbDataSetTableOneMTableAdapter.MonthReport(false, "2018-04");
            ShowMonthReport showreport = new ShowMonthReport((double)incomingOf, (double)OutgoOf, "Viewing of: 2018 April");
            showreport.Show();
            //MessageBox.Show(incomingOf.ToString());
        }

        private void checkboxviewAll_Checked(object sender, RoutedEventArgs e)
        {
            FilterByType(9);
        }

        private void checkboxviewAll_Unchecked(object sender, RoutedEventArgs e)
        {
            FilterByType(0);
        }
    }
}
