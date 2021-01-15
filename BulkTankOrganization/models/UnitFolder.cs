namespace GUIBulkNaming.Models
{
    public class UnitFolder
    {
        public readonly string Company;
        public readonly string TankSize;
        public readonly string SerialNumber;
        public readonly string CustomerNumber;
        public readonly string NationalNumber;
        public readonly string TankService;
        public readonly string TankStatus;
        public readonly string TankComments;

        public UnitFolder(UnitBuilder builder)
        {
            Company = builder.Company;
            TankSize = builder.TankSize;
            SerialNumber = builder.SerialNumber;
            CustomerNumber = builder.CustomerNumber;
            NationalNumber = builder.NationalNumber;
            TankService = builder.TankService;
            TankStatus = builder.TankStatus;
        }

        public override string ToString()
        {
            return $"{this.SerialNumber}";
        }
    }
}
