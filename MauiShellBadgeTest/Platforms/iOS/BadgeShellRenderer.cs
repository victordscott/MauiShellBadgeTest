using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiShellBadgeTest.Platforms.iOS
{
    public class BadgeShellRenderer : ShellRenderer
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item) =>
            new BadgeShellItemRenderer(this) { ShellItem = item };
    }
}
