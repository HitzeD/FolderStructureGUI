using System.Collections.Generic;

namespace BulkTankOrganization.models
{
    /// <summary>
    ///     Class that holds data Specific to Alloy Custom Products Customers
    /// </summary>
    public class Customer
    {
        private static long StaticID = 0;

        public long ID;

        /// <summary>
        ///     Holds the value of the cusomter's name
        /// </summary>
        public string Name { get; set; }
        private string path { get; set; }
        private List<Unit> units { get; set; }

        /// <summary>
        ///     Assigns the initializing variable to the proper field
        /// </summary>
        /// 
        /// <param name="name">
        ///     Holds the name of the customer
        /// </param>
        /// 
        /// <param name="path">
        ///     Holds the value of the Customer's File Path
        /// </param>
        public Customer(string name, string path)
        {
            StaticID++;

            this.ID = StaticID;
            this.Name = name;
            this.path = path;
            this.units = new List<Unit>();
        }

        /// <summary>
        ///     Adds a unit to the Customer's list
        /// </summary>
        /// 
        /// <param name="unit">
        ///     Holds the value of the unit that is going to be added to the customer's list
        /// </param>
        /// 
        /// <returns>
        ///     List data type of customer's list with Unit
        /// </returns>
        public List<Unit> AddUnitToList(Unit unit)
        {
            units.Add(unit);
            return this.units;
        }

        /// <summary>
        ///     Gets the Customer's list of Units
        /// </summary>
        /// 
        /// <returns>
        ///     List containing the Customer's units
        /// </returns>
        public List<Unit> GetUnitList()
        {

            return this.units;
        }

        /// <summary>
        ///     gets the Customer's File path
        /// </summary>
        /// 
        /// <returns>
        ///     String containing the Customer's path
        /// </returns>
        public string GetPath()
        {
            return this.path;
        }

        /// <summary>
        ///     Ovverride the ToString method to display proper needs
        /// </summary>
        /// 
        /// <returns>
        ///     String containing critical tank data
        /// </returns>
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
