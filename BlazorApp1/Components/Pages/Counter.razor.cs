using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Components.Pages;

public partial class Counter
{
    private int currentCount = 0;

    [Parameter]
    public int InitialValue {
        get => currentCount;
        set => currentCount = value;
    }
    private void IncrementCount() {
        currentCount++;
    }
}