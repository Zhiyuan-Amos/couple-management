using Couple.Shared.Model.User;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.User
{
    public partial class Profile
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        protected string Email { get; set; }
        protected bool IsModalVisible { get; set; }

        protected async Task Confirm()
        {
            IsModalVisible = false;
            await HttpClient.PostAsJsonAsync($"api/Pair", new PairDto { EmailAddress = Email, });
        }
    }
}
