using BlazorState;
using Couple.Client.Data;
using Couple.Client.Data.ToDo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Couple.Client.States.ToDo
{
    public partial class ToDoDataState : State<ToDoDataState>
    {
        private Dictionary<string, List<ToDoModel>> _categoryToToDos;
        public Dictionary<string, List<ToDoModel>> CategoryToToDos
        {
            get
            {
                var toReturn = new Dictionary<string, List<ToDoModel>>(_categoryToToDos);
                foreach (var key in toReturn.Keys)
                {
                    toReturn[key] = toReturn[key].ToList();
                }
                return toReturn;
            }
            set
            {
                _categoryToToDos = value;
                Categories = value
                    .Keys
                    .OrderBy(category => category)
                    .ToList();
            }
        }
        private Dictionary<Guid, ToDoModel> IdToToDo { get; set; }

        private List<string> _categories;
        public List<string> Categories
        {
            get => _categories.ToList();
            private set => _categories = value;
        }

        public bool TryGetToDos(string category, out List<ToDoModel> toDos)
        {
            if (category == null)
            {
                toDos = null;
                return false;
            }

            if (!CategoryToToDos.TryGetValue(category, out var originalList))
            {
                toDos = null;
                return false;
            }

            toDos = originalList.ToList();
            return true;
        }

        public bool TryGetToDo(Guid id, out ToDoModel toDo)
        {
            if (!IdToToDo.TryGetValue(id, out toDo))
            {
                return false;
            }

            return true;
        }

        public List<ToDoModel> GetToDos() => CategoryToToDos
            .Values
            .SelectMany(toDos => toDos)
            .ToList();

        public override void Initialize() { }

        public class RefreshToDosAction : IAction
        {
            public LocalStore LocalStore { get; }

            public RefreshToDosAction(LocalStore localStore) => (LocalStore) = (localStore);
        }

        public class RefreshToDosHandler : ActionHandler<RefreshToDosAction>
        {
            private ToDoDataState ToDoState => Store.GetState<ToDoDataState>();

            public RefreshToDosHandler(IStore store) : base(store) { }

            public override async Task<Unit> Handle(RefreshToDosAction refreshToDosAction, CancellationToken cancellationToken)
            {
                var toDos = await refreshToDosAction.LocalStore.GetAllAsync<List<ToDoModel>>("todo");

                var categoryToToDos = toDos
                    .GroupBy(toDo => toDo.Category)
                    .ToDictionary(toDo => toDo.Key, toDo => toDo.ToList());
                ToDoState.CategoryToToDos = categoryToToDos;
                ToDoState.IdToToDo = toDos.ToDictionary(toDo => toDo.Id);

                return Unit.Value;
            }
        }
    }
}
