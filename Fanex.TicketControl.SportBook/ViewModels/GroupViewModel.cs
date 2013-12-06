// -----------------------------------------------------------------------
// <copyright file="GroupViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.13">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Dynamic Group View model.
    /// </summary>
    /// <typeparam name="T">Dynamic type.</typeparam>
    public class GroupViewModel<T>
    {
        public GroupViewModel()
        {
            Tickets = new List<T>();
            Name = string.Empty;
        }

        public string Name { get; set; }

        public List<T> Tickets { get; set; }
    }
}