using DivineSharp;
using DivineSharp.Plugin;
using DivineSharp.Plugin.Context;
using DivineSharp.Plugin.Sdk;
using DivineSharp.Plugin.Sdk.Common.Events;
using DivineSharp.Plugin.Sdk.Common.Models;
using DivineSharp.Plugin.Sdk.UI;
using DivineSharp.Plugin.Sdk.UI.Menu;
using DivineSharp.Plugin.Sdk.UI.Menu.Items;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GSIPostPlugin
{
    [Plugin("GSI Post Plugin", "YourName", "1.0")]
    public class Plugin : IPlugin
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly MenuSwitcher enableSwitcher;

        public Plugin()
        {
            enableSwitcher = MenuFactory.CreateSwitcher("Enable GSI Post Plugin", false).OnValueChange += OnEnableSwitcherValueChange;
        }

        public void OnLoad()
        {
            Console.WriteLine("GSI Post Plugin loaded!");
        }

        public void OnUnload()
        {
            Game.OnUpdate -= OnUpdate;
            Console.WriteLine("GSI Post Plugin unloaded!");
        }

        private void OnEnableSwitcherValueChange(object sender, SwitcherEventArgs e)
        {
            if (e.Value)
            {
                Game.OnUpdate += OnUpdate;
            }
            else
            {
                Game.OnUpdate -= OnUpdate;
            }
        }

        private async void OnUpdate()
        {
            var gsiData = GetGSIData();
            if (gsiData != null)
            {
                await SendGSIDataAsync(gsiData);
            }
        }

        private string GetGSIData()
        {
            // Пример получения данных (замените на реальный код для получения данных)
            return "{ \"example_key\": \"example_value\" }";
        }

        private async Task SendGSIDataAsync(string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5001/gsi", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
