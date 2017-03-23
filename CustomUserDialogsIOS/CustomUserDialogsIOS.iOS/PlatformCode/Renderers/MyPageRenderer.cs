using System;
using System.Collections.Generic;
using System.Text;
using CustomUserDialogsIOS.iOS.PlatformCode.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(MyPageRenderer))]
namespace CustomUserDialogsIOS.iOS.PlatformCode.Renderers
{
    public class MyPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            CurrentViewController.Current = this.ViewController;
        }
    }
}
