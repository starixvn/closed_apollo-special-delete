// --------------------------------------------------------------------------
// <copyright file="ExcelExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.6">Created</change>
// </history>
namespace Fanex.TicketControl.Core.Extensions
{
    using System;
    using System.Drawing;
    using System.Web;

    using ExcelUtil.Spreadsheet;

    using Fanex.TicketControl.Core.Resources;

    /// <summary>
    /// Excel Extension.
    /// </summary>
    public static class ExcelExtensions
    {
        public const int FontSize = 160;
        public const int FontWidth = 30;

        public static readonly Color BackgroundColor = Color.White;
        public static readonly Color EvenRowColor = Color.FromArgb(228, 242, 253);
        public static readonly Color GrandTotalRowColor = Color.FromArgb(219, 225, 187);
        public static readonly Color GrandTotalSeperatorColor = Color.FromArgb(255, 102, 0);
        public static readonly Color HeaderTitleColor = Color.FromArgb(60, 94, 165);
        public static readonly Color OddRowColor = Color.White;
        public static readonly Color RevenueLicenseeColor = Color.FromArgb(213, 132, 52);
        public static readonly Color SubTotalRowColor = Color.FromArgb(194, 217, 236);
        public static readonly Color TotalRowColor = Color.FromArgb(244, 244, 190);

        private const string FontName = "Tahoma";
        private const string NegativeNumber = "#,##0{0}_);[RED]-#,##0{1}";
        private const string NormalNumberFormat = "#,##0{0}_);-#,##0{1}";
        private const string PositiveNumberFormat = "[RED][>=0.1]#,##0{0}_);-#,##0{1}";

        /// <summary>
        /// ExportEmptyReport: export empty report message.
        /// Ex: There is no data.
        /// </summary>
        /// <param name="ws">Excel Worksheet.</param>
        /// <param name="startRow">Start Row.</param>
        /// <param name="startColumn">Start Column.</param>
        /// <param name="currentRow">Current Row.</param>
        /// <param name="endColumn">End Column.</param>
        public static void ExportEmptyReport(this ExcelWorksheet ws, int startRow, int startColumn, ref int currentRow, int endColumn)
        {
            var currentColumn = startColumn;
            var currentCellRange = ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn);
            currentCellRange.Merged = true;
            currentCellRange.Value = Labels.EmptyReportMessage;
            currentCellRange.Style.FormatCellText(Color.White, true);
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentRow++;
        }

