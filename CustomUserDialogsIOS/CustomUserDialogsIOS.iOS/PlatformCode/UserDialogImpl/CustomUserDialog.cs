using System;
using System.Collections.Generic;
using Shared.Interfaces;
using Acr.UserDialogs;
using System;
using System.Linq;
using System.Text;
using UIKit;
using CoreGraphics;
using Foundation;
using Acr.Support.iOS;
using Splat;
using Xamarin.Forms;
using CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl;
using CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers;
using TTG;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(CustomUserDialog))]
namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl
{
    public class CustomUserDialog : ICustomUserDialog
    {
        private UIViewController TopViewController { get; }

        static CustomUserDialog()
        {
            UserDialogController();
        }

        static void UserDialogController()
        {
            BaseUserDialogController.SeparatorColor = UIColor.DarkGray;
            BaseUserDialogController.BackColor = UIColor.White;

            BaseUserDialogController.ButtonRowHeight = 50;
            BaseUserDialogController.RowHeight = 60;
            BaseUserDialogController.TitleRowHeight = 50;

            BaseUserDialogController.TitleTextSize = 20;
            BaseUserDialogController.TextSize = 18;
            BaseUserDialogController.ButtonTextSize = 19;

            BaseUserDialogController.ButtonTextUiColor = UIColor.Red;
            BaseUserDialogController.MainTextUiColor = UIColor.DarkGray;
            BaseUserDialogController.TitleTextUiColor = UIColor.Brown;
        }

        public CustomUserDialog() { TopViewController = UIApplication.SharedApplication.GetTopViewController(); }

        public void Prompt(PromptConfig config)
        {
            var popup = new PromptController(TopViewController, config);
            Present(popup);
        }

        public void ActionSheet(ActionSheetConfig config)
        {
            var popup = new ActionSheetController(TopViewController, config);
            Present(popup);
        }

        public void Confirm(ConfirmConfig config)
        {
            var popup = new ConfirmController(TopViewController, config);
            Present(popup);
        }

        public void Toast(ToastConfig config)
        {
            var toast = new TTG.TTGSnackbar();
            toast.Message = config.Message;
            toast.Duration = config.Duration;
            toast.AnimationType = TTGSnackbarAnimationType.FadeInFadeOut;

            if (config.BackgroundColor != null)
                toast.BackgroundColor = config.BackgroundColor.Value.ToNative();

            if (config.MessageTextColor != null)
                toast.MessageLabel.TextColor = config.MessageTextColor.Value.ToNative();

            toast.MessageLabel.TextAlignment = UITextAlignment.Center;
            toast.LeftMargin = 30; 
            toast.RightMargin = 30;
            toast.Show();
        }

        private void Present(BaseUserDialogController popup)
        {
            //var background = new BackgroundViewContainer();
            //background.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            //background.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            //TopViewController.PresentViewController(background, false, () => { });

            popup.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            TopViewController.PresentViewController(popup, true, popup.FadeInBackground);
        }
    }
}
