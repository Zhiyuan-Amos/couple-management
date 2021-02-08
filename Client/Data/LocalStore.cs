using Couple.Client.Data.Calendar;
using Couple.Client.Data.ToDo;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Couple.Client.Data
{
    public class LocalStore
    {
        protected readonly IJSRuntime js;

        public LocalStore(IJSRuntime js) => (this.js) = (js);

        public ValueTask<T> GetAsync<T>(string storeName, object key)
            => js.InvokeAsync<T>("localStore.get", storeName, key);

        public ValueTask<T> GetAllAsync<T>(string storeName)
            => js.InvokeAsync<T>("localStore.getAll", storeName);

        public ValueTask PutAsync<T>(string storeName, T value)
            => js.InvokeVoidAsync("localStore.put", storeName, value);

        public ValueTask DeleteAsync(string storeName, object key)
            => js.InvokeVoidAsync("localStore.delete", storeName, key);

        // Another possible implementation is to pass only the Event (w/ ToDos),
        // then retrieve the existing Event from database & generate set difference.
        // Pro: Impossible for callers of the method to pass the wrong set of model.
        // Con: Additional code to generate set difference and additional database call.
        // I've used this implementation for code simplicity.
        public ValueTask PutEventAsync(EventModel @event, IEnumerable<Guid> added, IEnumerable<ToDoModel> removed)
            => js.InvokeVoidAsync("localStore.putEvent", @event, added, removed);

        public ValueTask DeleteEventAsync(Guid id, IEnumerable<ToDoModel> removed)
            => js.InvokeVoidAsync("localStore.deleteEvent", id, removed);
    }
}

