using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Couple.Client.Components.ToDo
{
    public partial class CategoryListView
    {
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }

        protected List<string> Categories { get; set; }
        protected string SelectedCategory { get; set; }

        protected override void OnInitialized()
        {
            Categories = GetState<ToDoDataState>().Categories;
            SelectedCategory = GetState<SelectedCategoryState>().SelectedCategory;
        }
    }
}
