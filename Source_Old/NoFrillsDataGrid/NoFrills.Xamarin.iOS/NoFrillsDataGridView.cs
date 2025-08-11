using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using NoFrills.Shared;
using SkiaSharp.Views.iOS;
using UIKit;

namespace NoFrills.Xamarin.iOS
{
    public class NoFrillsDataGridView : SKCanvasView
    {
        #region Constructor

        public NoFrillsDataGridView ()
        {
            this.BackgroundColor = UIColor.Clear;
            this.PaintSurface += NoFrillsDataGridView_PaintSurface;
        }

        #endregion

        #region Properties

        private NoFrillsDataGrid data_grid;

        public NoFrillsDataGrid DataGrid
        {
            get => this.data_grid;
            set
            {
                if (this.data_grid != value)
                {
                    this.data_grid = value;
                    this.SetNeedsDisplayInRect(this.Bounds);
                }
            }
        }

        #endregion

        #region Paint surface handler

        private void NoFrillsDataGridView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.DataGrid != null)
            {
                this.DataGrid.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }

        #endregion
    }
}