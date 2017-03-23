using Acr.UserDialogs;

namespace Shared.Interfaces
{
    public interface IUserDialog
    {
        void Prompt(PromptConfig config);
        void ActionSheet(ActionSheetConfig config);
    }
}
