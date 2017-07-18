using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DemoDialogs.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var text = activity?.Text?.ToLower();

            if (text == "rechercher")
            {
                context.Call(new WikiSearchDialog(), ResumeAfterSearch);
            }
            else if (text.StartsWith("rechercher "))
            {
                var searchText = text.Substring(11);
                var searchMessage = Activity.CreateMessageActivity();
                searchMessage.Text = searchText;

                await context.Forward(new WikiSearchDialog(), ResumeAfterSearch, searchMessage);
            }
            else
            {
                await context.PostAsync("Essayez de dire \"rechercher\"");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterSearch(IDialogContext context, IAwaitable<string> result)
        {
            var selectedArticle = await result;

            if (!string.IsNullOrWhiteSpace(selectedArticle))
            {
                await context.PostAsync($"Vous avez choisi l'article \"{selectedArticle}\".");
            }
            else
            {
                await context.PostAsync("Vous n'avez pas sélectionné d'article");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}