using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFrills
{
    public class NoFrillsDataGridView : SKCanvasView
    {
        #region Constructor

        public NoFrillsDataGridView()
        {
            IgnorePixelScaling = true;
            BackgroundColor  = Colors.Transparent;
        }

        #endregion

        #region Dependency properties

        public static readonly BindableProperty DataGridProperty = BindableProperty.Create(nameof(DataGrid),
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

        #region Overrides

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            if (DataGrid != null && e != null)
            {
                DataGrid.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }

        #endregion

        #region Private Methods

        private static void OnDataGridChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((NoFrillsDataGridView)bindable).InvalidateSurface();
        }

        #endregion
    }
}
