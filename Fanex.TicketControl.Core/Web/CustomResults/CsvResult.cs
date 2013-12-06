// ----------------------------------------------------------------------
// <copyright file="CsvResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Web.CustomResults
{
    using System;
    using System.IO;

    using ExcelUtil.Spreadsheet;

    /// <summary>
    /// CSV Result.
    /// </summary>
    public class CsvResult : SpreadSheetResult
    {
        public CsvResult(ExcelFile file, string fileName)
            : base(file, fileName)
        {
        }

        protected override string ContentType
        {
            get
            {
                return "application/csv";
            }
        }

        protected override string Extension
        {
            get
            {
                return ".csv";
            }
        }

        protected override Action<ExcelFile, Stream> SaveAction
        {
            get
            {
                return (f, s) => f.SaveCsv(s, CsvType.CommaDelimited);
            }
        }
    }
}