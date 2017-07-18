using DemoDialogs.Wiki;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDialogs.Dialogs
{
    [Serializable]
    public class WikiSearchDialog : IDialog<string>
    {
        private string[] _lastSearchItems = null;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var text = activity?.Text?.ToLower();

            if (text.StartsWith("select "))
            {
                var index = int.Parse(text.Substring(7)) - 1;

                if (_lastSearchItems != null && _lastSearchItems.Length > index)
                {
                    context.Done(_lastSearchItems[index]);
                }
                else
                {
                    await context.PostAsync("Sélection impossible");
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                var articles = await WikiService.SearchArticlesAsync(activity.Text);
                _lastSearchItems = articles.ToArray();

                if (_lastSearchItems.Length == 0)
                {
                    await context.PostAsync("Pas de résultat");
                }
                else
                {
                    await context.PostAsync("Voici la liste des articles trouvées :");

                    var message = string.Empty;
                    for (var i = 0; i < _lastSearchItems.Length; i++)
                    {
                        message += $"{i + 1} - {_lastSearchItems[i]}" + "\n\n";
                    }

                    message += "\n\nDites \"select\" suivi du numéro";

                    await context.PostAsync(message);
                }

                context.Wait(MessageReceivedAsync);
            }
        }
    }
}