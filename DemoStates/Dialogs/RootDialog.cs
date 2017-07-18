using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DemoStates.Dialogs
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
            
            if (context.UserData.TryGetValue("flightInfo", out User existingFlightInfo))
            {
                await context.PostAsync($"Vol trouvé : n°{existingFlightInfo.FlightNumber} - {existingFlightInfo.FirstName} {existingFlightInfo.LastName}");
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                context.Call(new AskFlightInfoDialog(), ResumeAfterAskName);
            }
        }

        private async Task ResumeAfterAskName(IDialogContext context, IAwaitable<User> result)
        {
            var user = await result;

            context.UserData.SetValue("flightInfo", user);

            await context.PostAsync($"{user.FirstName} {user.LastName} - Vol n°{user.FlightNumber}");

            context.Wait(MessageReceivedAsync);
        }
    }
}