using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LiveCharts;
using LiveCharts.Wpf;

namespace LCTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] m_nHistgram = new int[256];
        public MainWindow()
        {

            InitializeComponent();
            InitGraph();
            /****************************************************XX Start*****************************************************/
            //    LineSeries ls1 = new LineSeries();
            //    SeriesCollection = new SeriesCollection
            //    {
            //        new LineSeries
            //        {
            //            Title = "2月6日",
            //            Values = new ChartValues<double>  { -4, -4, -5, -56, -7, -8, -9, -8, -6, -4, -5, -5, -2, -1, 0, 1, 3, 6, 6, 4, 52, 1, -1, -2},
            //            //Fill = Brushes.Transparent,

            //        },
            //        new LineSeries
            //        {
            //            Title = "9月5日",
            //            Values = new ChartValues<double> { 23, 22, 22, 21, -21, 21, 21, 22, 23, 25, 26, 27, 29, -30, 31, 31, 31, 30, 28, 26, 25, 24, 24, 23},
            //            //Fill = Brushes.Transparent,

            //        },

            //        new LineSeries
            //        {
            //            Title = "5月18日",
            //            Values = new ChartValues<double>  { 15, -4, -5, -6, -7, -8, 34, -8, -6, -4, 124, -5, 33, -1, 0, 1, 3, 6, 6, 4, 2, 1, -1, -2},
            //            //Fill = Brushes.Transparent,

            //        },
            //        new LineSeries
            //        {
            //            Title = "7月17日",
            //            Values = new ChartValues<double> { 23, 89, 22, 21, 45, 21, 21, 22, 23, -25, 26, 27, 78, 3, 4, 41, 33, 30, 28, 26, 25, 24, 24, 23},
            //            //Fill = Brushes.Transparent,

            //        },
            //    };

            //    Labels = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            //    //YFormatter = value => value.ToString("C");

            //    //modifying the series collection will animate and update the chart
            //    //SeriesCollection.Add(new LineSeries
            //    //{
            //    //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    //    LineSmoothness = 0 //straight lines, 1 really smooth lines
            //    //});

            //    ////modifying any series values will also animate and update the chart
            //    //SeriesCollection[2].Values.Add(5d);

                //DataContext = this;
            
        }

        //public SeriesCollection SeriesCollection { get; set; }
        //public string[] Labels { get; set; }
        //public Func<double, string> YFormatter { get; set; }
        /****************************************************XX End*****************************************************/
        public void InitGraph()
        {
            GraphData graphData = new GraphData();
            var chartValue = new ChartValues<int>();
            for(int nIdx = 0; nIdx < m_nHistgram.Length; nIdx++)
            {
                m_nHistgram[nIdx] = 0;
                chartValue.Add(m_nHistgram[nIdx]);
            }

            var seriesCollection = new SeriesCollection();

            var lineSeriesChart = new LineSeries()
            {
                Values = chartValue,
                Title = "Histgram"
            };

            seriesCollection.Add(lineSeriesChart);
            graphData.seriesCollection = seriesCollection;
            this.DataContext = graphData;

            return;
        }

        public void DrawHistgram(BitmapImage _bitmap)
        {
            GraphData graphData = new GraphData();

            InitHistgram();

            CalHistgram(_bitmap);

            var chartValue = new ChartValues<int>();
            for(int nIdx = 0; nIdx < m_nHistgram.Length; nIdx ++)
            {
                chartValue.Add(m_nHistgram[nIdx]);
            }
            var seriesCollection = new SeriesCollection();

            var lineSeriesChart = new LineSeries()
            {
                Values = chartValue,
                Title = "Histgram"
            };
            seriesCollection.Add(lineSeriesChart);
            graphData.seriesCollection = seriesCollection;
            this.DataContext = graphData;

        }
        public void CalHistgram(BitmapImage _bitmap)
        {
            int nWidthSize = _bitmap.PixelWidth;
            int nHeightSize = _bitmap.PixelHeight;

            WriteableBitmap wBitmap = new WriteableBitmap(_bitmap);

            int nIdxWidth;
            int nIdxHeight;

            unsafe
            {
                for(nIdxHeight = 0; nIdxHeight < nHeightSize; nIdxHeight++)
                {
                    for(nIdxWidth = 0; nIdxWidth < nWidthSize; nIdxWidth++)
                    {
                        byte* pPixel = (byte*)wBitmap.BackBuffer + nIdxHeight * wBitmap.BackBufferStride + nIdxWidth * 4;
                        byte nGrayScale = (byte)((pPixel[(int)ComInfo.Pixel.B] + pPixel[(int)ComInfo.Pixel.G] + pPixel[(int)ComInfo.Pixel.R]) / 3);
                        m_nHistgram[nGrayScale] += 1;
                    }
                }
            }

            

        }
        public void InitHistgram()
        {
            for(int nIdx = 0; nIdx < m_nHistgram.Length; nIdx++)
            {
                m_nHistgram[nIdx] = 0;
            }
        }
        /****************************************************事件函数*****************************************************/
        private void OnClickBtnFileSelect(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("打开文件");
            ComOpenFileDialog openFileDialog = new ComOpenFileDialog();
            openFileDialog.Filter = "JPG|*.jpg|PNG|*.png";
            openFileDialog.Title = "Open the file";
            if(openFileDialog.ShowDialog() == true)
            {
                image.Source = null;
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                bitmap.Freeze();
                image.Source = bitmap;
                DrawHistgram(bitmap);
            }
            return;

        }


    }

    public class GraphData
    {
        private SeriesCollection m_seriesCollection;
        public SeriesCollection seriesCollection
        {
            set { m_seriesCollection = value; }
            get { return m_seriesCollection; }
        }

    }
}
