using Microsoft.Extensions.Logging;

namespace MauiShellBadgeTest;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-regular-400.ttf", "FAR");
                fonts.AddFont("fa-solid-900.ttf", "FAS");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MIR");
            }).ConfigureMauiHandlers(handlers => {
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(MauiShellBadgeTest.Platforms.Android.BadgeShellRenderer));
#elif IOS
				handlers.AddHandler(typeof(Shell), typeof(MauiShellBadgeTest.Platforms.iOS.BadgeShellRenderer));
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
