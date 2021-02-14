using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Couple.Client.Components.ToDo
{
    public partial class CategoryListView
    {
        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private SelectedCategoryStateContainer SelectedCategoryStateContainer { get; set; }

        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }

        protected List<string> Categories { get; set; }
        protected string SelectedCategory { get; set; }

        protected override void OnInitialized()
        {
            Categories = ToDoStateContainer.Categories;
            SelectedCategory = SelectedCategoryStateContainer.SelectedCategory;
        }
    }
}
