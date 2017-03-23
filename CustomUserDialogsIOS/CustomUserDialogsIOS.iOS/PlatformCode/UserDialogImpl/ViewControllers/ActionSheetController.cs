using System;
using Acr.UserDialogs;
using CoreGraphics;
using Splat;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class ActionSheetController : BaseUserDialogController
    {
        private readonly ActionSheetConfig _config;

        public ActionSheetController(UIViewController viewController, ActionSheetConfig config) : base(viewController)
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

            //float distanceX = 2;
            //float distanceY = 2;
            //sheet.Layer.ShadowColor = UIColor.Black.CGColor;
            //sheet.Layer.ShadowOffset = new CGSize(distanceX, distanceY);
            //sheet.Layer.ShadowOpacity = 1.0f;

            //sheet.AddArrangedSubview(CreateSeparator());
            foreach (var option in _config.Options)
            {
                var view = CreateActionItem(option);
                sheet.AddArrangedSubview(view);
                view.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;
                sheet.AddConstraint(NSLayoutConstraint.Create(sheet, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, view, NSLayoutAttribute.Leading, 1, 0));
                sheet.AddConstraint(NSLayoutConstraint.Create(sheet, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, view, NSLayoutAttribute.Trailing, 1, 0));

                sheet.AddArrangedSubview(CreateSeparator());
            }

            

            var cancel = CreateCancelView(_config.Cancel);
            sheet.AddArrangedSubview(cancel);
            sheet.AddConstraint(NSLayoutConstraint.Create(sheet, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, cancel, NSLayoutAttribute.Leading, 1, 0));
            sheet.AddConstraint(NSLayoutConstraint.Create(sheet, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, cancel, NSLayoutAttribute.Trailing, 1, 0));

            this.View.AddSubview(sheet);

            this.View.AddConstraint(NSLayoutConstraint.Create( this.View,NSLayoutAttribute.Bottom, NSLayoutRelation.Equal,sheet,  NSLayoutAttribute.Bottom, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create( this.View, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Leading, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create( this.View, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, sheet, NSLayoutAttribute.Trailing, 1, 0));
        }
       
        private UIView CreateActionItem(ActionSheetOption option)
        {
            var view = new UIStackView
            {
                Alignment = UIStackViewAlignment.Center,
                AutoresizingMask = UIViewAutoresizing.All,
                Distribution = UIStackViewDistribution.Fill,
                Axis = UILayoutConstraintAxis.Horizontal
            };

            var tap = new UITapGestureRecognizer(() =>
            {
                try
                {
                    option.Action();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("ActionSheet in action (" + option.Text + ") exception: " + e);
                }
                finally
                {
                    this.DismissController();
                }
            });
            view.AddGestureRecognizer(tap);

            if (option.ItemIcon != null)
            {
                var image = UIImage.FromImage(option.ItemIcon.ToNative().CGImage);

                var imageView = new UIImageView
                {
                    Image = image,
                    BackgroundColor = UIColor.Clear,
                    ContentMode = UIViewContentMode.Center,
                    ContentScaleFactor = 2
                };

                imageView.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;
                imageView.WidthAnchor.ConstraintEqualTo(RowHeight).Active = true;

                view.AddArrangedSubview(imageView);
            }

            var textView = new UILabel
            {
                Text = option.Text,
                BackgroundColor = UIColor.Clear,
                TextColor = MainTextUiColor,
                TextAlignment = UITextAlignment.Left
            };
            textView.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;

            view.AddArrangedSubview(textView);

            return view;
        }

        private UIView CreateCancelView(ActionSheetOption option = null)
        {
            var button = new UIButton(UIButtonType.Custom)
            {
                BackgroundColor = UIColor.Clear,
                AutoresizingMask = UIViewAutoresizing.All,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Right,
                TitleEdgeInsets = new UIEdgeInsets(0,0,0,30)
            };

            button.SetTitle(string.IsNullOrEmpty(option?.Text) ? "Cancel" : option.Text, UIControlState.Normal);
            button.SetTitleColor(ButtonTextUiColor, UIControlState.Normal);
            button.AddTarget((x, y) =>
            {
                try
                {
                    option?.Action?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    DismissController();
                }
            }, UIControlEvent.TouchUpInside);

            button.HeightAnchor.ConstraintEqualTo(RowHeight).Active = true;

            return button;
        }
    }
}
