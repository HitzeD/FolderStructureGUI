namespace BulkTankOrganization.models
{
    /// <summary>
    ///     Solution to store data that must be returned from a single function within the application
    /// </summary>
    public struct QuoteData
    {

// Struct Field Variables

        /// <summary>
        ///     Holds the Path for the location of the new Quote
        /// </summary>
        private string Path;

        /// <summary>
        ///     Holds the count of how many Quotes there are
        /// </summary>
        private int Count;

// Struct Methods

        /// <summary>
        ///     returns the path of the Structure
        /// </summary>
        /// <returns>A string which contains the Unit's Path</returns>
        public string GetPath()
        {
            return this.Path;
        }

        /// <summary>
        ///     Sets the path of the structure
        /// </summary>
        /// 
        /// <param name="path">
        ///     Holds the value of the new path being assigned
        /// </param>
        public void SetPath(string path)
        {
            this.Path = path;
        }

        /// <summary>
        ///     Gets the Count of Quotes in the unit
        /// </summary>
        /// <returns>Int that contains the number of quotes</returns>
        public int GetCount()
        {
            return this.Count;
        }

        /// <summary>
        ///     Sets the amount of Quotes within a unit.
        /// </summary>
        /// 
        /// <param name="count">
        ///     Holds the int value of how many quotes it cycled through
        /// </param>
        public void SetCount(int count)
        {
            this.Count = count;
        }
    }
}
