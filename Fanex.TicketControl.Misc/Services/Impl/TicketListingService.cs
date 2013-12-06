// --------------------------------------------------------------------------
// <copyright file="TicketListingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.2.4">Created</change>
// </history>
namespace Fanex.TicketControl.Misc.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using ExcelUtil.Spreadsheet;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Extensions;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.Entities;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.Misc.Services.Interface;
    using Fanex.TicketControl.Misc.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingService : ServiceBase, ITicketListingService
    {
        private readonly ICoreService _coreService;
        private readonly TicketListingRepository _ticketListingRepository;

        public TicketListingService()
        {
            _ticketListingRepository = new TicketListingRepository();
            _coreService = new CoreService();
        }

        /// <summary>
        /// Export Excel ticket listing.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        public void ExportTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            try
            {
                // Get data for report
                var ticketListing = this.GetTicketListingBetList(criteria);

                // Prepare excel file and sheetname
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string sheetName = "Tickets Listing";
                ExcelWorksheet workSheet = worksheets.Add(sheetName);
                int colNumber = 11;
                int currentRow = 0;

                this.InitExportExcel(ref workSheet, ref currentRow, criteria);

                if (ticketListing.TotalItem == 0)
                {
                    CellRange eachRowRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, colNumber - 1);
                    eachRowRange.Merged = true;
                    eachRowRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    eachRowRange.Value = Labels.There_is_no_data;
                }
                else
                {
                    this.RenderBetList(ref workSheet, ref currentRow, ref ticketListing);
                }

                // Finally, define report file name
                var reportName = string.Format("Ticket Listing for Bettype {0}", criteria.BetTypeName);
                excelFile.ReponseStream(reportName.Replace(' ', '_'));
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }
        }

        /// <summary>
        /// Ticket listing.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return view model.</returns>
        public TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria)
        {
            return _ticketListingRepository.GetTicketListing(criteria);
        }

        /// <summary>
        /// Detail ticket listing.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return ticket detail.</returns>
        public TicketListingBetListModel GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            DataSet dataset = _ticketListingRepository.GetTicketListingBetList(criteria);
            var ticketListingBetList = new TicketListingBetListModel
            {
                Items = new List<TicketListingDetail>(),
                TotalItem = 0
            };

            if (dataset != null && dataset.Tables != null && dataset.Tables.Count > 1)
            {
                var singleTicketRowList = dataset.Tables[0].AsEnumerable(); // single ticket result
                var mixParlayTicketRowList = dataset.Tables[1].AsEnumerable(); // mix parlay ticket result

                ITicketHelper ticketHelper = new TicketHelper();
                ticketHelper.ShowBetType = false;
                ticketHelper.ShowLeague = false;
                ticketHelper.ShowMatch = false;

                this.RenderSingleTicket(singleTicketRowList, ref ticketListingBetList, ref ticketHelper);

                // Need to show bettype in cases: Mix Parlay, System Parlay
                ticketHelper.ShowBetType = true;
                this.RenderMixParlayTicket(mixParlayTicketRowList, ref ticketListingBetList, ref ticketHelper);
            }

            ticketListingBetList.TotalItem = ticketListingBetList.Items.Count;
            ticketListingBetList.Items = ticketListingBetList.Items.OrderBy(t => t.Ticket.TransId).ToList();

            return ticketListingBetList;
        }

        /// <summary>
        /// Mapping ticket.
        /// </summary>
        /// <param name="row">Current row.</param>
        /// <returns>Return ticket.</returns>
        private static Ticket MapTicket(DataRow row)
        {
            var ticket = new Ticket
            {
                UserName = Converter.ToString2(row["username"]),
                TransId = Converter.ToInt(row["transid"]),
                TransDate = Converter.ToDateTime(row["transdate"]),
                Odds = Converter.ToDecimal(row["odds"]),
                Stake = Converter.ToDecimal(row["stake"]),
                Status = Converter.ToString2(row["status"]),
                HomeId = Converter.ToInt(row["homeID"]),
                AwayId = Converter.ToInt(row["awayID"]),
                BetTeam = Converter.ToString2(row["betteam"]),
                BetTypeId = Converter.ToInt(row["bettype"]),
                Handicap1 = Converter.ToDecimal(row["hdp1"]),
                Handicap2 = Converter.ToDecimal(row["hdp2"]),
                LiveHomeScore = Converter.ToInt(row["livehomescore"]),
                LiveAwayScore = Converter.ToInt(row["liveawayscore"]),
                IsLive = Converter.ToBool(row["liveindicator"]),
                IsNeutral = Converter.ToBool(row["isNeutral"]),
                OddsType = Converter.ToString2(row["OddsType"]),
                Winlost = Converter.ToDecimal(row["winlost"]),
            };

            long transid;
            bool result = long.TryParse(row["transid"].ToString(), out transid);
            ticket.TransId = transid;
            return ticket;
        }

        /// <summary>
        /// Used to parse Choice html
        /// and get some info which serve for export excel.
        /// </summary>
        /// <param name="ticketDetail">Ticket detail.</param>
        /// <returns>Return value.</returns>
        private string GetChoiceInfo(TicketListingDetail ticketDetail)
        {
            var choiceHtml = ticketDetail.ChoiceHtml;
            var betTeam = string.Empty;
            var handicap = string.Empty;
            var liveScore = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            choiceHtml = choiceHtml.Replace("&nbsp;", " ");
            xmlDoc.LoadXml(choiceHtml);
            var foundSpanNodes = xmlDoc.SelectNodes("//span");
            if (foundSpanNodes.Count > 0)
            {
                var betChoiceNode = foundSpanNodes[0];
                this.GetChoiceInfoFromXml(ref betTeam, ref handicap, ref liveScore, betChoiceNode);
            }

            string choice = string.Empty;
            if (!string.IsNullOrEmpty(betTeam))
            {
                choice += betTeam;
            }

            if (!string.IsNullOrEmpty(handicap))
            {
                choice += " " + handicap;
            }

            if (!string.IsNullOrEmpty(liveScore))
            {
                choice += " " + liveScore;
            }

            if (ticketDetail.IsMixParlay)
            {
                choice += "\n" + ticketDetail.ParlayBetType;
            }

            return choice;
        }

        /// <summary>
        /// Used to parse Odd html
        /// and get some info which serve for export excel.
        /// </summary>
        /// <param name="ticketDetail">Ticket detail.</param>
        /// <returns>Return value.</returns>
        private string GetOddsInfo(TicketListingDetail ticketDetail)
        {
            var oddsHtml = ticketDetail.OddHtml;
            string oddsValue, oddsType;
            oddsValue = string.Empty;
            oddsType = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            oddsHtml = oddsHtml.Replace("\"", "'");
            oddsHtml = "<div>" + oddsHtml + "</div>";
            xmlDoc.LoadXml(oddsHtml);
            var foundSpanNodes = xmlDoc.SelectNodes("//span");
            var count = foundSpanNodes.Count;
            switch (count)
            {
                case 0:
                    // do nothing
                    break;
                case 1:
                    oddsValue = foundSpanNodes[0].InnerText.Trim();
                    break;
                default:
                    oddsValue = foundSpanNodes[0].InnerText.Trim();
                    oddsType = foundSpanNodes[1].InnerText.Trim();
                    break;
            }

            string odds = oddsValue;

            if (!string.IsNullOrEmpty(oddsType))
            {
                odds += " (" + oddsType + ")";
            }

            return odds;
        }

        /// <summary>
        /// Get combination name.
        /// </summary>
        /// <param name="selection">Bet team name.</param>
        /// <returns>Return string.</returns>
        private string GetSystemParlaySelection(string selection)
        {
            string systemParlaySelection = string.Empty;

            if (!string.IsNullOrEmpty(selection))
            {
                switch (selection)
                {
                    case "110":
                        systemParlaySelection = "Trixie ";
                        break;
                    case "1110":
                        systemParlaySelection = "Yankee";
                        break;
                    case "11110":
                        systemParlaySelection = "Canadian";
                        break;
                    case "111110":
                        systemParlaySelection = "Heinz";
                        break;
                    case "1111110":
                        systemParlaySelection = "Super Heinz";
                        break;
                    case "11111110":
                        systemParlaySelection = "Goliath";
                        break;
                    default:
                        systemParlaySelection = "Trixie!";
                        break;
                }
            }

            return systemParlaySelection;
        }

        /// <summary>
        /// Render single ticket.
        /// </summary>
        /// <param name="singleTicket">Single Ticket.</param>
        /// <param name="ticketListing">Ticket listing.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        private void RenderSingleTicket(
            IEnumerable<DataRow> singleTicket,
            ref TicketListingBetListModel ticketListing,
            ref ITicketHelper ticketHelper)
        {
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = Fanex.BetList.Core.TicketHelper.CacheRef.GetOddsTypeName;

            foreach (var row in singleTicket)
            {
                var ticket = MapTicket(row);
                ticket.BetId = Converter.ToInt(row["betid"]);

                var oddsHtml = builder.BuildOdds(ticket, null, funcGetOddsTypeName);
                var oddName = OddsText.GetOddsName(oddsHtml);
                oddName = string.IsNullOrWhiteSpace(oddName)
                    ? oddsHtml.Replace("<br />", " ")
                    : oddsHtml.Replace("<br />", " (") + ")";

                ticketListing.Items.Add(new TicketListingDetail
                {
                    Ticket = ticket,
                    ParlayTicket = new Ticket(),
                    TransDate = ticket.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                    ChoiceHtml = builder.BuildChoice(ticket, ticketHelper, null, false),
                    OddHtml = oddName,
                    Currency = Converter.ToString2(row["currencyName"]),
                    SAFlag = Converter.ToBool(row["SecAcceptFlag"]),
                    BetType = BetTypeText.GetBetTypeName(ticket.BetTypeId, ticket.IsLive)
                });
            }
        }

        /// <summary>
        /// Render mix parlay ticket.
        /// </summary>
        /// <param name="mixParlayTicketRowList">List Mix Parlay Ticket.</param>
        /// <param name="ticketListing">Ticket listing.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        private void RenderMixParlayTicket(
            IEnumerable<DataRow> mixParlayTicketRowList,
            ref TicketListingBetListModel ticketListing,
            ref ITicketHelper ticketHelper)
        {
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = Fanex.BetList.Core.TicketHelper.CacheRef.GetOddsTypeName;
            foreach (var row in mixParlayTicketRowList)
            {
                var ticket = MapTicket(row);
                var parlayTicket = MapTicket(row);
                parlayTicket.BetTypeId = Converter.ToInt(row["bettypem"]);
                parlayTicket.Odds = Converter.ToDecimal(row["oddsm"]); // Note: Odds is oddsm

                var oldBetTypeId = ticket.BetTypeId;
                ticket.BetTypeId = parlayTicket.BetTypeId;

                var oddsHtml = builder.BuildOdds(ticket, null, funcGetOddsTypeName);
                var oddName = OddsText.GetOddsName(oddsHtml);
                oddName = string.IsNullOrWhiteSpace(oddName)
                    ? oddsHtml.Replace("<br />", " ")
                    : oddsHtml.Replace("<br />", " (") + ")";

                ticketHelper.IsShowOddsWithChoice = true;
                var choiceHtml = builder.BuildChoice(parlayTicket, ticketHelper, null, false);
                ticket.BetTypeId = oldBetTypeId;

                var selection = GetSystemParlaySelection(Converter.ToString2(row["selection"]));

                ticketListing.Items.Add(new TicketListingDetail
                {
                    Ticket = ticket,
                    ParlayTicket = parlayTicket,
                    TransDate = ticket.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                    Currency = Converter.ToString2(row["currencyName"]),
                    SAFlag = Converter.ToBool(row["SecAcceptFlag"]),
                    IsMixParlay = Converter.ToInt(row["isMP"]) == 1 ? true : false,
                    Selection = selection,
                    ChoiceHtml = choiceHtml,
                    OddHtml = oddName,
                    BetType = BetTypeText.GetBetTypeName(ticket.BetTypeId, ticket.IsLive),
                    ParlayBetType = BetTypeText.GetBetTypeName(parlayTicket.BetTypeId, parlayTicket.IsLive)
                });
            }
        }

        /// <summary>
        ///     Get choice info.
        /// </summary>
        /// <param name="betTeam">Ref bet team para.</param>
        /// <param name="handicap">Ref handicap para.</param>
        /// <param name="liveScore">Ref live score para.</param>
        /// <param name="betChoiceNode">Ref bet choice para.</param>
        private void GetChoiceInfoFromXml(ref string betTeam, ref string handicap, ref string liveScore, XmlNode betChoiceNode)
        {
            int count = betChoiceNode.ChildNodes.Count;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    betTeam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                    break;
                case 2:
                    betTeam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                    handicap = betChoiceNode.ChildNodes[1].InnerText.Trim();
                    break;
                default:
                    betTeam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                    handicap = betChoiceNode.ChildNodes[1].InnerText.Trim();
                    liveScore = betChoiceNode.ChildNodes[2].InnerText.Trim();
                    break;
            }
        }

        /// <summary>
        /// Export excel.
        /// </summary>
        /// <param name="workSheet">Work sheet.</param>
        /// <param name="currentRow">Current row.</param>
        /// <param name="criteria">Criteria ticket detail.</param>
        private void InitExportExcel(ref ExcelWorksheet workSheet, ref int currentRow, GetTicketsListingDetailCriteria criteria)
        {
            int colNumber = 11;
            int currentColumn = 0;
            CellRange currentCellRange;

            // define the size of each column
            for (int colIndex = 0; colIndex < colNumber; colIndex++)
            {
                workSheet.Columns[colIndex].Width = 100 * ExcelExtensions.FontWidth;
            }

            workSheet.Columns[0].Width = 120 * ExcelExtensions.FontWidth;
            workSheet.Columns[1].Width = 160 * ExcelExtensions.FontWidth;
            workSheet.Columns[2].Width = 160 * ExcelExtensions.FontWidth;
            workSheet.Columns[3].Width = 350 * ExcelExtensions.FontWidth;
            workSheet.Columns[4].Width = 120 * ExcelExtensions.FontWidth;
            workSheet.Columns[8].Width = 150 * ExcelExtensions.FontWidth;

            var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HeaderTitleColor);
            var title = string.Format(Labels.Tickets_Listing_for_MatchID_Bettype, criteria.MatchID, criteria.BetTypeName);

            currentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 1, currentColumn + 5);
            currentCellRange.Merged = true;
            currentCellRange.Value = title;
            currentCellRange.Style = headerTextStyle;
            currentRow += 3;

            this.InitMatch(ref workSheet, ref currentRow, criteria);
            this.InitBetList(ref workSheet, ref currentRow);
        }

        /// <summary>
        /// Export excel Match.
        /// </summary>
        /// <param name="workSheet">Work sheet.</param>
        /// <param name="currentRow">Current row.</param>
        /// <param name="criteria">Criteria ticket.</param>
        private void InitMatch(ref ExcelWorksheet workSheet, ref int currentRow, GetTicketsListingDetailCriteria criteria)
        {
            CellRange currentCellRange;
            ExcelCell currentCell;
            var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HeaderTitleColor);

            // Match Header
            CellRange matchHeaderCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3);
            var headerCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.HeaderTitleColor, true, HorizontalAlignmentStyle.Center);
            currentRow++;

            matchHeaderCellRange.Style = headerTextStyle;
            currentCell = matchHeaderCellRange[0, 0];
            currentCell.Value = Labels.Code;

            currentCellRange = matchHeaderCellRange.GetSubrangeRelative(0, 1, 2, 1);
            currentCellRange.Merged = true;
            currentCellRange.Value = Labels.Match_Event;
            currentCell = matchHeaderCellRange[0, 3];
            currentCell.Value = Labels.Event_Date;

            // Match Content
            CellRange matchContentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3);
            var contentCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.BackgroundColor, false, HorizontalAlignmentStyle.Center);
            currentRow += 2;

            matchContentCellRange.Style = contentCellStyle;
            currentCell = matchContentCellRange[0, 0];
            currentCell.Value = criteria.MatchCode;
            currentCellRange = matchContentCellRange.GetSubrangeRelative(0, 1, 2, 1);
            currentCellRange.Merged = true;
            currentCellRange.Value = string.Format("{0} -vs- {1}", criteria.HomeName, criteria.AwayName);
            currentCell = matchContentCellRange[0, 3];
            currentCell.Value = criteria.EventDate.ToString(FormatKey.ShortDateFormatWithSlash);
        }

        /// <summary>
        /// Export excel bet list.
        /// </summary>
        /// <param name="workSheet">Work sheet.</param>
        /// <param name="currentRow">Current row.</param>
        private void InitBetList(ref ExcelWorksheet workSheet, ref int currentRow)
        {
            ExcelCell currentCell;
            var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HeaderTitleColor);

            // Bet List Header
            CellRange headerTicketCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 10);
            headerTicketCellRange.Style = headerTextStyle;
            currentRow++;

            currentCell = headerTicketCellRange[0, 0];
            currentCell.Value = Labels.BetID;

            currentCell = headerTicketCellRange[0, 1];
            currentCell.Value = Labels.TransDate;

            currentCell = headerTicketCellRange[0, 2];
            currentCell.Value = Labels.Member;

            currentCell = headerTicketCellRange[0, 3];
            currentCell.Value = Labels.Choice;

            currentCell = headerTicketCellRange[0, 4];
            currentCell.Value = Labels.Odds;

            currentCell = headerTicketCellRange[0, 5];
            currentCell.Value = Labels.Stake;

            currentCell = headerTicketCellRange[0, 6];
            currentCell.Value = Labels.WinLoss;

            currentCell = headerTicketCellRange[0, 7];
            currentCell.Value = Labels.Currency;

            currentCell = headerTicketCellRange[0, 8];
            currentCell.Value = Labels.Bettype;

            currentCell = headerTicketCellRange[0, 9];
            currentCell.Value = Labels.Status;

            currentCell = headerTicketCellRange[0, 10];
            currentCell.Value = Labels.SA;
        }

        /// <summary>
        /// Render bet list to export excel.
        /// </summary>
        /// <param name="workSheet">Work sheet.</param>
        /// <param name="currentRow">Current row.</param>
        /// <param name="ticketListing">Ticket listing.</param>
        private void RenderBetList(ref ExcelWorksheet workSheet, ref int currentRow, ref TicketListingBetListModel ticketListing)
        {
            ExcelCell currentCell;
            CellStyle decimalNumberStyle;
            var oddRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.EvenRowColor, false, HorizontalAlignmentStyle.Center);
            var evenRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.EvenRowColor, false, HorizontalAlignmentStyle.Center);
            var numberOddRowStyle = ExcelExtensions.GetCellNumberStyle(string.Empty, ExcelExtensions.EvenRowColor, false, false, true);
            var numberEvenRowStyle = ExcelExtensions.GetCellNumberStyle(string.Empty, ExcelExtensions.EvenRowColor, false, false, true);

            foreach (var ticket in ticketListing.Items)
            {
                CellRange eachRowRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 10);
                if (ticket.IsMixParlay == true)
                {
                    workSheet.Rows[currentRow].Height = workSheet.Rows[currentRow].Height * 2;
                }

                var bettypeID = ticket.Ticket.BetTypeId;
                if (currentRow % 2 == 0)
                {
                    eachRowRange.Style = evenRowStyle;
                    decimalNumberStyle = numberEvenRowStyle;
                }
                else
                {
                    eachRowRange.Style = oddRowStyle;
                    decimalNumberStyle = numberOddRowStyle;
                }

                currentCell = eachRowRange[0, 0];
                currentCell.Value = ticket.Ticket.TransId;
                currentCell = eachRowRange[0, 1];
                currentCell.Value = ticket.TransDate;
                currentCell = eachRowRange[0, 2];
                currentCell.Value = ticket.Ticket.UserName;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                string choice = string.Empty;
                if (bettypeID == 90)
                {
                    var liveScore = !ticket.Ticket.IsLive && ticket.Ticket.BetId.HasValue ? string.Empty : "[" + ticket.Ticket.BetId.Value + "]";
                    choice = string.Format("Group {0}-{1} {2}", ticket.Ticket.Handicap1, ticket.Ticket.BetTeam, liveScore);
                }
                else
                {
                    choice = GetChoiceInfo(ticket);
                }

                this.FormatCell(ref currentCell, ticket, choice, bettypeID, eachRowRange, decimalNumberStyle);
                currentRow++;
            }
        }

        /// <summary>
        /// Format cell.
        /// </summary>
        /// <param name="currentCell">Cell current.</param>
        /// <param name="ticket">Ticket detail.</param>
        /// <param name="choice">Choice value.</param>
        /// <param name="bettypeID">Bet-type id.</param>
        /// <param name="eachRowRange">Cell range.</param>
        /// <param name="decimalNumberStyle">Style number.</param>
        private void FormatCell(
            ref ExcelCell currentCell,
            TicketListingDetail ticket,
            string choice,
            int bettypeID,
            CellRange eachRowRange,
            CellStyle decimalNumberStyle)
        {
            currentCell = eachRowRange[0, 3];
            currentCell.Value = choice; // choice
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;

            currentCell = eachRowRange[0, 4];
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = GetOddsInfo(ticket);

            currentCell = eachRowRange[0, 5];
            currentCell.Style = decimalNumberStyle;
            currentCell.Value = ticket.Ticket.Stake;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            currentCell = eachRowRange[0, 6];
            currentCell.Style = decimalNumberStyle;
            if (!string.IsNullOrEmpty(ticket.AgentWinloss))
            {
                currentCell.Value = ticket.Ticket.Winlost;
            }

            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            currentCell = eachRowRange[0, 7];
            currentCell.Value = ticket.Currency;

            currentCell = eachRowRange[0, 8];
            currentCell.Value = ticket.BetType;

            // ie. System Parlay
            if (bettypeID == 29)
            {
                currentCell.Value = ticket.BetType + " - " + ticket.Selection;
            }

            currentCell = eachRowRange[0, 9];
            currentCell.Value = ticket.Ticket.Status;

            currentCell = eachRowRange[0, 10];
            currentCell.Value = ticket.SAFlag ? "X" : string.Empty;
        }
    }
}