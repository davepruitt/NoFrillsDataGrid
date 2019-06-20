using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using NoFrills.Shared;
using SkiaSharp.Views.Android;

namespace NoFrills.Xamarin.Android
{
    public class NoFrillsDataGridView : SKCanvasView
    {
        #region Constructor

        public NoFrillsDataGridView (Context context)
            : base(context)
        {
            this.PaintSurface += Handle_PaintSurface;
        }
        
        public NoFrillsDataGridView (Context context, IAttributeSet attributes)
            : base(context, attributes)
        {
            this.PaintSurface += Handle_PaintSurface;
        }

        public NoFrillsDataGridView (Context context, IAttributeSet attributes, int defStyleAtt)
            : base(context, attributes, defStyleAtt)
        {
            this.PaintSurface += Handle_PaintSurface;
        }

        public NoFrillsDataGridView (IntPtr ptr, JniHandleOwnership jni)
            : base(ptr, jni)
        {
            this.PaintSurface += Handle_PaintSurface;
        }

        #endregion

        #region Variables and properties

        private NoFrillsDataGrid data_grid;

        public NoFrillsDataGrid DataGrid
        {
            get => this.data_grid;
            set
            {
                if (this.data_grid != value)
                {
                    this.data_grid = value;
                    this.Invalidate();
                }
            }
        }
        
        #endregion

        #region Paint handler

        private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.DataGrid != null)
            {
                DataGrid.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }

        #endregion
    }
}