﻿namespace NContrib.International {
    
    public class CountrySubdivision {

        public string Code { get; set; }

        public string CountryCode {
            get { return Code.Substring(0, 2); }
        }

        public string SubdivisionCode {
            get { return Code.Substring(3, Code.Length - 3); }
        }

        public string Name { get; set; }

        /// <summary>
        /// State, District, Outlying Territory, Parish, Emirate, etc
        /// </summary>
        public string Category { get; set; }
        

        public CountrySubdivision(string code, string name, string category) {
            Code = code;
            Name = name;
            Category = category;
        }

        public static CountrySubdivisionCollection CountrySubdivisions = new CountrySubdivisionCollection {
            { "US-AL", "Alabama", "state" },
            { "US-AK", "Alaska", "state" },
            { "US-AZ", "Arizona", "state" },
            { "US-AR", "Arkansas", "state" },
            { "US-CA", "California", "state" },
            { "US-CO", "Colorado", "state" },
            { "US-CT", "Connecticut", "state" },
            { "US-DE", "Delaware", "state" },
            { "US-FL", "Florida", "state" },
            { "US-GA", "Georgia", "state" },
            { "US-HI", "Hawaii", "state" },
            { "US-ID", "Idaho", "state" },
            { "US-IL", "Illinois", "state" },
            { "US-IN", "Indiana", "state" },
            { "US-IA", "Iowa", "state" },
            { "US-KS", "Kansas", "state" },
            { "US-KY", "Kentucky", "state" },
            { "US-LA", "Louisiana", "state" },
            { "US-ME", "Maine", "state" },
            { "US-MD", "Maryland", "state" },
            { "US-MA", "Massachusetts", "state" },
            { "US-MI", "Michigan", "state" },
            { "US-MN", "Minnesota", "state" },
            { "US-MS", "Mississippi", "state" },
            { "US-MO", "Missouri", "state" },
            { "US-MT", "Montana", "state" },
            { "US-NE", "Nebraska", "state" },
            { "US-NV", "Nevada", "state" },
            { "US-NH", "New Hampshire", "state" },
            { "US-NJ", "New Jersey", "state" },
            { "US-NM", "New Mexico", "state" },
            { "US-NY", "New York", "state" },
            { "US-NC", "North Carolina", "state" },
            { "US-ND", "North Dakota", "state" },
            { "US-OH", "Ohio", "state" },
            { "US-OK", "Oklahoma", "state" },
            { "US-OR", "Oregon", "state" },
            { "US-PA", "Pennsylvania", "state" },
            { "US-RI", "Rhode Island", "state" },
            { "US-SC", "South Carolina", "state" },
            { "US-SD", "South Dakota", "state" },
            { "US-TN", "Tennessee", "state" },
            { "US-TX", "Texas", "state" },
            { "US-UT", "Utah", "state" },
            { "US-VT", "Vermont", "state" },
            { "US-VA", "Virginia", "state" },
            { "US-WA", "Washington", "state" },
            { "US-WV", "West Virginia", "state" },
            { "US-WI", "Wisconsin", "state" },
            { "US-WY", "Wyoming", "state" },
            { "US-DC", "District of Columbia", "district" },
            { "US-AS", "American Samoa", "outlying territory" },
            { "US-GU", "Guam", "outlying territory" },
            { "US-MP", "Northern Mariana Islands", "outlying territory" },
            { "US-PR", "Puerto Rico", "outlying territory" },
            { "US-UM", "United States Minor Outlying Islands", "outlying territory" },
            { "US-VI", "Virgin Islands, U.S.", "outlying territory" },
        };
    }
}
