using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using Shared.Interfaces;

namespace CustomUserDialogsIOS.ViewModels
{
    public class MainViewModel : BindableBase
    {

        ICommand _confirmCommand;
        public ICommand ConfirmCommand
        {
            get { return _confirmCommand; }
            set { SetProperty(ref _confirmCommand, value); }
        }

        ICommand _actionCommand;
        public ICommand ActionCommand
        {
            get { return _actionCommand; }
            set { SetProperty(ref _actionCommand, value); }
        }

        ICommand _promptCommand;
        public ICommand PromptCommand
        {
            get { return _promptCommand; }
            set { SetProperty(ref _promptCommand, value); }
        }

        ICommand _toastCommand;
        public ICommand ToastCommand
        {
            get { return _toastCommand; }
            set { SetProperty(ref _toastCommand, value); }
        }

        public MainViewModel()
        {
            ConfirmCommand = new Command(ConfirmAction);
            ActionCommand = new Command(ActionSheetAction);
            PromptCommand = new Command(PromptAction);
            ToastCommand = new Command(ToastAction);
        }


        private void ConfirmAction()
        {
            var config = new ConfirmConfig();
            config.Message = "Are you sure?";
            config.AndroidStyleId = 0;
            config.Title = "Delete all bookmarks";
            config.OnAction = (x) => {};
            config.UseYesNo();
            var dialog = DependencyService.Get<ICustomUserDialog>();
            dialog.Confirm(config);
        }

        async void ActionSheetAction()
        {
            var config = new ActionSheetConfig();
            config.SetCancel("Cancel");

            var searchIcon = await Splat.BitmapLoader.Current.LoadFromResource("ic_search_brown.png", null, null);
            var bookmarkIcon = await Splat.BitmapLoader.Current.LoadFromResource("ic_bookmark_brown.png", null, null);
            var biggerFontIcon = await Splat.BitmapLoader.Current.LoadFromResource("ic_font_bigger.png", null, null);
            var lowerFontIcon = await Splat.BitmapLoader.Current.LoadFromResource("ic_font_lower.png", null, null);

            config.Options.Add(new ActionSheetOption("Search in Text", () => { }, searchIcon));
            config.Options.Add(new ActionSheetOption("Add to Bookmarks", () => { }, bookmarkIcon));
            config.Options.Add(new ActionSheetOption("Make Font Bigger", () => { }, biggerFontIcon));
            config.Options.Add(new ActionSheetOption("Make Font Smaller", () => { }, lowerFontIcon));

            var dialog = DependencyService.Get<ICustomUserDialog>();
            dialog.ActionSheet(config);
        }

        private void PromptAction()
        {
            var config = new PromptConfig();
            config.AndroidStyleId = 0;
            config.SetCancelText("Cancel");
            config.SetOkText("Ok");
            config.SetCancellable(true);
            config.OnAction = (x) => { };
            config.SetOnTextChanged((x) => { });
            config.SetTitle("Add bookmark");
            config.SetPlaceholder("Add bookmark name");
            string text = "My bookmark";
            config.SetText(text);
            config.SetMaxLength(70 + 5);
            config.SetInputMode(InputType.Default);
            

            var dialog = DependencyService.Get<ICustomUserDialog>();
            dialog.Prompt(config);
        }

        void ToastAction()
        {
            var config = new ToastConfig("bookmark deleted");
            var dialog = DependencyService.Get<ICustomUserDialog>();
            dialog.Toast(config);
        }
    }
}
