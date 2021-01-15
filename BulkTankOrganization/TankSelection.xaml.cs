using BulkTankOrganization.models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BulkTankOrganization
{
    /// <summary>
    /// Interaction logic for TankSelection.xaml
    /// </summary>
    public partial class TankSelection : Page
    {
        /// <summary>
        ///     Holds the value of the user that is logged in
        /// </summary>
        private string loggedInUser = Environment.UserName;

        /// <summary>
        ///     List of Customers that are found in the initializing phase used to popualte the combobox
        /// </summary>
        private List<Customer> customers = new List<Customer>();

        /// <summary>
        ///     List of units belonging to a customer that is used to populate the unit combobox
        /// </summary>
        private List<Unit> units = new List<Unit>();

        /// <summary>
        ///     Holds the value for the logger to log errors and Large Events
        /// </summary>
        private ILogger _log;

        /// <summary>
        ///     Inside the constructor, the log is initialized 
        ///     as well as the component. the Customer combobox is 
        ///     also populated within the try/catch statement
        /// </summary>
        public TankSelection()
        {
            string firstPart = @"C:\Users\";
            string secondPart = @"\Cryogenic Industrial Solutions LLC\Operations - Bulk Tanks";

            _log = new LoggerConfiguration()
                .WriteTo.File($"C:\\Users\\{loggedInUser}\\Cryogenic Industrial Solutions LLC\\Operations - Bulk Tanks\\Logs\\Log.txt", rollingInterval: RollingInterval.Month, shared: true)
                .CreateLogger();

            InitializeComponent();

            try
            {
                // $"C:\\Users\\{loggedInUser}\\Cryogenic Industrial Solutions LLC\\Operations - Bulk Tanks"
                AddItemToCustomerCboList($"{firstPart}{loggedInUser}{secondPart}");
                _log.Information($"{Environment.UserName} established a connection to Teams");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Sync Microsoft Teams folder to your local machine for this program to work.", "Missing Folder Structure", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{Environment.UserName} COULD NOT CONNECT TO TEAMS", ex);
            }
        }

        /// <summary>
        ///     Has the standard path for the first companies. Beyond this, 
        ///     once a company is created, it will follow the same pattern and 
        ///     still be able to find the proper folder. 
        /// </summary>
        /// <param name="path">contains the basic path to get into the MS Teams folder structure</param>
        /// <param name="company">Contains the proper name of the company that will be used within the path.</param>
        /// <returns>String that contains the full proper path for that company to be located at.</returns>
        /// <remarks>MUST USE METHOD CompanyVerify FIRST!</remarks>
        private string CompanyPath(string path, string company)
        {
            switch (company.ToUpper())
            {
                case "AIRGAS":
                    return $"{path}\\AirGas\\Bulk Tanks";
                case "AG":
                    return $"{path}\\AirGas\\Bulk Tanks";
                case "AWG":
                    return $"{path}\\AWG";
                case "LEHIGH":
                    return $"{path}\\Lehigh";
                default:

                    if (Directory.Exists($"{path}\\{company}"))
                    {
                        Directory.CreateDirectory($"{path}\\{company}");
                    }
                    return $"{path}\\{company}";
            }
        }

        /// <summary>
        ///     Creates a Unit class for each of the customers aside from Airgas. Adds to list once complete
        /// </summary>
        /// <param name="folder">Holds the file path of the unit</param>
        /// <param name="cust">Holds the value of what customer the Unit belongs to</param>
        private void LessThan7UnitData(string folder, Customer cust)
        {
            try
            {
                if (folder.Split("\\")[6] != "Closed")
                {
                    Unit tempUnit = new Unit(folder, folder.Split("-")[3]);
                    tempUnit.Gallons = folder.Split("-")[2];
                    tempUnit.SetCustNo(folder.Split("-")[5]);
                    tempUnit.SetNBNumber(folder.Split("-")[4]);
                    tempUnit.Customer = cust;
                    tempUnit.SetStatus(folder.Split("-")[7]);
                    tempUnit.SetService(folder.Split("-")[6]);

                    cust.AddUnitToList(tempUnit);
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        ///     Creates a Unit class that gets assigned the tank data. Adds to list once complete
        /// </summary>
        /// <param name="folder">Holds the file path of the unit</param>
        /// <param name="cust">Holds the value of what customer the Unit belongs to</param>
        /// <remarks>ONLY FOR AIRGAS</remarks>
        private void MoreThan7UnitData(string folder, Customer cust)
        {
            Unit tempUnit = new Unit(folder, folder.Split("-")[3]);
            tempUnit.Gallons = folder.Split("-")[2];
            tempUnit.SetCustNo(folder.Split("-")[5]);
            tempUnit.SetNBNumber(folder.Split("-")[4]);
            tempUnit.Customer = cust;
            tempUnit.SetService(folder.Split("-")[6]);
            tempUnit.SetStatus(folder.Split("-")[7]);

            cust.AddUnitToList(tempUnit);
        }

        /// <summary>
        ///     Takes the path dedicated to the customer 
        ///     and finds the size of the string. Depending on 
        ///     the size depends on what method gets called. 
        ///     Different methods were created due to the differing 
        ///     size between the airgas and all other customers. 
        ///     When control is returned, method sorts list via 
        ///     unit.gallons and populates the list for the 
        ///     combobox item source
        /// </summary>
        /// <param name="cust">takes in the customer object and utilizes the path. This method also passes the customer along to the method called within.</param>
        private void FolderCycle(Customer cust)
        {
            string[] directory = Directory.GetDirectories(cust.GetPath());

            if (directory.Length != 0)
            {
                foreach (string folder in directory)
                {
                    if (folder.Split("\\").Length <= 7)
                    {
                        LessThan7UnitData(folder, cust);
                    }
                    else if (folder.Split("\\").Length >= 8)
                    {
                        if (folder.Split("\\")[7] != "Closed")
                        {
                            //
                            // Airgas should be the only customer travelling down this path
                            // as the folder structure is different than the others
                            //
                            MoreThan7UnitData(folder, cust);
                        }
                    }
                }

                cust.GetUnitList().Sort(delegate (Unit x, Unit y)
                {
                    return x.Gallons.CompareTo(y.Gallons);
                });

                cboSN.ItemsSource = cust.GetUnitList();

                cust.GetUnitList().ForEach(unit => units.Add(unit));
            }
        }

        /// <summary>
        ///     Called on in the constructor, this method populates the Customer combobox with the customers early in the program.
        /// </summary>
        /// <param name="path">holds the value of the base path to get into the MS Teams structure</param>
        private void AddItemToCustomerCboList(string path)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (string folder in directories)
            {
                if (folder.Split("\\")[5] != "TEST")
                {
                    if (folder.Split("\\")[5] != "Logs")
                    {
                        Customer tempCust = new Customer(folder.Split("\\")[5], CompanyPath(path, folder.Split("\\")[5]));
                        customers.Add(tempCust);
                    }
                }
            }

            cboCustomer.ItemsSource = customers;
        }

        /// <summary>
        ///     After customer selection, method passes selected customer to the FolderCycle method
        /// </summary>
        private void UpdateBoxesWithCustomerInfo(object sender, SelectionChangedEventArgs e)
        {
            Customer searchedCust = customers.Find(x => x == cboCustomer.SelectedItem);

            try
            {
                FolderCycle(searchedCust);
                _log.Information($"{Environment.UserName} SELECTED {searchedCust.Name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error:\n\n {ex.Message}");
                _log.Error($"{Environment.UserName} had an ERROR occur while selecting a customer", ex);
            }
        }

        /// <summary>
        ///     Takes value inside of the combobox that 
        ///     carries the unit information and grabs 
        ///     all the related data. Once found, method 
        ///     navigates to the 'Home' page
        /// </summary>
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            // Searching for unit within the list of units
            Unit searchedUnit = units.Find(unit => unit == cboSN.SelectedItem);

            try
            {
                UnitFolderCycle(searchedUnit);

                // Opens up new window to perform actions on the unit's files
                BulkTankHome bulkTankHome = new BulkTankHome(searchedUnit, _log);
                this.NavigationService.Navigate(bulkTankHome);
                _log.Information($"{loggedInUser} opened {searchedUnit} @ {DateTime.Now}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error that occured:\n\n {ex.Message}");
                _log.Error($"\n\n{loggedInUser} attempted to open unit {searchedUnit} under {searchedUnit.Customer.Name} @ {DateTime.Now}\n\n", ex.Message);
            }
        }
 
        /// <summary>
        ///     Retrieves all directories (folders) within 
        ///     the unit folder itself and calls the unit's 
        ///     method to add it to the unit's list of folders
        /// </summary>
        /// <param name="unit">Holds all the data specific to the unit itself.</param>
        private void UnitFolderCycle(Unit unit)
        {
            string[] directories = Directory.GetDirectories(unit.Path);

            foreach (string folder in directories)
            {
                try
                {
                    if (!unit.unitFolders.ContainsKey(folder.Split("\\")[7]))
                    {
                        unit.unitFolders.Add(folder.Split("\\")[7], new UnitFile(folder.Split("\\")[7], folder));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error with {unit.GetSerialNumber()}\n\n error: {ex.Message}");
                    continue;
                }
            }
        }

        /// <summary>
        ///     Navigates to the window for creating a new unit
        /// </summary>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateScreen createScreen = new CreateScreen(_log);
            this.NavigationService.Navigate(createScreen);
        }
    }
}
