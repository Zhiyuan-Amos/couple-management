using Couple.Client.Data.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class ToDoStateContainer
    {
        private Dictionary<string, List<ToDoModel>> _categoryToToDos;
        private Dictionary<Guid, ToDoModel> IdToToDo;
        private List<string> _categories;

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

        public void SetToDos(List<ToDoModel> toDos)
        {
            var categoryToToDos = toDos
                .GroupBy(toDo => toDo.Category)
                .ToDictionary(toDo => toDo.Key, toDo => toDo.ToList());
            CategoryToToDos = categoryToToDos;
            IdToToDo = toDos.ToDictionary(toDo => toDo.Id);
        }
    }
}
