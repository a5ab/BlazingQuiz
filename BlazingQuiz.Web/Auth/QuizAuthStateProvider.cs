using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingQuiz.Web.Auth
{
    public class QuizAuthStateProvider : AuthenticationStateProvider
    {
        private const string authType = "quiz--auth";
        private const string userDataKey = "quiz--auth";
        private  Task<AuthenticationState> _authStateTask;
        private readonly IJSRuntime _jsRuntime;

        public QuizAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            SetAuthStateTask();
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authStateTask;

        public LoggedInUser User { get; private set; }
        public bool isLoggedIn => User?.Id > 0;

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", userDataKey, user.ToJson());



        }

        public bool IsInitialized { get; private set; } = true;
        public async Task InitializeAsync()
        {
            try
            {
                var userDataInLocalStorage = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", userDataKey);
                if (string.IsNullOrEmpty(userDataInLocalStorage))
                {
                    //user data is not there in local storage so no need to do anything
                    return;
                }

                var user = LoggedInUser.FromJson(userDataInLocalStorage);

                if (user == null || user?.Id == 0)
                {
                    //user data is not Valid so we do not need it 
                    return;
                }

                await SetLoginAsync(user);
            }

            finally
            {
                IsInitialized = false;
            }


        }
        public async Task SetLogOutAsync()
        {
            User = null;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", userDataKey);

        }

        private void SetAuthStateTask()
        {
            if (isLoggedIn)
            {
                var identity = new ClaimsIdentity(User.ToClaims(), authType);
                var principle = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principle);
                _authStateTask = Task.FromResult(authState);
            }
            else
            {
                var identity = new ClaimsIdentity();
                var principle = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principle);
                _authStateTask = Task.FromResult(authState);
            }
        }

    }
}
