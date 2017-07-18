using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DemoCards.Dialogs
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

            if (text == "hero")
            {
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCountryCards();

                await context.PostAsync(reply);
            }

            context.Wait(MessageReceivedAsync);
        }

        private IList<Attachment> GetCountryCards()
        {
            var countries = new List<Attachment>();

            foreach (var country in DataStore.Countries)
            {
                var card = new HeroCard()
                {
                    Title = country.Name,
                    Subtitle = country.Subtitle,
                    Text = country.Description,
                    Images = new[] { new CardImage(country.ImageUrl, country.Name) },
                    Buttons = new[]
                    {
                        new CardAction(ActionTypes.ShowImage, "Voir l'image", value: country.ImageUrl),
                        new CardAction(ActionTypes.OpenUrl, "Plus d'infos", value: $"https://www.google.com/?q={country.Name}"),
                        new CardAction(ActionTypes.ImBack, "Choisir", value: $"Je veux aller à {country.Name}"),
                    }
                };

                countries.Add(card.ToAttachment());
            }

            return countries;
        }
    }
}