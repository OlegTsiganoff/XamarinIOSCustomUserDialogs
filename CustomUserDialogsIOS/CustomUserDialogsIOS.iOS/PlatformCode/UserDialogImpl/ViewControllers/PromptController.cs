using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class PromptController : BaseUserDialogController
    {
        private readonly PromptConfig _config;
        private UIView _activeview;             // stores active view information
        private float _scrollAmount = 0.0f;    // amount to scroll 
        private float _bottom = 0.0f;           // bottom point
        private readonly float offset = 10.0f;          // extra offset
        private bool _moveViewUp = false;       // which direction are we moving

        private PaddedUITextField _textField;

        public PromptController(UIViewController topController, PromptConfig config) : base(topController)
        {
            _config = config;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardUp);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardDown);
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

            _textField = CreateTextField();
            sheet.AddArrangedSubview(_textField);

            sheet.AddArrangedSubview(CreateSeparator());

            var buttons = CreateOkCancelButtonsView(_config.OnAction, _config.OkText, _config.CancelText);
            sheet.AddArrangedSubview(buttons);
            _activeview = sheet;

            this.View.AddSubview(sheet);
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Bottom, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Leading, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(this.View, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Trailing, 1, 0));
        }


        private void OnKeyboardUp(NSNotification notification)
        {
            var r = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
            foreach (var view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    _activeview = view;
            }

            if (_activeview != null)
            {
                // Bottom of the controller = initial position + height + offset      
                _bottom = ((float)(_activeview.Frame.Y + _activeview.Frame.Height + offset));

                // Calculate how far we need to scroll
                _scrollAmount = ((float)(r.Height - (View.Frame.Size.Height - _bottom)));

                // Perform the scrolling
                if (_scrollAmount > 0)
                {
                    _moveViewUp = true;
                    ScrollTheView(_moveViewUp);
                }
                else
                {
                    _moveViewUp = false;
                }
            }
        }

        private void ScrollTheView(bool move)
        {
            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.2);

            var frame = View.Frame;

            if (move)
            {
                frame.Y -= _scrollAmount;
            }
            else
            {
                frame.Y += _scrollAmount;
                _scrollAmount = 0;
            }

            View.Frame = frame;

            UIView.CommitAnimations();
        }


        private void OnKeyboardDown(NSNotification notification)
        {
            if (_moveViewUp) { ScrollTheView(false); }
        }

        private PaddedUITextField CreateTextField()
        {
            var textField = new PaddedUITextField
            {
                Placeholder = _config.Placeholder,
                Text = _config.Text,
                ShouldChangeCharacters = (textfield, range, replacementString) =>
                {
                    var newLength = textfield.Text.Length + replacementString.Length - range.Length;
                    return (_config.MaxLength == null ? newLength <= TextFieldDefaultMaxLength : newLength <= _config.MaxLength.Value);
                },
                BackgroundColor = UIColor.Clear,
                TextColor = MainTextUiColor,
                Font = UIFont.SystemFontOfSize(TextSize),
                TextAlignment = UITextAlignment.Left
            };
            textField.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;
            textField.Padding = new CGRect(10, 0, 10, 0);

            return textField;
        }

        private UIView CreateOkCancelButtonsView(Action<PromptResult> okAction, string okText = null, string cancelText = null)
        {
            var stack = new UIStackView()
            {
                Alignment = UIStackViewAlignment.Fill,
                AutoresizingMask = UIViewAutoresizing.All,
                Distribution = UIStackViewDistribution.Fill,
                Axis = UILayoutConstraintAxis.Horizontal
            };
            stack.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;

            var okButton = new UIButton {BackgroundColor = UIColor.Clear};
            okButton.SetTitle((string.IsNullOrEmpty(okText) ? "OK" : okText), UIControlState.Normal);
            okButton.SetTitleColor(ButtonTextUiColor, UIControlState.Normal);
            okButton.TitleLabel.Font = UIFont.SystemFontOfSize(ButtonTextSize);
            okButton.AddTarget((x, y) =>
            {
                try
                {
                    var result = new PromptResult(true, _textField.Text);
                    okAction(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    this.DismissController();
                }
                
            }, UIControlEvent.TouchUpInside);
            okButton.WidthAnchor.ConstraintEqualTo(this.View.Frame.Width / 2 - 1).Active = true;

            var separator = new UIView {BackgroundColor = SeparatorColor};
            separator.WidthAnchor.ConstraintEqualTo(1).Active = true;
            //separator.WidthAnchor.ConstraintEqualTo(70).Active = true;


            var cancelButton = new UIButton {BackgroundColor = UIColor.Clear};
            cancelButton.SetTitle((string.IsNullOrEmpty(cancelText) ? "Cancel" : cancelText), UIControlState.Normal);
            cancelButton.SetTitleColor(ButtonTextUiColor, UIControlState.Normal);
            okButton.TitleLabel.Font = UIFont.SystemFontOfSize(ButtonTextSize);
            cancelButton.AddTarget((x, y) => { this.DismissController(); }, UIControlEvent.TouchUpInside);
            okButton.WidthAnchor.ConstraintEqualTo(this.View.Frame.Width / 2).Active = true;

            stack.AddArrangedSubview(okButton);
            stack.AddArrangedSubview(separator);
            stack.AddArrangedSubview(cancelButton);

            return stack;
        }

    }


}