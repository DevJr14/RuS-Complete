using Microsoft.AspNetCore.Components;
using RuS.Application.Features.Tasks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Tasks
{
    public partial class TaskCard
    {
        [Parameter]
        public TaskResponse Task { get; set; }
    }
}
