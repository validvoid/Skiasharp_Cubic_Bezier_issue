using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SkiaSharp;
using SkiaSharp.Views.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Skiasharp_Cubic_Bezier
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SKMatrix _canvasMatrix = SKMatrix.CreateIdentity();

        private SKPaint _paint = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.Blue,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 5,
            TextSize = 120
        };

        private SKPicture _picture;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void SkCanvas_OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            SKImageInfo info = new SKImageInfo(e.BackendRenderTarget.Width, e.BackendRenderTarget.Height, e.ColorType);

            SKSurface surface = e.Surface;
            SKCanvas surfaceCanvas = surface.Canvas;

            surfaceCanvas.Clear();

            surfaceCanvas.SetMatrix(_canvasMatrix);


            if (_picture == null)
            {
                using (SKPictureRecorder pictureRecorder = new SKPictureRecorder())
                using (SKCanvas canvas = pictureRecorder.BeginRecording(info.Rect))
                using (SKPath path = new SKPath())
                {
                    /*
                     * Draw path with multiple segments(>=3).
                     * Then every time you translate the surfaceCanvas, it consumes more GPU memory.
                     */
                    path.CubicTo(150, 50, 200, 125, 300, 25);

                    // path.LineTo(80, 125);
                    // path.LineTo(130, 75);
                    // path.LineTo(200, 205);


                    canvas.DrawPath(path, _paint);

                    // Fine with DrawRoundRect or DrawText
                    // canvas.DrawText("TEXT", new SKPoint(50, 50), _paint);
                    // canvas.DrawRoundRect(120,120,300,220,12,12,paint);

                    _picture = pictureRecorder.EndRecording();
                }
            }


            surfaceCanvas.DrawPicture(_picture);

            /*
             * Directly drawing on surfaceCanvas is fine.
             */
            // using (SKPath path = new SKPath())
            // {
            //     path.CubicTo(150, 50, 200, 125, 300, 25);
            //
            //     path.LineTo(80, 125);
            //     path.LineTo(130, 75);
            //     path.LineTo(200, 205);
            //
            //     surfaceCanvas.DrawPath(path, _paint);
            // }
        }


        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _canvasMatrix.TransX += (float)(e.Delta.Translation.X / GetScreenScaleFactor());
            _canvasMatrix.TransY += (float)(e.Delta.Translation.Y / GetScreenScaleFactor());

            skCanvas.Invalidate();

            if (e.IsInertial)
            {
                e.Complete();
            }

            e.Handled = true;
        }

        private double GetScreenScaleFactor()
        {
            return Windows.Graphics.Display.DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
        }
    }
}
