using System;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using NoFrills.Shared;

namespace NoFrills.Xamarin.Forms
{
    public class NoFrillsDataGridView : SKCanvasView
    {
        #region Constructor

        public NoFrillsDataGridView()
        {
            this.IgnorePixelScaling = true;
            this.BackgroundColor = Color.Transparent;
            this.PaintSurface += Handle_PaintSurface;
        }

        #endregion

        #region Dependency properties

        public static readonly BindableProperty DataGridProperty = BindableProperty.Create(nameof(NoFrillsDataGrid),
            typeof(NoFrillsDataGrid), typeof(NoFrillsDataGridView), null, propertyChanged: OnDataGridChanged);

        public NoFrillsDataGrid DataGrid
        {
            get
            {
                return (NoFrillsDataGrid)GetValue(DataGridProperty);
            }
            set
            {
                SetValue(DataGridProperty, value);
            }
        }

        #endregion

        #region Private Methods

        private static void OnDataGridChanged (BindableObject bindable, object oldValue, object newValue)
        {
            ((NoFrillsDataGridView)bindable).InvalidateSurface();
        }

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
