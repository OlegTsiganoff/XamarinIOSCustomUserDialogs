using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class StackView : UIStackView
    {
        private UIColor _backgroundColor;

        public override UIColor BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                SetNeedsLayout(); 
            }
        }

        // this is because of UIStackView is a non-drawing view, meaning that drawRect() is never called and its background color is ignored

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var layer = new CAShapeLayer
            {
                Path = CGPath.FromRect(this.Bounds),
                FillColor = BackgroundColor?.CGColor
            };

            //if (Layer.Sublayers == null)
                Layer.InsertSublayer(layer, 0);
            //else
            //    Layer.ReplaceSublayer(Layer.Sublayers[0], layer);

        }
    }
}