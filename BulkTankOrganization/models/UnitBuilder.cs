namespace GUIBulkNaming.Models
{
    /// <summary>
    ///     Created so that a Unit class does not have an insurmountable number of parameters
    /// </summary>
    public struct UnitBuilder
    {
        public string Company;
        public string TankSize;
        public string SerialNumber;
        public string CustomerNumber;
        public string NationalNumber;
        public string TankService;
        public string TankStatus;

        public static UnitBuilder unit()
        {
            return new UnitBuilder();
        }

        public UnitBuilder withCompany(string company)
        {
            this.Company = company;
            return this;
        }

        public UnitBuilder withSize(string size)
        {
            this.TankSize = size;
            return this;
        }

        public UnitBuilder withSerial(string serial)
        {
            if (serial == "")
            {
                this.SerialNumber = "{blank}";
            }
            else
            {
                if (serial.Contains('-'))
                {
                    var firstHalf = serial.Split("-")[0];
                    var secondHalf = serial.Split("-")[1];

                    this.SerialNumber = $"{firstHalf};{secondHalf}";
                }
                else
                {
                    this.SerialNumber = serial;
                }
            }

            return this;
        }

        public UnitBuilder withCustNo(string custNo)
        {
            if (custNo == "")
            {
                this.CustomerNumber = "{blank}";
            }
            else
            {
                if (custNo.Contains('-'))
                {
                    var firstHalf = custNo.Split("-")[0];
                    var secondHalf = custNo.Split("-")[1];

                    this.CustomerNumber = $"{firstHalf};{secondHalf}";
                }
                else
                {
                    this.CustomerNumber = custNo;
                }
            }
            return this;
        }

        public UnitBuilder withNationalNo(string nb)
        {
            if (nb == "")
            {
                this.NationalNumber = "{blank}";
            }
            else
            {
                this.NationalNumber = nb;
            }
            return this;
        }

        public UnitBuilder withService(string service)
        {
            this.TankService = service;
            return this;
        }

        public UnitBuilder withStatus(string status)
        {
            this.TankStatus = status;
            return this;
        }

        public UnitFolder build()
        {
            return new UnitFolder(this);
        }
    }
}
