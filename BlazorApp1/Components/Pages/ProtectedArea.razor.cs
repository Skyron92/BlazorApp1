using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Components.Pages;

public partial class ProtectedArea
{
    private string _userInput="";
    private const string Password = "WxB9!4";
    private string _passwordResponse = "";

    private string PasswordResponse {
        set {
            if (value == "") LoadFakeStore();
            _passwordResponse = value;
        }
    }
    private void ChangePassword(ChangeEventArgs e) {
        _userInput = e.Value?.ToString() ?? string.Empty;
        PasswordResponse = _userInput == Password ? "" : "Invalid password.";
    }

    private void LoadFakeStore() {
        NavigationManager.NavigateTo("/FakeStore");
    }
}