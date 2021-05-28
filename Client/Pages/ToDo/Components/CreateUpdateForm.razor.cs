using AutoMapper;
using Couple.Client.Model.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CreateUpdateForm
    {
        [Parameter] public Func<Task> OnSaveCallback { get; set; }

        [Inject] protected CreateUpdateToDoStateContainer CreateUpdateToDoStateContainer { get; init; }

        private IReadOnlyList<IReadOnlyInnerViewModel> ToDos { get; set; }

        protected override void OnInitialized()
        {
            ToDos = CreateUpdateToDoStateContainer.ToDos;
        }

        private void OnForChange(For @for) => CreateUpdateToDoStateContainer.For = @for;

        private bool IsAddNewToDoEnabled => ToDos.All(toDo => toDo.Content.Any());

        private void AddNewToDo() => CreateUpdateToDoStateContainer.AddToDo("", false);
        private void SetContent(int index, string content) => CreateUpdateToDoStateContainer.SetContent(index, content);

        private void Save()
        {
            CreateUpdateToDoStateContainer.TrimToDos();
            OnSaveCallback();
        }

        private bool IsSaveEnabled => ToDos.Any(toDo => toDo.Content.Any());
    }
}
