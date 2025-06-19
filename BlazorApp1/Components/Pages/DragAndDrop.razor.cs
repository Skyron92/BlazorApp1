using System.Numerics;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp1.Components.Pages;

public partial class DragAndDrop {
    
    private bool _isDragging;
    private Vector2 _mousePosition;
    private string _buttonStyle = $"position: absolute; top: {0}px; left: {1}px; height: 50px; width: 100px;";
    private Vector2 _buttonSize = new(100, 50); // Width and height of the button
    private void Drag() {
        _isDragging = true;
    }
    
    private void OnMouseOver(MouseEventArgs mouseEventArgs) {
        if(!_isDragging) return;
        _mousePosition = new Vector2((float)mouseEventArgs.ClientX, (float)mouseEventArgs.ClientY);
        _buttonStyle = string.Format("position: absolute; top: {0}px; left: {1}px;", 
            _mousePosition.Y-100, _mousePosition.X-300);
    }

    private void Drop(MouseEventArgs mouseEventArgs) {
        if(mouseEventArgs.Button!=0) return; // Only handle left mouse button
        _isDragging = false;
    }
}