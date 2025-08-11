using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace test_data_grid
{
    public class NoFrillsDataGrid
    {
        #region Enumerations

        public enum GridLinesType
        {
            NoGridLines,
            AllGridLines
        }

        #endregion

        #region Constructor

        public NoFrillsDataGrid()
        {
            //empty
        }

        #endregion

        #region Properties

        public float Margin { get; set; } = 0;

        public int TableColumnHeaderTextSize { get; set; } = 18;

        public int TableCellContentTextSize { get; set; } = 18;

        public SKColor BackgroundColor { get; set; } = SKColors.Transparent;

        public SKColor GridLinesColor { get; set; } = SKColors.Black;

        public SKColor TableColumnHeaderTextColor { get; set; } = SKColors.Black;

        public SKColor TableCellContentTextColor { get; set; } = SKColors.Black;

        public List<string> TableColumnHeaders { get; set; } = new List<string>();

        public List<List<object>> TableCellData { get; set; } = new List<List<object>>();

        public List<Tuple<SKRectI, SKColor>> TableCellContentTextColorOverrides { get; set; } = new List<Tuple<SKRectI, SKColor>>();

        public List<Tuple<SKRectI, SKColor>> TableCellBackgroundColorOverrides { get; set; } = new List<Tuple<SKRectI, SKColor>>();

        public GridLinesType GridLineSettings = GridLinesType.AllGridLines;

        public float GridLinesStrokeWidth { get; set; } = 1.0f;

        public bool DisplayHeaderRow { get; set; } = true;

        public bool FitCellSizesToLargestText { get; set; } = true;

        public float CalculatedWidth { get; private set; } = 0.0f;

        public float CalculatedHeight { get; private set; } = 0.0f;

        #endregion

        #region Private methods

        private float GetLargestTextSize ()
        {
            float largest_text_size = 0.0f;

            //Measure the headers
            if (DisplayHeaderRow)
            {
                using (var paint = new SKPaint()
                    {
                        Color = TableColumnHeaderTextColor,
                        TextSize = TableColumnHeaderTextSize,
                        IsAntialias = true,
                        IsStroke = false
                    })
                {
                    foreach (var h in TableColumnHeaders)
                    {
                        if (!string.IsNullOrEmpty(h))
                        {
                            var text_width = paint.MeasureText(h);
                            if (text_width > largest_text_size)
                            {
                                largest_text_size = text_width;
                            }
                        }
                    }
                }
            }

            //Measure the table data
            using (var paint = new SKPaint()
                {
                    Color = TableCellContentTextColor,
                    TextSize = TableCellContentTextSize,
                    IsAntialias = true,
                    IsStroke = false
                })
            {
                for (int r = 0; r < TableCellData.Count; r++)
                {
                    for (int c = 0; c < TableCellData[r].Count; c++)
                    {
                        var text_width = paint.MeasureText(TableCellData[r][c].ToString());
                        if (text_width > largest_text_size)
                        {
                            largest_text_size = text_width;
                        }
                    }
                }
            }

            return largest_text_size;
        }

        private SKColor[,] GetColorOverrideMatrix (List<Tuple<SKRectI, SKColor>> spec)
        {
            SKColor[,] result = null;

            if (TableCellData.Count > 0 && TableCellData[0].Count > 0)
            {
                int number_of_rows = TableCellData.Count;
                if (DisplayHeaderRow)
                {
                    number_of_rows++;
                }

                int number_of_columns = TableCellData[0].Count;

                //Initialize the result matrix fo empty colors
                result = new SKColor[number_of_rows, number_of_columns];

                for (int r = 0; r < number_of_rows; r++)
                {
                    for (int c = 0; c < number_of_columns; c++)
                    {
                        result[r, c] = SKColor.Empty;
                    }
                }

                //Now fill it with the appropriate colors
                for (int i = 0; i < spec.Count; i++)
                {
                    var this_tuple = spec[i];
                    var this_rect = this_tuple.Item1;
                    var this_color = this_tuple.Item2;
                    for (int r = 0; r < this_rect.Height; r++)
                    {
                        var row_coord = this_rect.Top + r;
                        for (int c = 0; c < this_rect.Width; c++)
                        {
                            var col_coord = this_rect.Left + c;
                            if (row_coord < result.GetLength(0) && col_coord < result.GetLength(1))
                            {
                                result[row_coord, col_coord] = this_color;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private SKColor[,] GetCellBackgroundColors ()
        {
            return GetColorOverrideMatrix(TableCellBackgroundColorOverrides);
        }

        private SKColor[,] GetCellTextColors ()
        {
            return GetColorOverrideMatrix(TableCellContentTextColorOverrides);
        }

        #endregion

        #region Public methods

        public void CalculateExpectedDimensions ()
        {
            //Figure out how many rows to draw
            int number_of_table_rows = TableCellData.Count;
            if (DisplayHeaderRow)
            {
                number_of_table_rows++;
            }

            //Figure out how many columns to draw
            int number_of_table_columns = TableCellData[0].Count;

            float largest_width = GetLargestTextSize() * 1.25f;
            float largest_height = Math.Max(TableColumnHeaderTextSize, TableCellContentTextSize) * 1.5f;
            CalculatedWidth = Convert.ToInt32((largest_width * number_of_table_columns) + (2 * Margin));
            CalculatedHeight = Convert.ToInt32((largest_height * number_of_table_rows) + (2 * Margin));
        }

        public void Draw (SKCanvas canvas, int width, int height)
        {
            canvas.Clear(BackgroundColor);

            if (TableCellData.Count > 0 && TableCellData[0].Count > 0)
            {
                //Figure out how many rows to draw
                int number_of_table_rows = TableCellData.Count;
                if (DisplayHeaderRow)
                {
                    number_of_table_rows++;
                }

                //Figure out how many columns to draw
                int number_of_table_columns = TableCellData[0].Count;

                //Verify that all columns have the same count
                bool column_count_ok = TableCellData.All(x => x.Count == number_of_table_columns);
                if (column_count_ok && Margin >= 0)
                {
                    if (FitCellSizesToLargestText)
                    {
                        float largest_width = GetLargestTextSize() * 1.25f;
                        float largest_height = Math.Max(TableColumnHeaderTextSize, TableCellContentTextSize) * 1.5f;
                        width = Convert.ToInt32((largest_width * number_of_table_columns) + (2 * Margin));
                        height = Convert.ToInt32((largest_height * number_of_table_rows) + (2 * Margin));

                        CalculatedWidth = width;
                        CalculatedHeight = height;
                    }

                    //Account for margins
                    float actual_display_width = width - (2 * Margin);
                    float actual_display_height = height - (2 * Margin);
                    float actual_top_ypos = Margin;
                    float actual_bottom_ypos = height - Margin;
                    float actual_left_xpos = Margin;
                    float actual_right_xpos = width - Margin;

                    float column_width = Convert.ToSingle(actual_display_width) / number_of_table_columns;
                    float row_height = Convert.ToSingle(actual_display_height) / number_of_table_rows;
                    float half_column_width = column_width / 2.0f;
                    float half_row_height = row_height / 2.0f;

                    var backgrounds = GetCellBackgroundColors();
                    var foregrounds = GetCellTextColors();

                    //Draw the cell backgrounds
                    for (int r = 0; r < number_of_table_rows; r++)
                    {
                        for (int c = 0; c < number_of_table_columns; c++)
                        {
                            if (backgrounds[r, c] != SKColor.Empty)
                            {
                                float cell_xpos = actual_left_xpos + (column_width * c);
                                float cell_ypos = actual_top_ypos + (row_height * r);

                                using (var paint = new SKPaint()
                                    {
                                        Style = SKPaintStyle.StrokeAndFill,
                                        StrokeWidth = GridLinesStrokeWidth,
                                        Color = backgrounds[r, c],
                                        IsAntialias = true
                                    })
                                {
                                    canvas.DrawRect(new SKRect(cell_xpos, cell_ypos, cell_xpos + column_width,
                                        cell_ypos + row_height), paint);
                                }
                            }
                        }
                    }

                    //Draw the grid lines
                    if (GridLineSettings == GridLinesType.AllGridLines)
                    {
                        using (var paint = new SKPaint()
                            {
                                Style = SKPaintStyle.Stroke,
                                StrokeWidth = GridLinesStrokeWidth,
                                Color = GridLinesColor,
                                IsAntialias = true
                            })
                        {
                            //Start with drawing lines to separate columns
                            float xpos = actual_left_xpos;
                            for (int i = 0; i < (number_of_table_columns + 1); i++)
                            {
                                canvas.DrawLine(xpos, actual_top_ypos, xpos, actual_bottom_ypos, paint);
                                xpos += column_width;
                            }

                            //Now let's draw lines to separate the rows
                            float ypos = actual_top_ypos;
                            for (int i = 0; i < (number_of_table_rows + 1); i++)
                            {
                                canvas.DrawLine(actual_left_xpos, ypos, actual_right_xpos, ypos, paint);
                                ypos += row_height;
                            }
                        }
                    }

                    //Now draw the header text for each column
                    if (DisplayHeaderRow)
                    {
                        using (var paint = new SKPaint()
                            {
                                Color = TableColumnHeaderTextColor,
                                TextSize = TableColumnHeaderTextSize,
                                IsAntialias = true,
                                IsStroke = false
                            })
                        {
                            //Iterate over each column
                            float xpos = actual_left_xpos;
                            float cell_x_center = 0;
                            float cell_y_center = actual_top_ypos + half_row_height;
                            for (int i = 0; i < TableColumnHeaders.Count; i++)
                            {
                                //Change the text color if necessary
                                if (foregrounds[0, i] != SKColors.Empty)
                                {
                                    paint.Color = foregrounds[0, i];
                                }

                                //Measure the text we are about to display
                                var bounds = new SKRect();
                                var text = TableColumnHeaders[i];
                                if (!string.IsNullOrEmpty(text))
                                {
                                    paint.MeasureText(text, ref bounds);

                                    //Calculate the center of this cell
                                    cell_x_center = xpos + half_column_width;

                                    //Determine where to draw the text
                                    float text_x = cell_x_center - (bounds.Width / 2.0f);
                                    float text_y = cell_y_center + (bounds.Height / 2.0f);

                                    //Draw the text
                                    canvas.DrawText(text, text_x, text_y, paint);
                                }
                                
                                //Increment the x value to go to the next column
                                xpos += column_width;

                                //Revert the text color back to the default setting
                                paint.Color = TableColumnHeaderTextColor;
                            }
                        }
                    }

                    //Now let's draw the actual table data
                    using (var paint = new SKPaint()
                        {
                            Color = TableCellContentTextColor,
                            TextSize = TableCellContentTextSize,
                            IsAntialias = true,
                            IsStroke = false
                        })
                    {
                        float xpos = actual_left_xpos;
                        float ypos = (DisplayHeaderRow) ? actual_top_ypos + row_height : actual_top_ypos;
                        float cell_x_center = 0;
                        float cell_y_center = 0;

                        for (int r = 0; r < TableCellData.Count; r++)
                        {
                            xpos = actual_left_xpos;
                            cell_y_center = ypos + half_row_height;

                            for (int c = 0; c < number_of_table_columns; c++)
                            {
                                //Change the text color if necessary
                                var foregrounds_r = r;
                                if (DisplayHeaderRow) foregrounds_r++;
                                if (foregrounds[foregrounds_r, c] != SKColors.Empty)
                                {
                                    paint.Color = foregrounds[foregrounds_r, c];
                                }

                                //Measure the text we are about to display
                                var bounds = new SKRect();
                                var text = TableCellData[r][c].ToString();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    paint.MeasureText(text, ref bounds);

                                    //Calculate the center of this cell
                                    cell_x_center = xpos + half_column_width;

                                    //Determine where to draw the text
                                    float text_x = cell_x_center - (bounds.Width / 2.0f);
                                    float text_y = cell_y_center + (bounds.Height / 2.0f);

                                    //Draw the text
                                    canvas.DrawText(text, text_x, text_y, paint);
                                }

                                //Increment the x value to go to the next column
                                xpos += column_width;

                                //Revert the color back to the default
                                paint.Color = TableCellContentTextColor;
                            }

                            ypos += row_height;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
