using MauiShellBadgeTest.ViewModels;

namespace MauiShellBadgeTest;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        this.BindingContext = new AppShellVM();
    }
}
