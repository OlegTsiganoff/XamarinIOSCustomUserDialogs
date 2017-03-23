using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class PaddedUITextField : UITextField
    {
        public CGRect Padding { get; set; }

        public override CGRect TextRect(CGRect rect)
        {
            var paddedRect = new CGRect(rect.X + Padding.X, rect.Y + Padding.Y, rect.Width - Padding.Width, rect.Height - Padding.Height);
            return base.TextRect(paddedRect);
        }

        public override CGRect EditingRect(CGRect rect)
        {
            var paddedRect = new CGRect(rect.X + Padding.X, rect.Y + Padding.Y, rect.Width - Padding.Width, rect.Height - Padding.Height);
            return base.EditingRect(paddedRect);
        }
    }
}