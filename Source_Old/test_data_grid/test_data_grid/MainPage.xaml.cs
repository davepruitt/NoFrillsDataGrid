using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace test_data_grid
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NoFrillsDataGrid g = new NoFrillsDataGrid()
            {
                FitCellSizesToLargestText = true,
                DisplayHeaderRow = true,
                Margin = 50,
                BackgroundColor = SKColors.White,
                TableColumnHeaders = new List<string>() { string.Empty, "David", "Andrea", "Eric", "Michael", "Yuko", "Tanya" },
                TableCellData = new List<List<object>>()
                {
                    new List<object>() { 1, 1, 2, 3, 4, 5, 6 },
                    new List<object>() { 2, 7, 8, 9, 10, 11, 12 },
                    new List<object>() { "sum", 8, 10, 12, 14, 16, 18 }
                },
                TableCellBackgroundColorOverrides = new List<Tuple<SKRectI, SKColor>>()
                {
                    new Tuple<SKRectI, SKColor>(new SKRectI(0, 3, 6, 4), SKColors.SpringGreen)
                },
                TableCellContentTextColorOverrides = new List<Tuple<SKRectI, SKColor>>()
                {
                    new Tuple<SKRectI, SKColor>(new SKRectI(0, 3, 6, 4), SKColors.Blue)
                }
            };

            g.CalculateExpectedDimensions();

            NoFrillsDataGridView gv = new NoFrillsDataGridView()
            {
                DataGrid = g,
                WidthRequest = g.CalculatedWidth,
                HeightRequest = g.CalculatedHeight
            };

            TestScrollView.Content = gv;
            //Content = gv;
        }
    }
}
