using CoreGraphics;
using UIKit;

namespace CustomUserDialogsIOS.iOS.PlatformCode.UserDialogImpl.ViewControllers
{
    public class PaddedUILabel : UILabel
    {
        public CGRect Padding { get; set; }
        public override void DrawText(CGRect rect)
        {
            var paddedRect = new CGRect(rect.X + Padding.X, rect.Y + Padding.Y, rect.Width - Padding.Width, rect.Height - Padding.Height);
            base.DrawText(paddedRect);
        }
    }
}