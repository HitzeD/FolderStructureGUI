using GUIBulkNaming.Models;
using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BulkTankOrganization
{
    /// <summary>
    /// Interaction logic for CreateScreen.xaml
    /// </summary>
    public partial class CreateScreen : Page
    {
        private ILogger _log;
        private string user = Environment.UserName;


        public CreateScreen(ILogger log)
        {
            InitializeComponent();
            _log = log;
        }

        /// <summary>
        ///     uses all boxes within the window to gather data and create a folder for the new unit being submitted.
        /// </summary>
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string userName = Environment.UserName;
            string targetFile = $"C:\\Users\\{userName}\\Cryogenic Industrial Solutions LLC\\Operations - Bulk Tanks\\";

            string fileName;

            var unit = new UnitBuilder().withCompany(CompanyVerify(txtCustomer.Text)).withCustNo(txtCustNo.Text)
                .withSerial(txtNumber.Text).withSize(txtSize.Text).withNationalNo(txtNBNo.Text)
                .withService(ServiceCleaner(cboService.Text)).withStatus(StatusCleaner(cboStatus.Text))
                .build();

            try
            {
                if (check_Boxes())
                {
                    fileName = $"{unit.Company}-{unit.TankSize}-SN_{unit.SerialNumber}-NB_{unit.NationalNumber}-CustNo_{unit.CustomerNumber}-{unit.TankService}-{unit.TankStatus}-LI";
                    targetFile = $"{CompanyPath(targetFile, unit.Company)}\\{fileName}";

                    System.IO.Directory.CreateDirectory($"{targetFile}\\Certificates");
                    System.IO.Directory.CreateDirectory($"{targetFile}\\Quotes");
                    System.IO.Directory.CreateDirectory($"{targetFile}\\Photos\\Strainer");
                    System.IO.Directory.CreateDirectory($"{targetFile}\\Quality Documentation Package");

                    CommentsToTextFile(txtComments.Text, $"{targetFile}\\Comments.txt");
                    System.IO.File.SetAttributes($"{targetFile}\\Comments.txt", FileAttributes.ReadOnly);

                    ClearFields();
                    MessageBox.Show("Folder has been created!", "Completed");


                }

                _log.Information($"{user} CREATED FOLDER for {unit.Company} with a serial number of: {unit}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error creating the folder", "Folder Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} FAILED CREATING folder for {unit.Company}: {unit}", ex);
            }

        }

        /// <summary>
        ///     Verifies that all textboxes have been filled out
        /// </summary>
        /// <returns>Boolean. Returns false if any of the textboxes is empty else it returns true</returns>
        private bool check_Boxes()
        {
            if (txtCustomer.Text == "")
            {
                MessageBox.Show("Please Input Customer Name", "No Customer");
                return false;
            }
            else if (txtSize.Text == "")
            {
                MessageBox.Show("Please Input Tank Size", "No Tank Size");
                return false;
            }
            else if (txtNumber.Text == "" && txtCustNo.Text == "" && txtNBNo.Text == "")
            {
                MessageBox.Show("Please Input an Identifying Number for this Unit!", "No Identifier");
                return false;
            }
            else if (cboService.SelectedItem == null)
            {
                MessageBox.Show("Please Select Industrial or Medical in Service", "No Service");
                return false;
            }
            else if (cboStatus.SelectedItem == null)
            {
                MessageBox.Show("Please Select Rehab or Refurb in Repair Status", "No Status");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///     Changes the selection from the full word to abbreviated word
        /// </summary>
        /// <param name="selection">Holds the value of the string to be 'cleaned' by the method.</param>
        /// <returns>string value of the word abbreviated</returns>
        /// <remarks>ex: Industrial -> Ind</remarks>
        private string ServiceCleaner(string selection)
        {
            switch (selection)
            {
                case "Industrial":
                    return "Ind";
                case "Medical":
                    return "Med";
                case "Laser":
                    return "Lzr";
                default:
                    return "this should never show";
            }
        }

        /// <summary>
        ///     Changes the user selection into abbreviated words.
        /// </summary>
        /// <param name="selection">Holds the value of the word that needs to be 'cleaned'</param>
        /// <returns>string value that contains the abbreviated word</returns>
        /// <remarks>ex: Rehabilitation -> RH</remarks>
        private string StatusCleaner(string selection)
        {
            switch (selection)
            {
                case "Rehab":
                    return "RH";
                case "Refurb":
                    return "RF";
                default:
                    return "this should never show";
            }
        }
   
        /// <summary>
        ///     Checks for common ways of spelling the customer and corrects it to what is proper
        /// </summary>
        /// <param name="input">Contains the value of the user inputted customer name</param>
        /// <returns>String that contains the corrected company name. If the company was not matched, it uppercases the name and uses that for the folder.</returns>
        private string CompanyVerify(string input)
        {
            switch (input.ToLower())
            {
                case "airgas":
                    return "AG";
                case "ag":
                    return "AG";
                case "awg":
                    return "AWG";
                case "american welding and gas":
                    return "AWG";
                case "messer":
                    return "Messer";
                case "lehigh":
                    return input.ToUpper();
                default:
                    return input.ToUpper();
            }
        }

        /// <summary>
        ///     Removes all text that the user has inputted into the window
        /// </summary>
        private void ClearFields()
        {
            txtCustomer.Text = "";
            txtNumber.Text = "";
            txtSize.Text = "";
            cboService.SelectedItem = null;
            cboStatus.SelectedItem = null;
            txtComments.Text = "";
            txtCustNo.Text = "";
            txtNBNo.Text = "";
        }

        /// <summary>
        ///     Has the standard path for the first companies. Beyond this, once a company is created, it will follow the same pattern and still be able to find the proper folder. 
        /// </summary>
        /// <param name="path">contains the basic path to get into the MS Teams folder structure</param>
        /// <param name="company">Contains the proper name of the company that will be used within the path.</param>
        /// <returns>String that contains the full proper path for that company to be located at.</returns>
        /// <remarks>MUST USE METHOD CompanyVerify FIRST!</remarks>
        private string CompanyPath(string path, string company)
        {
            switch (company)
            {
                case "AG":
                    return $"{path}\\AirGas\\Bulk Tanks";
                case "AWG":
                    return $"{path}\\AWG";
                case "LEHIGH":
                    return $"{path}\\Lehigh";
                default:
                    return $"{path}\\{company}";
            }
        }

        /// <summary>
        ///     Takes the comments filled out into txtComments and writes them to the specified file
        /// </summary>
        /// <param name="comments">text written by the user in the textbox that is to be written and saved to the file.</param>
        /// <param name="path">contains the location of the file to be written to</param>
        private void CommentsToTextFile(string comments, string path)
        {
            if (txtComments.Text == "")
            {
                MessageBox.Show("Please input comments into the textbox!", "Missing Comments", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                comments += $"{Environment.NewLine} Commented by: {Environment.UserName} @ {DateTime.Now}\n";
                File.AppendAllText(path, comments);
                _log.Information($"{user} ADDED COMMENTS to {path}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an issue posting the comment.\n\n Please try again later!", "Comment Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} failed writing comments to {path}. ERROR: {ex.Message}");
            }
        }
    }
}
