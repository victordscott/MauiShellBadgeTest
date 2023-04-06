using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiShellBadgeTest.ViewModels
{
    public class AppShellVM : ObservableObject
    {
        private string tab1Count = "1";
        public string Tab1Count
        {
            get => tab1Count;
            set => SetProperty(ref tab1Count, value);
        }

        private string tab2Count = "2";
        public string Tab2Count
        {
            get => tab2Count;
            set => SetProperty(ref tab2Count, value);
        }

        private string tab3Count = "3";
        public string Tab3Count
        {
            get => tab3Count;
            set => SetProperty(ref tab3Count, value);
        }

        void ChangeCounts()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var rnd = new Random();
                    Tab1Count = rnd.Next(1, 50).ToString();
                    Tab2Count = rnd.Next(1, 50).ToString();
                    Tab3Count = rnd.Next(1, 50).ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            });
        }

        public AppShellVM()
        {
            try
            {
                var timer = Application.Current.Dispatcher.CreateTimer();
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += (s, e) => ChangeCounts();
                timer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
