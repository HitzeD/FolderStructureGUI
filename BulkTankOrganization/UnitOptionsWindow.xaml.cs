using BulkTankOrganization.models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static BulkTankOrganization.models.UnitEnums;

namespace BulkTankOrganization
{
    /// <summary>
    /// Interaction logic for UnitOptionsWindow.xaml
    /// </summary>
    public partial class UnitOptionsWindow : Page
    {
        private Unit unit;
        private ILogger _log;
        private string user = Environment.UserName;
        private List<UnitEnums.FolderType> fileList;
        private List<UnitFile> listOfFiles;

        /// <summary>
        ///     Within the constructor label value are set and textboxes are 
        ///     assigned values for default. Window is initialized as well as the 
        ///     list for the enums is loaded
        /// </summary>
        /// 
        /// <param name="unit">
        ///     Holds all the values pertaining to the unit
        /// </param>
        /// 
        /// <param name="logger">
        ///     Holds the data revolving around the logger
        /// </param>
        public UnitOptionsWindow(Unit unit, ILogger logger)
        {
            InitializeComponent();
            this.unit = unit;
            _log = logger;
            fileList = new List<UnitEnums.FolderType>();
            listOfFiles = new List<UnitFile>();

            fileList.Add(UnitEnums.FolderType.Certificates);
            fileList.Add(UnitEnums.FolderType.Photos);
            fileList.Add(UnitEnums.FolderType.Misc);

            lblCustomer.Content = unit.Customer.Name;
            lblSerial.Content = unit.GetSerialNumber();
            lblService.Content = unit.GetService().ToString();
            lblStatus.Content = unit.GetStatus().ToString();
            lblJobNumber.Content = unit.GetJobNumber();
            lblGallon.Content = unit.Gallons;

            txtSerialNumberChange.Text = unit.GetSerialNumber();
            txtNBNumberChange.Text = unit.GetNBNumber();
            txtCustNumberChange.Text = unit.GetCustNumber();
            txtGallonsChange.Text = unit.Gallons;

            cboFolder.ItemsSource = fileList;
        }

