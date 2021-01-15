namespace BulkTankOrganization.models
{
    /// <summary>
    ///     Class created to hold data refering to a Alloy Custom Product File
    /// </summary>
    public class UnitFile
    {
        /// <summary>
        ///     Name of the File
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Path of the file location
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Assigns values to the classes fields
        /// </summary>
        /// <param name="name">Holds value of the name</param>
        /// <param name="path">Holds the value of the file's path</param>
        public UnitFile(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        /// <summary>
        ///     Override the default ToString to retrieve the critical data
        /// </summary>
        /// <returns>String containing the file's name</returns>
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
