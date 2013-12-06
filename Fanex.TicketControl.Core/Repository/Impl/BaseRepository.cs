// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.22">Created</change>
//   <change who="Ricky.Nguyen" date="2012.12.11">Update Get Data Function</change>
// </history>
namespace Fanex.TicketControl.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRepository : DynamicRepository
    {
        /// <summary>
        ///     Fetch Data from Criteria.
        /// </summary>
        /// <typeparam name="T">Class input.</typeparam>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="mapObjectFunction">Read data reader and pass object.</param>
        /// <returns>Collection of object.</returns>
        public virtual IList<T> Fetch<T>(ICriteria criteria, Func<IDataReader, T> mapObjectFunction)
            where T : class
        {
            IList<T> listObject = new List<T>();
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (IDataReader reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            listObject.Add(mapObjectFunction(reader));
                        }
                    }
                }
            }

            return listObject;
        }

        #region Helpers

        /// <summary>
        /// Get result info.
        /// </summary>
        /// <param name="db">Object database.</param>
        /// <returns>Return result.</returns>
        public Result GetResultInfo(ObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            // get result message
            var result = new Result
            {
                ErrorCode = Converter.ToInt(errorCode),
                ErrorMsg = Converter.ToString2(errorMessage)
            };

            return result;
        }

        #endregion Helpers
    }
}