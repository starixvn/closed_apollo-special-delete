// -----------------------------------------------------------------------
// <copyright file="HandicapText.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Utils
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HandicapText
    {
        /// <summary>
        /// Handicap text.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <param name="betTeam">Bet team value.</param>
        /// <param name="handicap1">Handicap 1.</param>
        /// <param name="handicap2">Handicap 2.</param>
        /// <returns>Return handicap.</returns>
        public static string GetHandicapText(int betType, string betTeam, decimal handicap1, decimal handicap2)
        {
            switch (betType)
            {
                case 1:
                case 7:
                case 22:
                    var handicap = betTeam == "h" ? handicap2 - handicap1 : handicap1 - handicap2;
                    return handicap.ToString();

                case 3:
                case 8:
                    return handicap1.ToString();
                default:
                    return "--";
            }
        }
    }
}