        /// <summary>
        /// Format Empty Row Extension.
        /// </summary>
        /// <param name="ws">Work sheet.</param>
        /// <param name="startRow">Start Row.</param>
        /// <param name="startColumn">Start Column.</param>
        /// <param name="currentRow">Current Row.</param>
        /// <param name="endColumn">End Column.</param>
        public static void ExportEmptyRow(this ExcelWorksheet ws, int startRow, int startColumn, ref int currentRow, int endColumn)
        {
            var currentColumn = startColumn;
            var currentCellRange = ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn);
            currentCellRange.Merged = true;
            currentCellRange.Value = string.Empty;
            currentCellRange.Style.FormatCellText(Color.White, false);
            currentCellRange.Style.Font.Color = Color.White;
            currentRow++;
        }

        public static void FormatCellNumberStyle(this CellStyle cell, string symbol, Color color, bool isBold, bool isPositive, bool isNegative)
        {
            cell.FormatCellNumberStyle(symbol, color, Color.Black, isBold, isPositive, isNegative);
        }

        /// <summary>
        /// Format cell style.
        /// </summary>
        /// <param name="cell">Cell Style.</param>
        /// <param name="symbol">Symbol character.</param>
        /// <param name="color">Color style.</param>
        /// <param name="font">Font name.</param>
        /// <param name="isBold">Font style.</param>
        /// <param name="isPos">Set position.</param>
        /// <param name="isNag">Negative number.</param>
        public static void FormatCellNumberStyle(this CellStyle cell, string symbol, Color color, Color font, bool isBold, bool isPos, bool isNag)
        {
            cell.VerticalAlignment = VerticalAlignmentStyle.Center;
            cell.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            cell.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);

            if (isNag)
            {
                cell.NumberFormat = string.Format(NegativeNumber, symbol, symbol);
            }
            else if (isPos)
            {
                cell.NumberFormat = string.Format(PositiveNumberFormat, symbol, symbol);
            }
            else
            {
                cell.NumberFormat = string.Format(NormalNumberFormat, symbol, symbol);
            }

            cell.Font.Name = FontName;
            cell.Font.Size = FontSize;

            if (color != null)
            {
                cell.FillPattern.SetSolid(color);
            }

            if (font != null)
            {
                cell.Font.Color = font;
            }

            if (isBold)
            {
                cell.Font.Weight = ExcelFont.BoldWeight;
            }
        }

        /// <summary>
        /// Format Exist Cell Text.
        /// </summary>
        /// <param name="cellStyle">Cell Style.</param>
        /// <param name="backgroundColor">Background Color.</param>
        /// <param name="isBold">Is Bold font.</param>
        public static void FormatCellText(this CellStyle cellStyle, Color backgroundColor, bool isBold)
        {
            cellStyle.FormatCellText(backgroundColor, isBold, HorizontalAlignmentStyle.General);
        }

        public static void FormatCellText(this CellStyle cellStyle, Color backgroundColor, bool isBold, HorizontalAlignmentStyle textAlign)
        {
            cellStyle.FormatCellText(backgroundColor, isBold, textAlign, false, Color.Black);
        }

        /// <summary>
        ///     Format cell text.
        /// </summary>
        /// <param name="cell">Cell Style.</param>
        /// <param name="color">Color style.</param>
        /// <param name="isBold">Font is bold.</param>
        /// <param name="text">Text style.</param>
        /// <param name="isItalic">Font is Italic.</param>
        /// <param name="font">Font name.</param>
        public static void FormatCellText(this CellStyle cell, Color color, bool isBold, HorizontalAlignmentStyle text, bool isItalic, Color font)
        {
            cell.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            cell.Font.Name = FontName;
            cell.Font.Size = FontSize;
            if (color != null)
            {
                cell.FillPattern.SetSolid(color);
            }

            cell.VerticalAlignment = VerticalAlignmentStyle.Center;
            cell.HorizontalAlignment = text;
            if (isBold)
            {
                cell.Font.Weight = ExcelFont.BoldWeight;
            }

            if (isItalic)
            {
                cell.Font.Italic = true;
            }

            if (font != null)
            {
                cell.Font.Color = font;
            }
        }

        /// <summary>
        /// Format exist Title Cell.
        /// </summary>
        /// <param name="cellStyle">Cell Style.</param>
        /// <param name="backgroundColor">Background Color.</param>
        public static void FormatTitleCell(this CellStyle cellStyle, Color backgroundColor)
        {
            cellStyle.FormatCellText(backgroundColor, true);
            cellStyle.Font.Color = Color.White;
            cellStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            cellStyle.WrapText = true;
        }

        /// <summary>
        /// Get Style Of Cell Number.
        /// </summary>
        /// <param name="symbol">Symbol input.</param>
        /// <param name="backgroundColor">Background Color.</param>
        /// <param name="isBold">Is Bold font.</param>
        /// <param name="isPositive">Is Positive.</param>
        /// <param name="isNegative">Is Negative.</param>
        /// <returns>Cell Style.</returns>
        public static CellStyle GetCellNumberStyle(string symbol, Color backgroundColor, bool isBold, bool isPositive, bool isNegative)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatCellNumberStyle(symbol, backgroundColor, isBold, isPositive, isNegative);
            return cellStyle;
        }

        public static CellStyle GetCellNumberStyle(string symbol, Color color, Color fontColor, bool isBold, bool isPossitive, bool isNegative)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatCellNumberStyle(symbol, color, fontColor, isBold, isPossitive, isNegative);
            return cellStyle;
        }

        public static CellStyle GetCellTextStyle(Color backgroundColor, bool isBold)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatCellText(backgroundColor, isBold);
            return cellStyle;
        }

        public static CellStyle GetCellTextStyle(Color color, bool isBold, HorizontalAlignmentStyle textAlign, bool isItalic, Color fontColor)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatCellText(color, isBold, textAlign, isItalic, fontColor);
            return cellStyle;
        }

        public static CellStyle GetCellTextStyle(Color backgroundColor, bool isBold, HorizontalAlignmentStyle textAlign)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatCellText(backgroundColor, isBold, textAlign);
            return cellStyle;
        }

        /// <summary>
        /// Get Style of Title Cell.
        /// </summary>
        /// <param name="backgroundColor">Background Color.</param>
        /// <returns>Cell Style.</returns>
        public static CellStyle GetCellTitleStyle(Color backgroundColor)
        {
            CellStyle cellStyle = new CellStyle();
            cellStyle.FormatTitleCell(backgroundColor);
            return cellStyle;
        }

        /// <summary>
        /// Response Stream for export excel file.
        /// </summary>
        /// <param name="excelFile">Excel File.</param>
        /// <param name="reportName">Report Name.</param>
        public static void ReponseStream(this ExcelFile excelFile, string reportName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
        }
    }
}