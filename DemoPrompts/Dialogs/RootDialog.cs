using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DemoPrompts.Dialogs
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
            context.Call(new AskFlightInfoDialog(), ResumeAfterAskName);
        }

        private async Task ResumeAfterAskName(IDialogContext context, IAwaitable<User> result)
        {
            var user = await result;

            await context.PostAsync($"{user.FirstName} {user.LastName} - Vol n°{user.FlightNumber}");

            context.Wait(MessageReceivedAsync);
        }
    }
}