        /// <summary>
        ///     Checks for empty textboxes before running method.
        ///     Once cleared, method creates the new file path and 
        ///     use File.IO package to upload the file into the structure. 
        ///     Finally, textboxes are reset to ""
        /// </summary>
        /// 
        /// <param name="path">
        ///     Holds the base path at which the unit lives
        /// </param>
        /// 
        /// <param name="name">
        ///     Holds the value that the user wants to name the document
        /// </param>
        private void FileUpload(string path, string name)
        {
            if (cboDocType.Text == "" || txtDocName.Text == "")
            {
                MessageBox.Show("Please Select a Document Type or Insert a Name", "Missing Type Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _log.Warning($"{user} Did NOT enter Doc Type for submission");
                return;
            }

            try
            {
                string userFilePath = this.unit.GetUserFile("");

                File.Copy(userFilePath, $"{DocSelectionSwitch(cboDocType.Text, this.unit.Path, txtDocName.Text)}.{userFilePath.Split(".")[1]}");
                _log.Information($"{user} UPLOADED {name} to {path}");

                txtDocName.Text = "";
                cboDocType.Text = "";

                MessageBox.Show($"You have uploaded {name} to {this.unit.GetSerialNumber()}", "Document Upload", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an issue uploading the document\n\n{ex.Message}", "Document Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} attempted to UPLOAD file {name} to {path}. ERROR: {ex.Message}");
            }
        }

        /// <summary>
        ///     Removes the file that has been passed to the method
        /// </summary>
        /// 
        /// <param name="path">
        ///     holds the value of the file's path to be deleted
        /// </param>
        private void RemoveFile(string path)
        {
            File.Delete(path);
            _log.Warning($"{user} DELETED file {path}");
        }

        /// <summary>
        ///     takes the job number provided by the user and appends it to the
        ///     path of the folder. Program uses '&' to delimit the string for 
        ///     a job number. To admend, method uses File.IO to 'move' the folder
        ///     using a different name than previously and then updates the unit's
        ///     path variable
        /// </summary>
        /// 
        /// <param name="path">
        ///     Holds the value of the path where the folder is located at
        /// </param>
        /// 
        /// <param name="jobNumber">
        ///     Holds the value provided by the user. set as string because value *always* starts with a 'V'
        /// </param>
        private void AddJobNo(string path, string jobNumber)
        {
            try
            {
                var oldNumber = this.unit.GetJobNumber();
                var tempPath = path.Contains("&") ? path.Split("&")[0] : path;

                Directory.Move(path, $"{tempPath}&{jobNumber.ToUpper()}");
                _log.Information($"{user} changed {this.unit}'s Job number from {oldNumber} to {jobNumber}");

                // set label to show updated job number on page

                MessageBox.Show($"Job Number: {jobNumber} Posted Successfully!", "Job Number Added", MessageBoxButton.OK, MessageBoxImage.Information);
                lblJobNumber.Content = jobNumber;
                this.unit.Path = $"{tempPath}&{jobNumber.ToUpper()}";
                this.unit.SetJobNumber(this.unit.Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error adding job number {jobNumber} to unit {this.unit.GetSerialNumber()}", "Job Number Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} FAILED adding/changing job numbers to {jobNumber} \t {ex.Message}");
            }
        }

        /// <summary>
        ///     Takes the new status value from the combobox 
        ///     and passes it into the unit's changestatus method
        /// </summary>
        /// 
        /// <param name="unit">
        ///     Holds the value of the unit whose value is being changed
        /// </param>
        /// 
        /// <param name="status">
        ///     Holds the value of the new status that is being changed to
        /// </param>
        private void ChangeTankStatus(Unit unit, UnitEnums.Status status)
        {
            try
            {
                UnitEnums.Status prevStatus = unit.GetStatus();

                unit.ChangeStatus(status);
                _log.Information($"{user} CHANGED STATUS of {unit} from {prevStatus} to {status}");
                lblStatus.Content = status;
                MessageBox.Show($"Unit: {this.unit.GetSerialNumber()} was changed from {prevStatus} to {status}", "Status Change Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error changing the Status. \n\n Please try again!", "Changing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} failed to CHANGED STATUS of {unit} from {unit.GetStatus()} to {status}", ex);
            }
        }

        /// <summary>
        ///     Takes the new Service value form the combobox
        ///     and passes it into the unit's changeservice method
        /// </summary>
        /// 
        /// <param name="unit">
        ///     Holds the value of the unit whose value is being changed
        /// </param>
        /// 
        /// <param name="service">
        ///     Holds the value of the new service that is being changed to
        /// </param>
        private void ChangeTankService(Unit unit, Service service)
        {
            try
            {
                UnitEnums.Service prevService = unit.GetService();

                unit.ChangeService(service);
                _log.Information($"{user} CHANGED SERVICE of {unit} from {prevService} to {service}");
                lblService.Content = service;
                MessageBox.Show($"Unit: {this.unit.GetSerialNumber()} was changed from {prevService} to {service}", "Service Change Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error changing the Status.\n\nPlease try again!", "Changing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} failed to CHANGED SERVICE of {unit} from {unit.GetService()} to {service}", ex);
            }
        }

        /// <summary>
        ///     Checks all the UnitEnums.Status values and compares them.
        /// </summary>
        /// <param name="status">String value that was selected from the combobox</param>
        /// <returns> A UnitEnums.Status value that represents the string value that was passed in</returns>
        public Status StatusSwitch(string status)
        {
            switch (status)
            {
                case "Rehabilitation":
                    return Status.Rehabilitation;
                case "Refurbishment":
                    return Status.Refurbishment;
                default:
                    return Status.Unknown;
            }
        }

        /// <summary>
        ///     Checks all the UnitEnums.Service values and compares them.
        /// </summary>
        /// <param name="service">String value that was selected from the combobox</param>
        /// <returns> A UnitEnums.Service value that represents the string value that was passed in</returns>
        public Service ServiceSwitch(string service)
        {
            switch (service)
            {
                case "Medical":
                    return Service.Medical;
                case "Industrial":
                    return Service.Industrial;
                case "Laser":
                    return Service.Laser;
                default:
                    return Service.Unknown;
            }
        }

        /// <summary>
        ///     Checks for empty textbox and either warns the user of missing text or it passes the value onto the FileUpload method
        /// </summary>
        /// 
        /// <seealso cref="FileUpload(string, string)"/>
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (cboDocType.Text == "")
            {
                MessageBox.Show("Please Select the Type of Document", "Missing Document", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                FileUpload(FileSwitch(cboDocType.Text, this.unit.Path), txtDocName.Text);
            }
        }

        /// <summary>
        ///     Searches through the list of files for the correct file. Once found, it passes the UnitFile to RemoveFile method
        /// </summary>
        /// <seealso cref="RemoveFile(string)"/>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            UnitFile file = listOfFiles.Find(item => item == cboDocument.SelectedItem);

            try
            {
                RemoveFile(file.Path);
                listOfFiles.Remove(file);

                cboDocument.SelectedItem = null;
                cboDocument.Items.Clear();
                cboDocument.ItemsSource = listOfFiles;

                MessageBox.Show($"File {file.Name} was deleted from the System!", "Deletion Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error deleting the file.", "File Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} had an error deleting a file. ERROR: {ex.Message}");
            }
        }

