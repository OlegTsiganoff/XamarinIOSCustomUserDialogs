using System;
using Acr.UserDialogs;

namespace Shared.Interfaces
{
    public interface ICustomUserDialog
    {
        void Prompt(PromptConfig config);
        void ActionSheet(ActionSheetConfig config);
        void Confirm(ConfirmConfig config);
        void Toast(ToastConfig config);
    }
}
