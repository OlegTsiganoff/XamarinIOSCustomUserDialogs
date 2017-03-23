using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public abstract class BaseUserDialogController : UIViewController
    {
        protected UIViewController TopController { get; private set; }

        private CAShapeLayer backgroundLayer;

        private static UIColor _mainTextUiColor;
        public static UIColor MainTextUiColor
        {
            get
            {
                if (_mainTextUiColor != null)
                    return _mainTextUiColor;
                return UIColor.Black;
            }
            set { _mainTextUiColor = value; }
        }

        private static UIColor _backColor;
        public static UIColor BackColor
        {
            get
            {
                if (_backColor != null)
                    return _backColor;
                return UIColor.White;
            }
            set { _backColor = value; }
        }

        private static UIColor _buttonTextUiColor;
        public static UIColor ButtonTextUiColor
        {
            get
            {
                if (_buttonTextUiColor != null)
                    return _buttonTextUiColor;
                return UIColor.Black;
            }
            set { _buttonTextUiColor = value; }
        }

        private static UIColor _titleTextUiColor;
        public static UIColor TitleTextUiColor
        {
            get
            {
                if (_titleTextUiColor != null)
                    return _titleTextUiColor;
                return UIColor.Black;
            }
            set { _titleTextUiColor = value; }
        }

        private static UIColor _separatorColor;
        public static UIColor SeparatorColor
        {
            get
            {
                if (_separatorColor != null)
                    return _separatorColor;
                return UIColor.LightGray;
            }
            set { _separatorColor = value; }
        }

        public static float TitleTextSize { get; set; } = 18;
        public static float TextSize { get; set; } = 18;
        public static float ButtonTextSize { get; set; } = 18;
        public static float TitleRowHeight { get; set; } = 50;
        public static float RowHeight { get; set; } = 50;
        public static float ButtonRowHeight { get; set; } = 50;

        protected readonly int TextFieldDefaultMaxLength = 100;

        protected BaseUserDialogController(UIViewController viewController)
        {
            TopController = viewController;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.AutoresizingMask = UIViewAutoresizing.All;
            this.View.BackgroundColor = UIColor.Clear;
            backgroundLayer = new CAShapeLayer { Path = CGPath.FromRect(this.View.Bounds) };
            backgroundLayer.BackgroundColor = UIColor.Black.CGColor;
            backgroundLayer.Opacity = 0;
            this.View.Layer.InsertSublayer(backgroundLayer, 0);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if (backgroundLayer != null)
                backgroundLayer.Path = CGPath.FromRect(this.View.Bounds);
        }

        public void FadeInBackground()
        {
            UIView.Animate(0.2, () => { backgroundLayer.Opacity = 0.5f; });
        }

        protected void DismissController()
        {
            //UIView.Animate(10, () => { backgroundLayer.Opacity = 0f; }, () =>
            //{
                backgroundLayer.RemoveFromSuperLayer();
                DismissModalViewController(true);
            //});
        }

        protected UIView CreateTitle(string text)
        {
            var titleView = new UILabel
            {
                Text = text,
                BackgroundColor = UIColor.Clear,
                TextColor = TitleTextUiColor,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(TitleTextSize)
            };
            titleView.HeightAnchor.ConstraintEqualTo(TitleRowHeight).Active = true;

            return titleView;
        }

        protected UIView CreateSeparator()
        {
            var separator = new UIView { BackgroundColor = SeparatorColor };
            separator.HeightAnchor.ConstraintEqualTo(1).Active = true;
            //separator.WidthAnchor.ConstraintEqualTo(TopController.View.Frame.Width).Active = true;
            return separator;
        }
    }
}