using BulkTankOrganization.models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using static BulkTankOrganization.models.UnitEnums;

namespace BulkTankOrganization
{
    /// <summary>
    ///     Class that holds data specific to Alloy Custom Product repair Bulk Tanks
    /// </summary>
    public class Unit
    {
        private static long _staticId_ = 0;

        private long TankId;
        private string SerialNumber;
        private string NationalBoardNo;
        private string CustomerNo;                                                      // could be blank
        private string jobNumber;

        /// <summary>
        ///     File location path for the tank
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Holds the size of the tank in a string
        /// </summary>
        /// <remarks>500, 1500, 3000, etc</remarks>
        public string Gallons { get; set; }

        /// <summary>
        ///     Holds the value of the Customer that the Unit belongs to
        /// </summary>
        public Customer Customer { get; set; }

        private Service Service;                                                        // contains Industrial, Medical, Laser , Unknown
        private Status Status;                                                          // contains Rehab, Refurb, or Unknown status

        /// <summary>
        ///     A dictionary that uses {"FOLDER NAME", UNITFILE} structure for files
        /// </summary>
        public Dictionary<string, UnitFile> unitFiles { get; set; }

        /// <summary>
        ///     A dictionary that uses {"FOLDER NAME", UNITFILE} structure for folders
        /// </summary>
        public Dictionary<string, UnitFile> unitFolders { get; set; }

        /// <summary>
        ///     Initializes the class with the Path and 
        ///     the Name of the Unit along with initialize the lists
        /// </summary>
        /// <param name="path">Holds the value of the path in the folder structure to the unit</param>
        /// <param name="name">Holds the value of the Serial number of the tank and is assigned to the Serial Number field</param>
        public Unit(string path,
                    string name)
        {
            _staticId_++;
            TankId = _staticId_;
            SerialNumber = this.DecryptFromFile(this.RemoveUnderScores(name));          // This removes underscores and then removes a semicolon in place for a hyphen
            Path = path;

            unitFiles = new Dictionary<string, UnitFile>();
            unitFolders = new Dictionary<string, UnitFile>();

            InitializeJobNumber(Path);                                                  // This will check to see if a Job number has been assigned
        }

        /// <summary>
        ///     retrieves the Unit's Serial number
        /// </summary>
        /// <returns>String that holds the value of the Serial Number</returns>
        public string GetSerialNumber()
        {
            return DecryptFromFile(this.SerialNumber);
        }

        /// <summary>
        ///     Sets the Serial Number of tank
        /// </summary>
        /// 
        /// <param name="number">
        ///     Holds the value of the new Serial Number of the 
        ///     Unit. Set as string because it can contain hyphens and letters
        /// </param>
        public void SetSerialNumber(string number)
        {
            this.SerialNumber = EncryptToFile(number);
        }

        /// <summary>
        ///     Take the National Board number of the 
        ///     unit and run it through the 'Decrypter' to make it readable. 
        /// </summary>
        /// <seealso cref="DecryptFromFile(string)"/>
        /// <returns>String containing readable print</returns>
        public string GetNBNumber()
        {
            return DecryptFromFile(this.NationalBoardNo);
        }

        /// <summary>
        ///     Sets the Customer number by passing the string to the 'Encrypter' method to delimit the string
        /// </summary>
        /// <seealso cref="EncryptToFile(string)"/>
        /// <param name="number" type="string">String that contains the limited version of the string</param>
        public void SetCustNo(string number)
        {
            this.CustomerNo = EncryptToFile(RemoveUnderScores(number));
        }

        /// <summary>
        ///     Sets the National Board number of the unit
        /// </summary>
        /// <seealso cref="RemoveUnderScores(string)"/>
        /// <param name="number">String that contains the unit's National Board number</param>
        public void SetNBNumber(string number)
        {
            this.NationalBoardNo = RemoveUnderScores(number);
        }

        /// <summary>
        ///     Sets the Customer Number of the unit
        /// </summary>
        /// <seealso cref="EncryptToFile(string)"/>
        /// <param name="number">String that contains the Unit's Customer Number</param>
        public void SetCustNumber(string number)
        {
            this.CustomerNo = EncryptToFile(number);
        }

        /// <summary>
        /// Retrieves the Unit's Customer Number
        /// </summary>
        /// <seealso cref="DecryptFromFile(string)"/>
        /// <returns>String containing the Unit's Customer Number</returns>
        public string GetCustNumber()
        {
            return DecryptFromFile(this.CustomerNo);
        }

        /// <summary>
        ///     Retrieves the Unit's Customer Number in File Format (delimited)
        /// </summary>
        /// <returns>String that contains the Unit's Customer Number in unreadable format</returns>
        public string GetCustNoForFile()
        {
            return this.CustomerNo;
        }

        /// <summary>
        ///     Retrieves the Unit's Job Number
        /// </summary>
        /// <returns>String that contains the Unit's Job Number</returns>
        public string GetJobNumber()
        {
            return this.jobNumber;
        }

