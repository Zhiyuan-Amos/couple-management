using BlazorState;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Couple.Client.States.ToDo
{
    public partial class SelectedCategoryState : State<SelectedCategoryState>
    {
        public string SelectedCategory { get; private set; }

        public override void Initialize() { }

        public class ModifySelectedCategoryAction : IAction
        {
            public string SelectedCategory { get; }

            public ModifySelectedCategoryAction(string selectedCategory) => (SelectedCategory) = (selectedCategory);
        }

        public class ModifySelectedCategoryHandler : ActionHandler<ModifySelectedCategoryAction>
        {
            private SelectedCategoryState SelectedCategoryState => Store.GetState<SelectedCategoryState>();

            public ModifySelectedCategoryHandler(IStore store) : base(store) { }

            public override Task<Unit> Handle(ModifySelectedCategoryAction modifySelectedCategoryAction, CancellationToken cancellationToken)
            {
                SelectedCategoryState.SelectedCategory = modifySelectedCategoryAction.SelectedCategory;
                return Unit.Task;
            }
        }

        public class RefreshSelectedCategoryAction : IAction
        {
        }

        public class RefreshSelectedCategoryHandler : ActionHandler<RefreshSelectedCategoryAction>
        {
            private SelectedCategoryState SelectedCategoryState => Store.GetState<SelectedCategoryState>();
            private ToDoDataState ToDoDataState => Store.GetState<ToDoDataState>();

            public RefreshSelectedCategoryHandler(IStore store) : base(store) { }

            public override Task<Unit> Handle(RefreshSelectedCategoryAction modifySelectedCategoryAction, CancellationToken cancellationToken)
            {
                var selectedCategory = ToDoDataState.Categories.Any() ? ToDoDataState.Categories[0] : "";
                SelectedCategoryState.SelectedCategory = selectedCategory;
                return Unit.Task;
            }
        }
    }
}
