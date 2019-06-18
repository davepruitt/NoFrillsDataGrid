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
                TableColumnHeaders = new List<string>() { "David", "Andrea", "Eric", "Michael", "Yuko", "Tanya" },
                TableCellData = new List<List<double>>()
                {
                    new List<double>() { 1, 2, 5, 5, 5, 5 },
                    new List<double>() { 3, 4, 5, 5, 5, 5 }
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