        /// <summary>
        ///     Takes the Unit's path and performs Regex 
        ///     on the string to find the Job Number. Job Number is set at the end
        ///     of the Unit path and separated by '&' char
        /// </summary>
        /// <param name="path">String that Contains the path of the unit</param>
        public void SetJobNumber(string path)
        {
            if (path.Contains("&"))
            {
                this.jobNumber = path.Split("&")[1].Trim();
            }
            else
            {
                this.jobNumber = "Not Assigned";
            }
        }

        /// <summary>
        ///     Takes the Unit's path and split by '-' into an array. 
        ///     Loop through the array and rebuild the path until 
        ///     the service section is hit. Once hit, ChangeService 
        ///     changes the designater within the path
        /// </summary>
        /// <param name="service">Type UnitEnums.Service variable of what the unit is being changed to</param>
        public void ChangeService(UnitEnums.Service service)
        {
            string[] brokenPath = Path.Split("-");
            string tempPath = "";
            int i = 0;

            foreach (string portion in brokenPath)
            {
                i++;

                if (portion == "Ind" || portion == "Med" || portion == "Lzr" || portion == "UKN")
                {
                    if (service == Service.Industrial)
                    {
                        tempPath += "-Ind";
                    }
                    else if (service == Service.Medical)
                    {
                        tempPath += "-Med";
                    }
                    else if (service == Service.Laser)
                    {
                        tempPath += "-Lzr";
                    }
                    else
                    {
                        tempPath += "-UKN";
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        tempPath += $"{portion}";
                    }
                    else
                    {
                        tempPath += $"-{portion}";
                    }
                }
            }



            Directory.Move(Path, tempPath);
            //    C:\Users\hitzed\Cryogenic Industrial Solutions LLC\Operations - Bulk Tanks\AirGas\Bulk Tanks\AG-0-SN_blank-NB_blank-CustNo_TC;67745-Ind-RH-LI


            this.Service = service;
            this.Path = tempPath;
        }

        /// <summary>
        ///     Takes the Unit's path and split by '-' into an array. 
        ///     Loop through the array and rebuild the path until 
        ///     the Status section is hit. Once hit, ChangeStatus 
        ///     changes the designater within the path
        /// </summary>
        /// <param name="status">Type UnitEnums.Status variable of what the unit is being changed to</param>
        public void ChangeStatus(UnitEnums.Status status)
        {
            string[] brokenPath = Path.Split("-");
            string tempPath = "";
            int i = 0;

            foreach (string portion in brokenPath)
            {
                i++;

                if (portion == "RH" || portion == "RF" || portion == "UKN")
                {
                    if (status == Status.Refurbishment)
                    {
                        tempPath += "-RF";
                    }
                    else if (status == Status.Rehabilitation)
                    {
                        tempPath += "-RH";
                    }
                    else
                    {
                        tempPath += "-UKN";
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        tempPath += $"{portion}";
                    }
                    else
                    {
                        tempPath += $"-{portion}";
                    }
                }
            }



            Directory.Move(Path, tempPath);


            this.Status = status;
            this.Path = tempPath;
        }

        /// <summary>
        ///     Retrieves the Unit's Service
        /// </summary>
        /// <returns>Type UnitEnum.Service containing the unit's service</returns>
        public Service GetService()
        {
            return this.Service;
        }

        /// <summary>
        ///     Sets the Unit's Service. If selection not 
        ///     found, "UNKNOWN" is set as the Service
        /// </summary>
        /// <param name="service">String containing the abbreviated service type</param>
        /// <remarks>param example: Ind, Med, Lzr, UKN</remarks>
        public void SetService(string service)
        {
            switch (service)
            {
                case "Ind":
                    this.Service = UnitEnums.Service.Industrial;
                    break;
                case "Med":
                    this.Service = UnitEnums.Service.Medical;
                    break;
                case "Lzr":
                    this.Service = UnitEnums.Service.Laser;
                    break;
                default:
                    this.Service = UnitEnums.Service.Unknown;
                    break;
            }
        }

        /// <summary>
        ///     Sets the Unit's Service. If selection not 
        ///     found, "UNKNOWN" is set as the Service
        /// </summary>
        /// <param name="status">String containing the abbreviated status type</param>
        /// <remarks>param example: RH, RF, UKN</remarks>
        public void SetStatus(string status)
        {
            switch (status)
            {
                case "RH":
                    this.Status = UnitEnums.Status.Rehabilitation;
                    break;
                case "RF":
                    this.Status = UnitEnums.Status.Refurbishment;
                    break;
                case "UKN":
                    this.Status = UnitEnums.Status.Unknown;
                    break;
                default:
                    this.Status = UnitEnums.Status.Unknown;
                    break;
            }
        }

