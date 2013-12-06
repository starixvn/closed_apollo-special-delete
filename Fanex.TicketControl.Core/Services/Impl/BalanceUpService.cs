// ----------------------------------------------------------------------
// <copyright file="BalanceUpService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/09/27">Implement function get special date.</change>
// </history>

namespace Fanex.TicketControl.Core.Services.Impl
{
    using System;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Services.Interface;

    /// <summary>
    /// Cache manager.
    /// </summary>
    public class BalanceUpService : ServiceBase, IBalanceUpService
    {
        protected ICache CacheManager
        {
            get
            {
                return CacheService.CreateInstance(CacheType.MemoryCache);
            }
        }

        private dynamic SpecialDays
        {
            get
            {
                var specialDays = CacheManager.Get<dynamic>(CacheServiceKey.BalanceUpdayKey);
                if (specialDays == null)
                {
                    specialDays = this.GetListData<dynamic>(new BalanceUpCriteria()).Single();
                    CacheManager.Set<dynamic>(CacheServiceKey.BalanceUpdayKey, specialDays, DateTime.Now.AddMinutes(5));
                }

                return specialDays;
            }
        }

        public DateTime GetBalanceUpDay()
        {
            return SpecialDays.BalanceUpDay;
        }

        public DateTime MoveBetsDay()
        {
            return SpecialDays.MoveBetsDay;
        }
    }
}