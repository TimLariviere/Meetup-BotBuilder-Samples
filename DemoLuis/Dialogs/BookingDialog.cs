using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DemoLuis.Dialogs
{
    [Serializable]
    [LuisModel("4911f709-4d08-40ad-9598-f0d99abf29dd", "4e8286bcd3134a658bc3ab7944982a58")]
    public class BookingDialog : LuisDialog<BookInfo>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Je ne reconnais pas ce que vous dites");
        }
        
        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Bonjour ! Que puis-je pour vous ?");
        }

        [LuisIntent("BookHotel")]
        public Task BookHotel(IDialogContext context, LuisResult result)
        {
            var location = result.Entities.Where(e => e.Type == "location").Select(e => e.Entity).FirstOrDefault();
            var dateStr = result.Entities.Where(e => e.Type == "builtin.datetime.date").SelectMany(e => e.Resolution).Select(e => e.Value).FirstOrDefault() as string;

            DateTime.TryParse(dateStr, out DateTime date);

            var bookInfo = new BookInfo()
            {
                Location = location,
                Date = date
            };

            context.Done(bookInfo);
            return Task.CompletedTask;
        }
    }
}