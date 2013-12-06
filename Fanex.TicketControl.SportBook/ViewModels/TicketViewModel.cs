// -----------------------------------------------------------------------
// <copyright file="TicketViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.13">Created</change>
// </history>
namespace Fanex.TicketControl.SportBook.ViewModels
{
    using Core.DTO;

    /// <summary>
    /// Dynamic Ticket View model.
    /// </summary>
    /// <typeparam name="T">Dynamic type.</typeparam>
    public class TicketViewModel<T>
    {
        public TicketViewModel()
        {
            Match = new Match();
            Tickets = new GroupViewModel<T>();
            Result = new Result();
        }

        public Match Match { get; set; }

        public GroupViewModel<T> Tickets { get; set; }

        public Result Result { get; set; }
    }
}