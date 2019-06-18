using NoFrills.Shared;
using NoFrills.Xamarin.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExampleApplication
{
    [DesignTimeVisible(false)]
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
                TableColumnHeaders = new List<string>() { "Column A", "Column B", "Column C", "Column D", "Column E", "Column F" },
                TableCellData = new List<List<double>>()
                {
                    new List<double>() { 1, 2, 3, 4, 5, 6 },
                    new List<double>() { 7, 8, 9, 10, 11, 12 }
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
        }
    }
}