        /// <summary>
        ///     Retrieves the Unit's Status
        /// </summary>
        /// <returns>Type UnitEnum.Status containing the unit's status</returns>
        public Status GetStatus()
        {
            return this.Status;
        }

        /// <summary>
        ///     Takes a string and removes the hyphen from it and replaces with a semicolon
        /// </summary>
        /// <param name="str">String that needs to be checked for a hyphen and replaced with a semicolon</param>
        /// <returns>String that contains the necessary changes for saving the file</returns>
        public string EncryptToFile(string str)
        {
            /*
             * Structure is broken up by hyphens
             * to segregate data more, i utilized a semicolon
             * to use in place of in-text hyphens.
             * 
             * This function swaps out the hyphen and replaces it with 
             * a semicolon
             */

            if (str == "")
            {
                return "{blank}";
            }
            else
            {
                if (str.Contains('-'))
                {
                    var firstHalf = str.Split("-")[0];
                    var secondHalf = str.Split("-")[1];

                    return $"{firstHalf};{secondHalf}";
                }
                else
                {
                    return str;
                }
            }
        }

        /// <summary>
        ///     Removes the semicolon from a string and replaces it with a hyphen
        /// </summary>
        /// <param name="str">String that has the values that needs to be changed</param>
        /// <returns>String that contains the corrected string for viewing in the window</returns>
        public string DecryptFromFile(string str)
        {
            /*
             * Structure is broken up by hyphens
             * to segregate data more, i utilized a semicolon
             * to use in place of in-text hyphens.
             * 
             * This function swaps out the semi colon and replaces it with 
             * a hyphen
             */

            if (str == "")
            {
                return "{blank}";
            }
            else
            {
                if (str.Contains(';'))
                {
                    var firstHalf = str.Split(";")[0];
                    var secondHalf = str.Split(";")[1];

                    return $"{firstHalf}-{secondHalf}";
                }
                else
                {
                    return str;
                }
            }
        }

        /// <summary>
        ///     Performs Regex to extract the needed data that is behind an underscore
        /// </summary>
        /// <param name="str">Value that needs the data extracted</param>
        /// <returns>String that contains the extracted data</returns>
        private string RemoveUnderScores(string str)
        {
            if (str.Contains("_"))
            {
                return str.Split("_")[1];
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        ///     Ovverride the ToString to provide data that is required
        /// </summary>
        /// <returns>String containing the critical information that populates the combobox</returns>
        public override string ToString()
        {
            return $"Gal: {this.Gallons}  ~  ACP#: {this.jobNumber}  ~  SN: {this.SerialNumber}  ~  Cust #: {this.CustomerNo}   ~  NB: {this.NationalBoardNo}";
        }

        /// <summary>
        ///     When the unit is created, regex is performed 
        ///     on the string to find if it has a job number 
        ///     added on using '&' to delimit
        /// </summary>
        /// <param name="path">Contains the path to the unit's location</param>
        private void InitializeJobNumber(string path)
        {
            if (this.Path.Contains("&"))
            {
                this.jobNumber = this.Path.Split("&")[1];
            }
            else
            {
                this.jobNumber = "Not Assigned";
            }
        }

        /// <summary>
        ///     Opens up a dialog and allows the user to pick a file to upload
        /// </summary>
        /// <param name="path">Contains the file path to start the dialog at</param>
        /// <returns>String that contains the file location on the users computer</returns>
        public string GetUserFile(string path)
        {

            if (path == "")
            {
                path = this.Path;
            }

            FileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = path;                     // Create new Dialog object
            dialog.ShowDialog();                                // Open Dialog 

            return dialog.FileName;
        }

        /// <summary>
        ///     Switch Statement that assigns the 
        ///     correct Service Enum to the param sent
        /// </summary>
        /// <param name="type">UnitEnums.Service type of which the unit is changing to</param>
        /// <returns>String containing the abbreviated version of the Service</returns>
        /// <remarks>Returns: Ind, Lzr, Med, UKN</remarks>
        public string ServiceSwitch(Service type)
        {
            switch (type)
            {
                case Service.Industrial:
                    return "Ind";
                case Service.Laser:
                    return "Lzr";
                case Service.Medical:
                    return "Med";
                case Service.Unknown:
                    return "UKN";
                default:
                    return "UKN";
            }
        }

        /// <summary>
        ///     Switch Statement that assigns the 
        ///     correct Service Enum to the param sent
        /// </summary>
        /// <param name="type">UnitEnums.Status type of which the unit is changing to</param>
        /// <returns>String containing abbreviated version of the UnitEnums.Status type which the unit is changing to</returns>
        /// <remarks>Returns: RF, RH, UKN</remarks>
        public string StatusSwitch(Status type)
        {
            switch (type)
            {
                case Status.Refurbishment:
                    return "RF";
                case Status.Rehabilitation:
                    return "RH";
                case Status.Unknown:
                    return "UKN";
                default:
                    return "UKN";
            }
        }
    }
}
