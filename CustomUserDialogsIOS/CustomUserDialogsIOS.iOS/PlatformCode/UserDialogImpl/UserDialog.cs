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

[assembly: Dependency(typeof(UserDialog))]
namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl
{
    public class UserDialog : IUserDialog
    {

        public void Prompt(PromptConfig config)
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            UITextField txt = null;

            if (config.IsCancellable)
            {
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
                    config.OnAction?.Invoke(new PromptResult(false, txt.Text)
                )));
            }

            var btnOk = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
                config.OnAction?.Invoke(new PromptResult(true, txt.Text)
            ));
            dlg.AddAction(btnOk);

            dlg.AddTextField(x =>
            {
                txt = x;
                this.SetInputType(txt, config.InputType);
                txt.Placeholder = config.Placeholder ?? String.Empty;
                txt.Text = config.Text ?? String.Empty;

                if (config.MaxLength != null)
                {
                    txt.ShouldChangeCharacters = (field, replacePosition, replacement) =>
                    {
                        var updatedText = new StringBuilder(field.Text);
                        updatedText.Remove((int)replacePosition.Location, (int)replacePosition.Length);
                        updatedText.Insert((int)replacePosition.Location, replacement);
                        return updatedText.ToString().Length <= config.MaxLength.Value;
                    };
                }

                if (config.OnTextChanged != null)
                {
                    txt.AddTarget((sender, e) => ValidatePrompt(txt, btnOk, config), UIControlEvent.EditingChanged);
                    ValidatePrompt(txt, btnOk, config);
                }
            });

            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                var top = UIApplication.SharedApplication.GetTopViewController();
                if (dlg.PreferredStyle == UIAlertControllerStyle.ActionSheet && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    var x = top.View.Bounds.Width;
                    var y = top.View.Bounds.Bottom;
                    var rect = new CGRect(x, y, 0, 0);

                    dlg.PopoverPresentationController.SourceView = top.View;
                    dlg.PopoverPresentationController.SourceRect = rect;
                    dlg.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
                }
                top.PresentViewController(dlg, true, null);
            });
            //return new DisposableAction(() =>
            //{
            //    try
            //    {
            //        app.InvokeOnMainThread(() => dlg.DismissViewController(true, null));
            //    }
            //    catch { }
            //});
        }

        public void ActionSheet(ActionSheetConfig config)
        {
            var sheet = UIAlertController.Create(config.Title, config.Message, UIAlertControllerStyle.ActionSheet);

            if (config.Destructive != null)
                this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive);

            config
                .Options
                .ToList()
                .ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default, config.ItemIcon));

            if (config.Cancel != null)
                this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel);

            // present
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                var top = UIApplication.SharedApplication.GetTopViewController();
                if (sheet.PreferredStyle == UIAlertControllerStyle.ActionSheet && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    var x = top.View.Bounds.Width;
                    var y = top.View.Bounds.Bottom;
                    var rect = new CGRect(x, y, 0, 0);

                    sheet.PopoverPresentationController.SourceView = top.View;
                    sheet.PopoverPresentationController.SourceRect = rect;
                    sheet.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
                }
                top.PresentViewController(sheet, true, null);
            });
        }

        static void ValidatePrompt(UITextField txt, UIAlertAction btn, PromptConfig config)
        {
            var args = new PromptTextChangedArgs { Value = txt.Text };
            config.OnTextChanged(args);
            btn.Enabled = args.IsValid;
            if (!txt.Text.Equals(args.Value))
                txt.Text = args.Value;
        }

        protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style, IBitmap image = null)
        {
            var alertAction = UIAlertAction.Create(opt.Text, style, x => opt.Action?.Invoke());

            if (opt.ItemIcon == null && image != null)
                opt.ItemIcon = image;

            if (opt.ItemIcon != null)
                alertAction.SetValueForKey(opt.ItemIcon.ToNative(), new NSString("image"));

            controller.AddAction(alertAction);
        }

        protected virtual void SetInputType(UITextField txt, InputType inputType)
        {
            switch (inputType)
            {
                case InputType.DecimalNumber:
                    txt.KeyboardType = UIKeyboardType.DecimalPad;
                    break;

                case InputType.Email:
                    txt.KeyboardType = UIKeyboardType.EmailAddress;
                    break;

                case InputType.Name:
                    break;

                case InputType.Number:
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.NumericPassword:
                    txt.SecureTextEntry = true;
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.Password:
                    txt.SecureTextEntry = true;
                    break;

                case InputType.Phone:
                    txt.KeyboardType = UIKeyboardType.PhonePad;
                    break;

                case InputType.Url:
                    txt.KeyboardType = UIKeyboardType.Url;
                    break;
            }

        }
    }
}
