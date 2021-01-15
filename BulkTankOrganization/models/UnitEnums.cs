namespace BulkTankOrganization.models
{
    /// <summary>
    ///     Class that contains enums used throughout the solution
    /// </summary>
    public class UnitEnums
    {

        /// <summary>
        ///     Enum that holds the values for all the Status options that are available
        /// </summary>
        public enum Status
        {
            Rehabilitation,
            Refurbishment,
            Unknown
        }

        /// <summary>
        ///     Enum that contains all the values for Service options that are available
        /// </summary>
        public enum Service
        {
            Medical,
            Industrial,
            Laser,
            Unknown
        }

        /// <summary>
        ///     Enum that contains the different folders that are standard in a unit folder
        /// </summary>
        public enum FolderType
        {
            Certificates,
            Photos,
            Misc
        }
    }
}
