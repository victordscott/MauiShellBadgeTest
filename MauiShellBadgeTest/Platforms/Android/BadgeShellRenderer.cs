using Android.Content;
using MauiShellBadgeTest.Platforms.Android;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiShellBadgeTest.Platforms.Android
{
    /// <summary>
    /// The BadgeShellRenderer is necessary in order to replace the ShellItemRenderer with your own.
    /// </summary>
    public class BadgeShellRenderer : ShellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeShellRenderer"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="Context"/>.</param>
        public BadgeShellRenderer(Context context) : base(context) { }

        /// <summary>
        /// The CreateShellItemRenderer.
        /// </summary>
        /// <param name="shellItem">The shellItem<see cref="ShellItem"/>.</param>
        /// <returns>The <see cref="IShellItemRenderer"/>.</returns>
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem) =>
            new BadgeShellItemRenderer(this);
    }
}
