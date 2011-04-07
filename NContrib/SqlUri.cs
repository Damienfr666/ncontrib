﻿using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NContrib.Extensions;

namespace NContrib {

    public static class SqlUri {

        
        /// <summary>
        /// Parses a SQL Server connection URI in the format
        /// mssql://username:password@server\instance/database
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ParseSqlServerUri(string uri) {
            const string re = @"(?xi)
                ^
                (?<protocol>mssqls?)
                ://
                (?:
                    (?:
                        (?<username>[^:@]+)
                        (?: [:] (?<password>[^@]+) )?
                    )?
                    [@]
                )?
                (?:     (?<server>[^/]+) )
                (?: [/] (?<database>\w+)   )?
                $";

            var match = Regex.Match(uri, re);

            if (!match.Success)
                throw new ArgumentException("Not recognised as a SQL Server connection string");

            Func<string, string> getVal = name => match.Groups[name].Value.IsBlank() ? null : Uri.UnescapeDataString(match.Groups[name].Value);
            Action<string, Action<string>> assign = (name, action) => { var value = getVal(name); if (value != null) action(value); };

            var protocol = getVal("protocol").ToLower();

            if (protocol.NotIn("mssql", "mssqls"))
                throw new ArgumentException(string.Format("{0} is not a valid protocol", protocol));

            var csb = new SqlConnectionStringBuilder();

            assign("server", s => csb.DataSource = s);
            assign("database", s => csb.InitialCatalog = s);

            if (protocol == "mssqls")
                csb.Encrypt = true;

            if (getVal("username").IsBlank())
                csb.IntegratedSecurity = true;
            else {
                assign("username", s => csb.UserID = s);
                assign("password", s => csb.Password = s);
            }
            
            return csb.ConnectionString;
        }
    }
}