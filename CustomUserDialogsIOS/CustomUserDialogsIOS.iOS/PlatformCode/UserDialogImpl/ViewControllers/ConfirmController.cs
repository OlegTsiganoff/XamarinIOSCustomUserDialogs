using System;
using Acr.UserDialogs;
using CoreGraphics;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class ConfirmController : BaseUserDialogController
    {
        private readonly ConfirmConfig _config;

        public ConfirmController(UIViewController viewController, ConfirmConfig config) : base(viewController)
        {
            _config = config;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitView();
        }

        private void InitView()
        {
            var sheet = new StackView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = BackColor,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.Fill,
                Axis = UILayoutConstraintAxis.Vertical
            };

            var title = CreateTitle(_config.Title);
            sheet.AddArrangedSubview(title);

            sheet.AddArrangedSubview(CreateSeparator());

            var text = CreateText(_config.Message);
            sheet.AddArrangedSubview(text);

            sheet.AddArrangedSubview(CreateSeparator());

            var buttons = CreateOkCancelButtonsView(_config.OnAction, _config.OkText, _config.CancelText);
            sheet.AddArrangedSubview(buttons);

            this.View.AddSubview(sheet);
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Bottom, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Leading, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Trailing, 1, 0));
        }

        private UIView CreateText(string text)
        {
            var textView = new PaddedUILabel
            {
                BackgroundColor = UIColor.Clear,
                Text = text,
                Font = UIFont.SystemFontOfSize(TextSize),
                TextColor = ButtonTextUiColor,
                TextAlignment = UITextAlignment.Left,
                Padding = new CGRect(30, 0, 0, 0)
            };
            textView.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;
            return textView;
        }

        private UIView CreateOkCancelButtonsView(Action<bool> action, string okText = null, string cancelText = null)
        {
            var stack = new UIStackView()
            {
                Alignment = UIStackViewAlignment.Fill,
                AutoresizingMask = UIViewAutoresizing.All,
                Distribution = UIStackViewDistribution.Fill,
                Axis = UILayoutConstraintAxis.Horizontal
            };
            stack.HeightAnchor.ConstraintEqualTo(ButtonRowHeight).Active = true;

            var okButton = new UIButton {BackgroundColor = UIColor.Clear };
            okButton.SetTitle(okText ?? "OK", UIControlState.Normal);
            okButton.SetTitleColor(ButtonTextUiColor, UIControlState.Normal);
            okButton.AddTarget((x, y) =>
            {
                try
                {
                    action(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    DismissController();
                }
                
            }, UIControlEvent.TouchUpInside);
            okButton.WidthAnchor.ConstraintEqualTo(TopController.View.Frame.Width / 2 - 1).Active = true;

            //var separator = new UIView {BackgroundColor = SeparatorColor};
            //separator.WidthAnchor.ConstraintEqualTo(1).Active = true;

            var cancelButton = new UIButton {BackgroundColor = UIColor.Clear};
            cancelButton.SetTitle((string.IsNullOrEmpty(cancelText) ? "Cancel" : cancelText), UIControlState.Normal);
            cancelButton.SetTitleColor(ButtonTextUiColor, UIControlState.Normal);
            cancelButton.AddTarget((x, y) =>
            {
                DismissController();
            }, UIControlEvent.TouchUpInside);
            okButton.WidthAnchor.ConstraintEqualTo(TopController.View.Frame.Width / 2).Active = true;

            stack.AddArrangedSubview(okButton);
            //stack.AddArrangedSubview(separator);
            stack.AddArrangedSubview(cancelButton);

            return stack;
        }
    }
}