using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RuS.Application.Features.Discussions.Queries;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Discussion
{
    public partial class DiscussionCard
    {
        [Parameter] public DiscussionResponse Discussion { get; set; }
        [Parameter] public EventCallback<int> OnEditSelection { get; set; }
        [Parameter] public EventCallback<int> OnDeleteSelection { get; set; }

        protected async Task DeleteSelectionChanged(MouseEventArgs e, int tastId)
        {
            await OnDeleteSelection.InvokeAsync(tastId);
        }

        protected async Task EditSelectionChanged(MouseEventArgs e, int id)
        {
            await OnEditSelection.InvokeAsync(id);
        }
    }
}
