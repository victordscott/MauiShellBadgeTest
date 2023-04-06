using AsyncAwaitBestPractices;
using CoreFoundation;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace MauiShellBadgeTest.Platforms.iOS
{
    public class BadgeShellItemRenderer : ShellItemRenderer
    {
        static bool? s_isiOS15OrNewer;
        internal static bool IsiOS15OrNewer
        {
            get
            {
                if (!s_isiOS15OrNewer.HasValue)
                    s_isiOS15OrNewer = UIDevice.CurrentDevice.CheckSystemVersion(15, 0);
                return s_isiOS15OrNewer.Value;
            }
        }

        private readonly string[] _applyPropertyNames =
            new string[]
            {
                Badge.TextProperty.PropertyName,
                Badge.TextColorProperty.PropertyName,
                Badge.BackgroundColorProperty.PropertyName
            };

        private readonly Dictionary<Guid, int> _tabRealIndexByItemId =
            new Dictionary<Guid, int>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public BadgeShellItemRenderer(IShellContext context) : base(context) { }

        /// <summary>
        /// Occures when view is about to appear.
        /// </summary>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                Device
                    .InvokeOnMainThreadAsync(InitBadges)
                    .SafeFireAndForget();
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnShellSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnShellSectionPropertyChanged(sender, e);

            if (_applyPropertyNames.All(x => x != e.PropertyName))
                return;

            Device
                .InvokeOnMainThreadAsync(() =>
                {
                    var item = (ShellSection)sender;
                    if (item.IsVisible)
                    {
                        var index = _tabRealIndexByItemId.GetValueOrDefault(item.Id, -1);
                        UpdateBadge(item, index);
                    }
                })
                .SafeFireAndForget();
        }

        private void InitBadges()
        {
            _tabRealIndexByItemId.Clear();
            if (ShellItem?.Items == null)
                return;
            for (int index = 0, filteredIndex = 0; index < ShellItem.Items.Count; index++)
            {
                var item = ShellItem.Items.ElementAtOrDefault(index);
                if (item == null || !item.IsVisible)
                    continue;
                _tabRealIndexByItemId[item.Id] = filteredIndex;
                UpdateBadge(item, filteredIndex);
                filteredIndex++;
            }
        }

        private void UpdateBadge(ShellSection item, int index)
        {
            if (index < 0)
                return;

            var text = Badge.GetText(item);
            var textColor = Badge.GetTextColor(item);
            var bg = Badge.GetBackgroundColor(item);
            ApplyBadge(index, text, bg, textColor);
        }

        private void ApplyBadge(int index, string text, Color bg, Color textColor)
        {
            if (TabBar.Items.Any())
            {
                if (TabBar.Items.ElementAtOrDefault(index) is UITabBarItem currentTabBarItem)
                {
                    int.TryParse(text, out var badgeValue);

                    if (string.IsNullOrEmpty(text))
                    {
                        currentTabBarItem.BadgeValue = default;
                        textColor = Color.FromRgba(255, 255, 255, 0);
                        bg = Color.FromRgba(255, 255, 255, 0);
                    }
                    else if (badgeValue == 0)
                    {
                        currentTabBarItem.BadgeValue = "●";
                        textColor = bg;
                        bg = Color.FromRgba(255, 255, 255, 0);
                    }
                    else
                    {
                        currentTabBarItem.BadgeValue = text;
                    }

                    currentTabBarItem.BadgeColor = bg.ToPlatform();
                    currentTabBarItem.SetBadgeTextAttributes(
                        new UIStringAttributes
                        {
                            ForegroundColor = textColor.ToPlatform()
                        }, UIControlState.Normal);
                }
            }
        }

        protected override void UpdateShellAppearance(ShellAppearance appearance)
        {
            if (IsiOS15OrNewer)
            {
                // base.UpdateShellAppearance(appearance);

                IShellAppearanceElement appearanceElement = appearance;
                var backgroundColor = appearanceElement.EffectiveTabBarBackgroundColor;
                var unselectedColor = appearanceElement.EffectiveTabBarUnselectedColor;
                var titleColor = appearanceElement.EffectiveTabBarTitleColor;

                if (!backgroundColor.IsDefault())
                    TabBar.BackgroundColor = backgroundColor.ToPlatform();
                TabBar.BarTintColor = backgroundColor.ToPlatform();
                if (!titleColor.IsDefault())
                    TabBar.TintColor = titleColor.ToPlatform();

                if (!unselectedColor.IsDefault())
                {
                    TabBar.UnselectedItemTintColor = unselectedColor.ToPlatform();
                }

            }
            else
            {
                base.UpdateShellAppearance(appearance);
            }
        }
    }
}
