using Dynamo.Wpf.Extensions;
using System.Windows.Controls;


namespace Xmas
{
    public class XmasViewExtension : IViewExtension
    {

        internal MenuItem XmasTab;
        internal MenuItem XmasTabMenuItem;
        private const string extensionName = "XmasViewExtension";
        internal bool LightSwitched = false;

        private XmasLightService _lightService;

        public static XmasViewExtension Instance { get; set; }
        public string Name => extensionName;
        public string UniqueId => "XMAS-2025-LIGHTS-EXT";

        public void Dispose()
        {
            _lightService?.Stop();
        }

        public void Loaded(ViewLoadedParams viewLoadedParams)
        {
            try
            {
                //Build the Orkestra Menu
                Instance = this;
                _lightService = new XmasLightService(viewLoadedParams);
                XmasTab = new MenuItem { Header = "Xmas2025", IsCheckable = false };
                XmasTabMenuItem = new MenuItem { Header = "LightSwitch", IsCheckable = true };
                XmasTabMenuItem.Click += XmasTabMenuItem_Clicked;
                XmasTab.Items.Add(XmasTabMenuItem);
                viewLoadedParams.dynamoMenu.Items.Add(XmasTab);


            }
            catch (Exception ex)
            {}
        }

        private void XmasTabMenuItem_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (XmasTabMenuItem.IsChecked)
            {
                _lightService.Start();
            }
            else
            {
                _lightService.Stop();
            }
        }


        public void Shutdown()
        {
            Dispose();
        }
        public void Startup(ViewStartupParams p)
        {
        }

    }
}