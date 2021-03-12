﻿using Couple.Client.Model.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class ToDoStateContainer : Notifier
    {
        private List<ToDoModel> _toDos = new();
        private Dictionary<string, List<ToDoModel>> _categoryToToDos = new();
        private Dictionary<Guid, ToDoModel> _idToToDo = new();
        private List<string> _categories = new();

        public List<ToDoModel> ToDos
        {
            get => _toDos;
            set
            {
                _toDos = value.ToList();
                var categoryToToDos = value
                    .GroupBy(toDo => toDo.Category)
                    .ToDictionary(toDo => toDo.Key, toDo => toDo.ToList());
                CategoryToToDos = categoryToToDos;
                _idToToDo = value.ToDictionary(toDo => toDo.Id);
                NotifyStateChanged();
            }
        }

        private Dictionary<string, List<ToDoModel>> CategoryToToDos
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
            if (!_idToToDo.TryGetValue(id, out toDo))
            {
                return false;
            }

            return true;
        }
    }
}
