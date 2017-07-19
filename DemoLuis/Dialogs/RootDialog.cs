using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DemoLuis.Dialogs
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
            await context.Forward(new BookingDialog(), ResumeAfterBooking, activity);
        }

        private async Task ResumeAfterBooking(IDialogContext context, IAwaitable<BookInfo> result)
        {
            var bookInfo = await result;

            await context.PostAsync($"Je vous ai réservé une nuit d'hôtel à {bookInfo.Location} le {bookInfo.Date.ToString("dd/MM/yyyy")}");

            context.Wait(MessageReceivedAsync);
        }
    }
}