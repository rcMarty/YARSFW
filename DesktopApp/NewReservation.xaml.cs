using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.Toolkit;
using DataLayer;
using DataLayer.DatabaseEntites;

namespace DesktopApp
{
    /// <summary>
    /// Interakční logika pro NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Window
    {
        private Dictionary<Type, ObservableCollection<object>> tableDataAllTables = null;
        private Reservation reservation = null;
        private bool createNew = false;

        DateTimePicker DTPStartTime;

        private void Datetimepicker()
        {
            
            DTPStartTime = new DateTimePicker();
            DTPStartTime.HorizontalAlignment = HorizontalAlignment.Left;
            DTPStartTime.Margin = new Thickness(88, 210, 0, 0);
            DTPStartTime.VerticalAlignment = VerticalAlignment.Top;
            DTPStartTime.Width = 120;
            DTPStartTime.Visibility = Visibility.Visible;
            MainGrid.Children.Add(DTPStartTime);
        }
        public NewReservation(Dictionary<Type, ObservableCollection<object>> tableDataAllTables)
        {
            //< xctk:DateTimePicker HorizontalAlignment = "Left" Margin = "150,200,0,0" VerticalAlignment = "Top" Width = "120" />
            
            
            InitializeComponent();
            Datetimepicker();
            this.tableDataAllTables = tableDataAllTables;
            this.createNew = true;

            CBUser.ItemsSource = tableDataAllTables[typeof(User)];
            CBPlace.ItemsSource = tableDataAllTables[typeof(Place)];
            

            
        }

        public NewReservation(Dictionary<Type, ObservableCollection<object>> tableDataAllTables, Reservation reservation)
        {
            InitializeComponent();
            Datetimepicker();
            this.reservation = reservation;
            this.tableDataAllTables = tableDataAllTables;
            this.createNew = false;
            CBUser.ItemsSource = tableDataAllTables[typeof(User)];
            CBPlace.ItemsSource = tableDataAllTables[typeof(Place)];
            CBUser.Text = reservation.User.ToString();
            CBPlace.Text = reservation.Place.ToString();
            DTPStartTime.Text = reservation.StartTime.ToString();
            TBDuration.Text = reservation.Duration.ToString();

        }

        private Reservation MakeNewReservation()
        {
            Reservation newReservation = new Reservation();
            newReservation.User = (User)CBUser.SelectedItem;
            newReservation.Place = (Place)CBPlace.SelectedItem;

            newReservation.StartTime = DTPStartTime.Value.Value;

            if (int.TryParse(TBDuration.Text, out int result))
                newReservation.Duration = result;
            else
                System.Windows.MessageBox.Show("duratin too big :pepeevil:");
            
            
            newReservation.OverallPrice = (newReservation.Place.Price * 0.178) * newReservation.Duration;

            return newReservation;
        }
        private void UpdateReservation()
        {
            reservation.User = (User)CBUser.SelectedItem;
            reservation.Place = (Place)CBPlace.SelectedItem;
            reservation.StartTime = DTPStartTime.Value.Value;

            if (int.TryParse(TBDuration.Text, out int result))
                reservation.Duration = result;
            else
                System.Windows.MessageBox.Show("duratin too big :pepeevil:");

            reservation.OverallPrice = (reservation.Place.Price * 0.178) * reservation.Duration;
        }   

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            if(createNew)
            {
                DBConnection.Insert(MakeNewReservation());
                System.Windows.MessageBox.Show("New reservation Created");
                this.Close();
            }
            else
            {
                UpdateReservation();
                DBConnection.Update(reservation);
                System.Windows.MessageBox.Show("Reservation Updated");
                this.Close();
            }
        }
    }
}
