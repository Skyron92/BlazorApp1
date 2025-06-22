namespace BlazorApp1.Components.Layout;

public partial class NavMenu {
    private struct NavMenuItem {
        public NavMenuItem(string label, string route, string icon) {
            Label = label;
            Route = route;
            Icon = icon;
        }
        public string Label { get; }
        public string Route { get; }
        public string Icon { get; }
    }

    private NavMenuItem[] NavMenuItems = [
        new NavMenuItem("Home", "","bi-house-door-fill-nav-menu"),
        new NavMenuItem("Counter", "counter","bi-plus-square-fill-nav-menu"),
        new NavMenuItem("Weather", "weather","bi-list-nested-nav-menu"),
        new NavMenuItem("Magic number", "magicNumber","bi-plus-square-fill-nav-menu"),
        new NavMenuItem("Drag and drop", "dragAndDrop","bi-list-nested-nav-menu"),
        new NavMenuItem("Protected area", "protected","bi-list-nested-nav-menu")
    ];
}