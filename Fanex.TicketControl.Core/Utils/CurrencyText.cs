// ----------------------------------------------------------------------
// <copyright file="CurrencyText.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Constants;
    using DTO;
    using Services.Impl;
    using Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CurrencyText
    {
        /// <summary>
        /// Get Currency name.
        /// </summary>
        /// <param name="id">Currency id.</param>
        /// <returns>Return currency.</returns>
        public static string CurrencyById(int id)
        {
            var currencyName = string.Empty;
            var coreService = new CoreService();
            try
            {
                var currencies = CacheManager.Get<IList<Currency>>(CacheServiceKey.KeyCurrency);
                if (currencies == null)
                {
                    currencies = coreService.GetCurrency();
                    CacheManager.Set(CacheServiceKey.KeyCurrency, currencies);
                }

                currencyName = currencies.Where(s => s.Id == id.ToString(CultureInfo.InvariantCulture)).Select(s => s.Name).FirstOrDefault();
                if (string.IsNullOrEmpty(currencyName))
                {
                    currencyName =
                        coreService.GetCurrency()
                            .Where(s => s.Id == id.ToString(CultureInfo.InvariantCulture))
                            .Select(s => s.Name)
                            .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return currencyName;
        }
    }
}