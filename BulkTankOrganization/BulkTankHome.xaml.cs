using BulkTankOrganization.models;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace BulkTankOrganization
{
    /// <summary>
    /// Interaction logic for BulkTankHome.xaml
    /// </summary>
    public partial class BulkTankHome : Page
    {
        private Unit unit;
        private string userName = Environment.UserName;
        private ILogger _log;

        public BulkTankHome(Unit unit, ILogger log)
        {
            InitializeComponent();
            this.unit = unit;
            this._log = log;

            lblTank.Content = $"Serial Number: {this.unit.GetSerialNumber()}";
            lblCust.Content = $"Customer: {this.unit.Customer.Name}";
        }

        /// <summary>
        ///     Passes the unit data and log data to the new window before initializing. Once initialized, the method navigates to the window.
        /// </summary>
        private void btnElse_Click(object sender, RoutedEventArgs e)
        {
            // Take off to another window with alternate options
            try
            {
                UnitOptionsWindow unitOptions = new UnitOptionsWindow(this.unit, _log);
                _log.Information($"{userName} navigated to the the options page of unit {this.unit}.");
                this.NavigationService.Navigate(unitOptions);
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"{userName} FAILED navigating to unit options for {this.unit}.");
            }

        }

        /// <summary>
        ///     Opens the traveller if available, or creates one and saves within the proper location
        /// </summary>
        private void btnTraveler_Click(object sender, RoutedEventArgs e)
        {
            string travelerPath = $"C:\\Users\\{userName}\\Cryogenic Industrial Solutions LLC\\Cryogenic Industrial Solutions - Quality Forms\\CIS-QAF-047 - Bulk Tank Services.xlsm"; // removed 's' from Quality Forms

            if (File.Exists($"{this.unit.Path}\\Traveler.xlsm"))
            {
                OpenDoc($"{this.unit.Path}\\Traveler.xlsm");
                _log.Information($"{userName} OPENED TRAVELER for {this.unit}");
            }
            else
            {
                try
                {
                    File.Copy(travelerPath, $"{unit.Path}\\Traveler.xlsm");

                    MessageBoxResult mbResults = MessageBox.Show($"unit:\n\n\tSN: {this.unit.GetSerialNumber()}\n\tCustomer Number: {this.unit.GetCustNumber()}\n\nHas had a traveler created.", "Traveler Creation", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    if (mbResults == MessageBoxResult.OK)
                    {
                        OpenDoc($"{this.unit.Path}\\Traveler.xlsm");
                        _log.Information($"{userName} CREATED TRAVELER for {this.unit}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error: \n\n {ex.Message}");
                    _log.Error(ex, $"\n\n{userName} ATTEMPTED to CREATE TRAVELER for {this.unit}.\n\n");
                }
            }
        }

        /// <summary>
        ///     If a Quote exists, it is opened.
        ///     
        ///     If many Quotes exist, a dialog box is opened to the location of the quotes to 
        ///     pick one to open. 
        ///     
        ///     If no quotes exist one is created. 
        ///     
        ///     If one already exists, the option to still create a new one 
        ///     is available. Upon creation, data from the unit is passed 
        ///     to Excel via Microsoft.Office.Interop.Excel package.
        /// </summary>
        private void btnQuote_Click(object sender, RoutedEventArgs e)
        {
            string quotePath = $"C:\\Users\\{userName}\\Cryogenic Industrial Solutions LLC\\Operations - Bulk Tanks\\AIRGAS QUOTE FORM.xlsm";
            string nonAirGasQuote = $"C:\\Users\\{userName}\\Cryogenic Industrial Solutions LLC\\Operations - Bulk Tanks\\Standard Bulk Tank Bid (003).xlsm";

            QuoteData data = QuoteFileNaming($"{this.unit.Path}\\Quotes\\QUOTE0.xlsm");

            try
            {
                if (data.GetCount() > 0)
                {
                    var resp = MessageBox.Show($"There is/are {data.GetCount()} quote(s) within this unit.\n\n Would you like to select one to open?", "Quote Selection", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (resp == MessageBoxResult.Yes)
                    {
                        OpenDoc(this.unit.GetUserFile($"{this.unit.Path}\\Quotes"));
                    }
                    else if (resp == MessageBoxResult.No)
                    {
                        var nextResp = MessageBox.Show("Would you like to create a new quote?", "Quote Creation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (nextResp == MessageBoxResult.Yes)
                        {
                            try
                            {
                                File.Copy(this.unit.Customer.Name == "AirGas" ? quotePath : nonAirGasQuote, data.GetPath());
                                PassExcelData(data.GetPath(), data.GetCount());

                                MessageBox.Show("Quote Has been Initialized!", "Quote Creation", MessageBoxButton.OK, MessageBoxImage.Information);
                                OpenDoc(data.GetPath());
                                _log.Information($"QUOTE CREATED for {this.unit} by {userName}");
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        else if (nextResp == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                    else if (resp == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    try
                    {

                        if (this.unit.Customer.Name == "AirGas")
                        {
                            File.Copy(quotePath, data.GetPath());
                            PassExcelData(data.GetPath(), data.GetCount());
                        }
                        else
                        {
                            File.Copy(nonAirGasQuote, data.GetPath());
                            PassExcelData(data.GetPath(), data.GetCount());
                        }

                        MessageBox.Show("Quote Has been Initialized!", "Quote Creation", MessageBoxButton.OK, MessageBoxImage.Information);
                        OpenDoc(data.GetPath());
                        _log.Information($"QUOTE CREATED for {this.unit} by {userName}");
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error: \n\n {ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error(ex, $"{userName} tried CREATING QUOTE for {this.unit}.");
            }
        }

        /// <summary>
        ///     Takes a file location and attempts to open the file
        /// </summary>
        /// <param name="path">The location of which file is to be opened</param>
        private void OpenDoc(string path)
        {
            try
            {
                var process = new Process();

                process.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };

                process.Start();
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"{userName} tried OPENING Document {path}.");
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        ///     Uses Recursion to cycle through the count of Quotes. When finished it returns the Quote's count and Path.
        /// </summary>
        /// <param name="path">The possible path that could be the location. Always starts as 'Quote0'</param>
        /// <param name="count">OPTIONAL. This is used to keep track of the recursion and count. It defaults to 0 and when recursion is used, the new count is passed into the method</param>
        /// <returns>Object of type QuoteData is returned that contains the path assigned and the count it took to get there.</returns>
        private QuoteData QuoteFileNaming(string path, int count = 0)
        {
            if (!File.Exists(path))
            {
                QuoteData tempData = new QuoteData();
                tempData.SetPath(path);
                tempData.SetCount(count);

                return tempData;
            }
            else
            {
                string tempPath = path.Split($"QUOTE{count}")[0];
                count++;
                return QuoteFileNaming($"{tempPath}\\QUOTE{count}.xlsm", count);
            }
        }

        /// <summary>
        ///     Pass Excel data that it uses to populate the headers with critical tank information
        /// </summary>
        /// <param name="path">The location of which the quote is located at that needs data passed to it</param>
        /// <param name="count">The number of quotes that was counted is passed here to be used as a 'Revision' counter</param>
        private void PassExcelData(string path, int count)
        {
            // Initiailizing the Document Variables
            Excel.Application xlApp;
            Excel.Workbook xlWb;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWb = xlApp.Workbooks.Open(path);

            try
            {
                // Running VBA Function to input data from C#
                xlApp.Run("DataFromCSharp", this.unit.GetJobNumber() == "Not Assigned" ? "TBD" : this.unit.GetJobNumber(), this.unit.GetSerialNumber(), this.unit.GetNBNumber(), count, this.unit.Customer.Name, this.unit.Gallons);

                _log.Information($"{userName} Transferred data for {this.unit} in Quote");
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"{userName} FAILED transferring data for {this.unit} to quote.");
            }
            finally
            {
                // Preparing to close/closing the application
                xlWb.Save();
                xlWb.Close(false, misValue, misValue);
                xlApp.Quit();


                // Removing the referenece and object
                if (xlWb != null) { System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWb); }
                if (xlApp != null) { System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp); }
                _log.Information($"Excel Objects have been released");
            }

        }
    }
}
