using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace DemoPrompts.Dialogs
{
    [Serializable]
    public class AskNameDialog : IDialog<User>
    {
        private User _user = null;

        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Text(context, ResumeAfterFirstNamePrompt, "Quel est votre prénom ?");
            return Task.CompletedTask;
        }

        private async Task ResumeAfterFirstNamePrompt(IDialogContext context, IAwaitable<string> result)
        {
            if (_user == null)
            {
                _user = new User();
            }

            _user.FirstName = await result;

            PromptDialog.Text(context, ResumeAfterLastNamePrompt, "Quel est votre nom de famille ?");
        }

        private async Task ResumeAfterLastNamePrompt(IDialogContext context, IAwaitable<string> result)
        {
            _user.LastName = await result;
            context.Done(_user);
        }
    }
}