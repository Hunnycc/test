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
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace EMT.Farm
{
    [Plugin("EMT Farm", "YourName", "1.0")]
    public class Plugin : IPlugin
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly MenuSwitcher enableSwitcher;
        private readonly MenuButton updateButton;

        public Plugin()
        {
            var mainMenu = MenuFactory.Create("Plugins", "EMT Farm");
            enableSwitcher = mainMenu.CreateSwitcher("Enable EMT Farm Plugin", false)
                .SetTooltip("Toggle EMT Farm Plugin")
                .OnValueChange += OnEnableSwitcherValueChange;
            updateButton = mainMenu.CreateButton("Update Now", "Send match info immediately")
                .OnClick += OnUpdateButtonClick;
            mainMenu.Attach();
        }

        public void OnLoad()
        {
            Console.WriteLine("EMT Farm Plugin loaded!");
        }

        public void OnUnload()
        {
            Game.OnUpdate -= OnUpdate;
            Console.WriteLine("EMT Farm Plugin unloaded!");
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
            var match = Context.Game?.Match;
            if (match == null) return;

            var matchInfo = new
            {
                match.Id,
                match.GameTime,
                match.State,
                Heroes = match.Heroes.Select(hero => new
                {
                    hero.Id,
                    hero.Name,
                    hero.Level,
                    hero.Health,
                    hero.MaxHealth,
                    hero.Mana,
                    hero.MaxMana,
                    hero.IsAlive,
                    hero.IsStunned,
                    hero.IsSilenced,
                    hero.IsDisarmed,
                    hero.IsMagicImmune,
                    hero.IsHexed,
                    hero.IsMuted,
                    hero.HasDebuff,
                    Position = new { hero.Position.X, hero.Position.Y, hero.Position.Z }
                })
            };

            await SendMatchInfoAsync(matchInfo);
        }

        private async void OnUpdateButtonClick(object sender, EventArgs e)
        {
            await OnUpdate();
        }

        private async Task SendMatchInfoAsync(object data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:6001/matchinfo", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}
