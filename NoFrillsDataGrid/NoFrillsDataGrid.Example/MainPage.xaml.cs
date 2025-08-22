using NoFrills;
using SkiaSharp;

namespace NoFrills.DataGridExample
{
    public partial class MainPage : ContentPage
    {
        #region Constructor

        public MainPage()
        {
            InitializeComponent();
            CreateDataGrid();
        }

        #endregion

        #region Private methods

        private void CreateDataGrid ()
        {
            NoFrillsDataGrid g = new NoFrillsDataGrid()
            {
                FitCellSizesToLargestText = true,
                DisplayHeaderRow = true,
                Margin = 50,
                BackgroundColor = SKColors.White,
                TableColumnHeaders = new List<string>() { "Column A", "Column B", "Column C", "Column D", "Column E", "Column F" },
                TableCellData = new List<List<object>>()
                    {
                        new List<object>() { 1, 2, 3, 4, 5, 6 },
                        new List<object>() { 7, 8, 9, 10, 11, 12 }
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

        #endregion
    }
}
