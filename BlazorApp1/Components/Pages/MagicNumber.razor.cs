namespace BlazorApp1.Components.Pages;

public partial class MagicNumber
{
    private int _userInputValue = 0;
    private int _magicNumberValue = 0;
    private const int MagicNumberMinValue = 0;
    private const int MagicNumberMaxValue = 20;

    private int _lifes = 5;

    private int Lifes {
        get => _lifes;
        set => _lifes = value;
    }

    private string _responseToUser = "";

    protected override void OnInitialized() {
        base.OnInitialized();
        var random = new Random();
        _magicNumberValue = random.Next(MagicNumberMinValue, MagicNumberMaxValue);
    }

    private void CheckResponse() {
        if (_userInputValue == _magicNumberValue) {
            Win();
        }
        else {
            Lifes--;
            _responseToUser = $"{_userInputValue} is {(_userInputValue<_magicNumberValue?"lesser":"greater")} than the magic number.";
            if(Lifes <=0) Lose();
        }
    }

    private void Win() {
        _responseToUser =$"Congratulations ! You guessed the magic number : {_magicNumberValue}!";
    }

    private void Lose()
    {
        _responseToUser = "You have no more life, game over...";
    }
}