        /// <summary>
        ///     Checks for the text field to be empty, if all is good then it passes the value to AddJobNo method
        /// </summary>
        /// <seealso cref="AddJobNo(string, string)"/>
        private void btnJobNum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtJobNum.Text != "")
                {
                    AddJobNo(this.unit.Path, txtJobNum.Text);
                }

                txtJobNum.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error adding the Job Number", "Error Job change", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} FAILED JOB CHANGE from {this.unit.GetJobNumber()} to {txtJobNum.Text}", ex);
            }
        }

        /// <summary>
        ///     Performs a switch statement on the combobox value and passes it to ChangeTankService
        /// </summary>
        /// <seealso cref="ChangeTankService(Unit, Service)"/>
        /// <seealso cref="ServiceSwitch(string)"/>
        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            ChangeTankService(this.unit, ServiceSwitch(cboService.Text));
            cboService.Text = "";
        }

        /// <summary>
        ///     Performs switch statement on the combobox value and passes it to ChangeTankStatus
        /// </summary>
        /// <seealso cref="ChangeTankStatus(Unit, Status)"/>
        /// <seealso cref="StatusSwitch(string)"/>
        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            ChangeTankStatus(this.unit, StatusSwitch(cboStatus.Text));

            cboStatus.Text = "";
        }

        /// <summary>
        ///     takes the path and appends the proper ending on the end of the path
        /// </summary>
        /// <param name="type">Holds the value from a combobox that details what folder is being utilized</param>
        /// <param name="path">Holds the base path of the unit prior to adding the folder extensions</param>
        /// <returns>String that contains the path to be utilized</returns>
        private string FileSwitch(string type, string path)
        {
            switch (type)
            {
                case "Quote":
                    return $"{path}\\Quotes";
                case "Photos":
                    return $"{path}\\Photos";
                case "Certificates":
                    return $"{path}\\Certificates";
                case "Misc":
                    return $"{path}";
                default:
                    return $"{path}";
            }
        }

        /// <summary>
        ///     Performs Regex on the unit's path to remove the job number from the file path. Once complete File.IO is utilized to 'Move' the file
        /// </summary>
        private void btnResetJobNum_Click(object sender, RoutedEventArgs e)
        {
            var resp = MessageBox.Show("This will remove the Job Number from this unit.\n\n Are you sure?", "Job Number Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resp == MessageBoxResult.Yes)
            {
                try
                {
                    string newPath = this.unit.Path.Split("&")[0];

                    Directory.Move(this.unit.Path, newPath);
                    _log.Information($"{user} REMOVED job number for {this.unit}");
                    MessageBox.Show($"Job Number: {this.unit.GetJobNumber()} for {this.unit.GetSerialNumber()} was removed.", "Job Removal", MessageBoxButton.OK, MessageBoxImage.Information);

                    lblJobNumber.Content = "Not Assigned";
                    this.unit.Path = newPath;
                    this.unit.SetJobNumber(this.unit.Path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error removing the Job Number", "Job Number Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _log.Error($"{user} FAILED REMOVING {this.unit} job number. Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Cancelled Job Number Removal!", "Cancelling", MessageBoxButton.OK, MessageBoxImage.Information);
                _log.Information($"{user} chose not to delete job number {this.unit.GetJobNumber()}");
                return;
            }
        }

        /// <summary>
        ///     Checks for empty comments. Method then appends the comments to the end of the file.
        /// </summary>
        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            if (txtComments.Text != "")
            {

                try
                {
                    System.IO.File.SetAttributes($"{this.unit.Path}\\Comments.txt", FileAttributes.Normal);
                    File.AppendAllText($"{this.unit.Path}\\Comments.txt", $"{Environment.NewLine}{Environment.NewLine}{txtComments.Text}{Environment.NewLine} Commented By: {user} @ {DateTime.Now}");
                    System.IO.File.SetAttributes($"{this.unit.Path}\\Comments.txt", FileAttributes.ReadOnly);

                    _log.Information($"{user} ADDED COMMENT to tank {this.unit.GetSerialNumber()}");

                    txtComments.Text = "";
                    MessageBox.Show("Comments Posted Successfully!", "Additional Comments", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error adding the comment, please try again!", "Comments Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _log.Error($"{user} FAILED adding COMMENT to unit {this.unit}. Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please imput a comment for submission before pressing submit", "Missing COmments", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Warning($"{user} attempted to comment without writing anything");
            }
        }

        /// <summary>
        /// Opens the Comment.txt file that is inside of each unit's folder
        /// </summary>
        /// 
        /// <seealso cref="OpenDoc(string)"/>
        private void btnViewComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenDoc($"{this.unit.Path}\\Comments.txt");
                _log.Information($"{Environment.UserName} viewed comments for {this.unit}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error opening the comments", "Comments Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} COULD NOT view comments for unit: {this.unit}. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Takes the path of the file to be opened and attempts to open the file.
        /// </summary>
        /// <param name="path">Holds the value of the file's path that is to  be opened</param>
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
                _log.Information($"{user} has opened {path}");
            }
            catch (Exception ex)
            {
                _log.Error($"{Environment.UserName} tried OPENING Document {path}. ERROR: {ex.Message}");
                throw new Exception();
            }
        }

        /// <summary>
        ///     Appends to the path which folder is being navigated to to upload file.
        /// </summary>
        /// 
        /// <param name="docType">
        ///     Holds the value of what folder is being searched. This value comes from a combobox
        /// </param>
        /// 
        /// <param name="unitPath">
        ///     holds the value of the unit's full path
        /// </param>
        /// 
        /// <param name="name">
        ///     Holds the value of the document name. Generally it will be left empty
        /// </param>
        /// 
        /// <returns>
        ///     String which holds the value of the file path with name added
        /// </returns>
        private string DocSelectionSwitch(string docType, string unitPath, string name = "")
        {
            switch (docType)
            {
                case "Certificates":
                    return $"{unitPath}\\Certificates\\{name}";
                case "Photos":
                    return $"{unitPath}\\Photos\\{name}";
                case "Misc":
                    return $"{unitPath}\\{name}";
                default:
                    return "";
            }
        }

        /// <summary>
        ///     When a folder is selected, method cycles through the folder structure to find all files 
        ///     inside. Method create a new UnitFile instance and adds it to a list
        /// </summary>
        private void cboFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboDocument.ItemsSource = null;
            try
            {
                string[] files;

                if (cboFolder.SelectedIndex == 2)
                {
                    if (this.unit.Customer.Name == "AirGas")
                    {
                        files = Directory.GetFiles($"{this.unit.Path}");

                        foreach (string file in files)
                        {
                            UnitFile tempFile = new UnitFile(file.Split("\\")[8], file);
                            listOfFiles.Add(tempFile);
                        }
                    }
                    else
                    {
                        files = Directory.GetFiles($"{this.unit.Path}");

                        foreach (string file in files)
                        {
                            UnitFile tempFile = new UnitFile(file.Split("\\")[7], file);
                            listOfFiles.Add(tempFile);
                        }
                    }
                }
                else
                {
                    if (this.unit.Customer.Name == "AirGas")
                    {
                        files = Directory.GetFiles($"{this.unit.Path}\\{cboFolder.SelectedValue}");

                        foreach (string file in files)
                        {
                            UnitFile tempFile = new UnitFile(file.Split("\\")[9], file);
                            listOfFiles.Add(tempFile);
                        }
                    }
                    else
                    {
                        files = Directory.GetFiles($"{this.unit.Path}\\{cboFolder.SelectedValue}");

                        foreach (string file in files)
                        {
                            UnitFile tempFile = new UnitFile(file.Split("\\")[8], file);
                            listOfFiles.Add(tempFile);
                        }
                    }

                }

                cboDocument.ItemsSource = listOfFiles;
                _log.Information($"{user} has loaded documents for removal");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an issue grabbing all the documents.\n\n If you continue to have trouble, click 'View Folder' button for quick access!", "Doument Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} had an ERROR getting all documents form {this.unit}. ERROR: {ex.Message}");
            }
        }

        /// <summary>
        ///     Takes the path of the unit assigned and simply opens the folder within a dialog box
        /// </summary>
        /// <seealso cref="OpenDoc(string)"/>
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenDoc(this.unit.Path);
                _log.Information($"{user} opened folder for {this.unit}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error opening the Folder.\n\n Please Try Again Later!", "Folder Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} FAILED opening folder for {this.unit}. ERROR: {ex.Message}");
            }
        }

        /// <summary>
        ///     Checks all of the textboxes for values. When complete, method assigns the values to the unit
        /// </summary>
        private void btnSubmitChange_Click(object sender, RoutedEventArgs e)
        {
            if (txtSerialNumberChange.Text != "")
            {
                if (txtNBNumberChange.Text != "")
                {
                    if (txtCustNumberChange.Text != "")
                    {
                        if (txtGallonsChange.Text != "")
                        {
                            try
                            {
                                this.unit.SetSerialNumber(txtSerialNumberChange.Text);
                                this.unit.SetNBNumber(txtNBNumberChange.Text);
                                this.unit.SetCustNo(txtCustNumberChange.Text);
                                this.unit.Gallons = txtGallonsChange.Text;

                                this.unit.Path = UpdatePath(this.unit);

                                MessageBox.Show("Unit details have been updated!", "Unit Updates", MessageBoxButton.OK, MessageBoxImage.Information);
                                _log.Information($"{user} UPDATED tank data");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("There was an error updating the Tank Data. \n\n Please try again later.", "Updating Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                _log.Error($"{user} encountered error updating tank data. ERROR: {ex.Message}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please input data for the Gallons!", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            txtGallonsChange.Text = this.unit.Gallons;
                            _log.Warning($"{user} didn't include data in submission");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input data for the Customer Number!", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtCustNumberChange.Text = this.unit.GetCustNumber();
                        _log.Warning($"{user} didn't include data in submission");
                    }
                }
                else
                {
                    MessageBox.Show("Please input data for the National Board!", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtNBNumberChange.Text = this.unit.GetNBNumber();
                    _log.Warning($"{user} didn't include data in submission");
                }
            }
            else
            {
                MessageBox.Show("Please input data for the Serial Number!", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtSerialNumberChange.Text = this.unit.GetSerialNumber();
                _log.Warning($"{user} didn't include data in submission");
            }
        }

        /// <summary>
        /// Method runs when critical unit data is being updated and the path must be refreshed. This loops through and adds the beginning bit of the unit's path before taking in all textbox values and adding them in.
        /// </summary>
        /// <param name="unit">Holds the value of the unit which must have it's path altered</param>
        /// <returns>Returns a string that holds the unit's new path for updating</returns>
        private string UpdatePath(Unit unit)
        {
            try
            {
                string[] structure = unit.Path.Split("\\");
                string tempPath = $"";

                for (int i = 0; i < structure.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        tempPath = structure[i];
                    }
                    else
                    {
                        tempPath += $"\\{structure[i]}";
                    }
                }

                tempPath = $"{tempPath}\\{unit.Customer.Name}-{unit.Gallons}-SN_{unit.GetSerialNumber()}-NB_{unit.GetNBNumber()}-CustNo_{unit.GetCustNoForFile()}-{unit.ServiceSwitch(unit.GetService())}-{unit.StatusSwitch(unit.GetStatus())}-LI";

                Directory.Move(this.unit.Path, tempPath);

                return tempPath;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks for common ways of spelling the customer and corrects it to what is proper
        /// </summary>
        /// <param name="input">Contains the value of the user inputted customer name</param>
        /// <returns>
        ///     String that contains the corrected company name. 
        ///     If the company was not matched, it uppercases the name and 
        ///     uses that for the folder.
        /// </returns>
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
        /// Method takes the unit folder and moves it into the 'Closed' folder within the customer
        /// </summary>
        private void btnCloseFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderName = $"{CompanyVerify(this.unit.Customer.Name)}-{this.unit.Gallons}-SN_{unit.GetSerialNumber()}-NB_{unit.GetNBNumber()}-CustNo_{unit.GetCustNumber()}-{unit.GetService()}-{unit.GetStatus()}-LI";
            string newPath = $"{this.unit.Customer.GetPath()}\\Closed\\{folderName}";

            try
            {
                Directory.Move(this.unit.Path, newPath);

                MessageBox.Show($"Job {this.unit.GetJobNumber()} has been moved into the 'Closed' folder for this customer", "Job Closed", MessageBoxButton.OK, MessageBoxImage.Information);
                _log.Information($"{user} CLOSED job {this.unit.GetJobNumber()}");

                this.unit.Path = newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error closing job {this.unit.GetJobNumber()}", "Closing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _log.Error($"{user} FAILED closing job {this.unit.GetJobNumber()}. ERROR: {ex.Message}");
            }
        }
    }
}
