using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CategoryListView
    {
        [Inject] private ToDoStateContainer ToDoStateContainer { get; init; }

        [Parameter] public EventCallback<string> OnClickCallback { get; init; }

        protected List<string> Categories { get; set; }

        protected override void OnInitialized()
        {
            Categories = ToDoStateContainer.Categories;
        }
    }
}
