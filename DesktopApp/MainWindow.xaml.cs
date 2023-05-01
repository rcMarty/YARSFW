using DataLayer.DatabaseEntites;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
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
using DataLayer;
using DataLayer.DatabaseEntites;
using System.CodeDom;
using System.Diagnostics;
using System.Threading;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Type> tabletypes = new List<Type>() { typeof(User), typeof(Reservation), typeof(Place) };
        public Dictionary<Type, ObservableCollection<object>> tableDataAllTables;

        private (bool, Type) changedTable = (false, null);

        public void GetTables()
        {
            tableDataAllTables = new Dictionary<Type, ObservableCollection<object>>();
            foreach (Type tabletype in tabletypes)
            {
                //MethodInfo genericmethod = typeof(DBConnection).GetMethods().Where(m => m.Name == "Select" && m.GetParameters().Length == 1).First().MakeGenericMethod(property.PropertyType);
                //object? foreignkey = genericmethod.Invoke(null, new object[] { value });

                MethodInfo genericmethod = typeof(MainWindow).GetMethod("SelectAllFromTable").MakeGenericMethod(tabletype);
                var objects = ((IEnumerable)genericmethod.Invoke(this, null)).Cast<object>().ToList();

                tableDataAllTables.Add(tabletype, new ObservableCollection<object>(objects));

            }

        }

        public List<T> SelectAllFromTable<T>()
        {
            List<T> result = DBConnection.Select<T>(new Dictionary<string, string>());
            return result;
        }

        private void printTableData()
        {
            foreach (KeyValuePair<Type, ObservableCollection<object>> table in tableDataAllTables)
            {
                Trace.WriteLine(table.Key);
                foreach (object row in table.Value)
                {
                    Trace.WriteLine(row);
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            //TableWiewFrame.Content = new UserDataView();
            CBSelectTables.ItemsSource = tabletypes;
            DGTableView.CellEditEnding += DGTableView_CellEditEnding;

            GetTables();
        }

        private void DGTableView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            var yourClassInstance = e.EditingElement.DataContext;
            if (IsWhatever.IsAttribute<DBForeignKeyAttribute>(yourClassInstance.GetType().GetProperties(), out List<PropertyInfo> foreignKeyAttribute))
            {
                return;
            }


            Type typeClass = yourClassInstance.GetType();
            changedTable = (true, typeClass);
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;
            Trace.WriteLine(newValue);
            Trace.WriteLine(typeClass);
            Trace.WriteLine(tableDataAllTables[typeClass].Count);
            Trace.WriteLine(foreignKeyAttribute);

        }

        private void CBSelectTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //printTableData();
            Type selectedtype = (Type)CBSelectTables.SelectedItem;
            DGTableView.ItemsSource = tableDataAllTables[selectedtype];

            //delete row from DGTableView where is primary key 
            DGTableView.Columns[0].IsReadOnly = true;
            DGTableView.Columns[0].Header = "ID";


            BEditReservation.Visibility = Visibility.Hidden;
            BNewReservation.Visibility = Visibility.Hidden;
            DGTableView.IsReadOnly = false;

            if (IsWhatever.IsAttribute<DBForeignKeyAttribute>(selectedtype.GetProperties(), out List<PropertyInfo> foreignKeyAttribute))
            {
                DGTableView.IsReadOnly = true;
                BNewReservation.Visibility = Visibility.Visible;
                BEditReservation.Visibility = Visibility.Visible;
            }

            UpdateTable();

        }

        private void UpdateTable()
        {
            if (changedTable.Item1)
            {
                //yes no box
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //update mby all items in table
                    //and after update mby select?

                    foreach (object row in tableDataAllTables[changedTable.Item2])
                    {
                        Trace.WriteLine("UPDATE:UPDATE:UPDATE:UPDATE:UPDATE:UPDATE:UPDATE: ");
                        Trace.WriteLine(row);
                        DBConnection.InsertOrUpdate(row);
                    }
                    changedTable = (false, null);
                    GetTables();
                }
                else
                {
                    //revert changes
                    GetTables();
                    DGTableView.ItemsSource = tableDataAllTables[(Type)CBSelectTables.SelectedItem];
                    changedTable = (false, null);

                }

            }
        }

        private void BDelete_Click(object sender, RoutedEventArgs e)
        {

            if (CBSelectTables.SelectedItem == null || DGTableView.SelectedItem == null)
            {
                MessageBox.Show("Select table or Item");
                return;
            }


            Type selectedType = (Type)CBSelectTables.SelectedItem;
            Trace.WriteLine(DGTableView.SelectedItem);
            DBConnection.Delete(DGTableView.SelectedItem);
            GetTables();

        }

        private void BNewReservation_Click(object sender, RoutedEventArgs e)
        {
            

            Type selectedType = (Type)CBSelectTables.SelectedItem;
            if (selectedType == typeof(Reservation))
            {
                NewReservation reservation = new NewReservation(tableDataAllTables);
                reservation.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select reservation table");
            }
        }

        private void BEditReservation_Click(object sender, RoutedEventArgs e)
        {
            Type selectedType = (Type)CBSelectTables.SelectedItem;
            if (selectedType == typeof(Reservation) && DGTableView.SelectedItem != null)
            {
                NewReservation reservation = new NewReservation(tableDataAllTables, (Reservation)DGTableView.SelectedItem);
                reservation.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select reservation");
            }
        }



        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
            if (!changedTable.Item1)
            {
                GetTables();
                DGTableView.ItemsSource = tableDataAllTables[(Type)CBSelectTables.SelectedItem ?? typeof(User)];
            }
        }
    }
}