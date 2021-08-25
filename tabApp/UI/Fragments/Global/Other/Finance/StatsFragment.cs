using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LechoLib.HelloCharts.Gestures;
using LechoLib.HelloCharts.Models;
using LechoLib.HelloCharts.Utils;
using LechoLib.HelloCharts.Views;
using Microcharts;
using Microcharts.Droid;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Other.Finance;
using static tabApp.Core.ViewModels.Global.Other.Finance.StatsViewModel;
using Axis = LechoLib.HelloCharts.Models.Axis;

namespace tabApp.UI.Fragments.Global.Other.Finance
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class StatsFragment : BaseFragment<StatsViewModel>
    {
        private MainActivity _activity;
        private ComboLineColumnChartView _chartView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;

            View view = inflater.Inflate(Resource.Layout.StatsFragment, container, false);

            _chartView = (ComboLineColumnChartView) view.FindViewById(Resource.Id.chartView);

            return view;
        }

        public override void SetUI()
        {
            GenerateData();
        }

        public override void SetupBindings()
        {
        }
        public override void CleanBindings()
        {
        }

        private void GenerateData()
        {
            // Chart looks the best when line data and column data have similar maximum viewports.
            var data = new ComboLineColumnChartData(GenerateColumnData(), GenerateLineData());

            if (true)
            {
                Axis axisTop = new Axis();
                Axis axisX = new Axis();
                Axis axisY = new Axis().SetHasLines(true);
               
                axisX.SetName("Dias");
                axisY.SetName("Saldo (€)");
                axisTop.SetName("Estatistica de gastos");
                axisTop.SetValues(null);
                axisX.SetTextSize(25);
                axisY.SetTextSize(25);
                axisTop.SetTextSize(30);

                data.AxisXBottom = axisX;
                data.AxisYLeft = axisY;
                data.AxisXTop = axisTop;

                List<AxisValue> list = new List<AxisValue>();
                List<AxisValue> list2 = new List<AxisValue>();
                for (int n = 0; n < ViewModel.ProductsList.Count; ++n)
                {
                    AxisValue axisValue = new AxisValue(n);
                    AxisValue axisValue2 = new AxisValue(n);

                    axisValue.SetLabel(ViewModel.ProductsList[n].OrderRegistDate.ToString("dd/MM/yyyy"));
                    list.Add(axisValue);

                    axisValue2.SetLabel("");
                    list2.Add(axisValue2);
                }

                axisX.SetValues(list);
                axisTop.SetValues(list2);
            }

            _chartView.ComboLineColumnChartData = data;
        }

        private LineChartData GenerateLineData()
        {

            List<Line> lines = new List<Line>();
            for (int i = 0; i < 2; ++i)
            {

                List<PointValue> values = new List<PointValue>();
                for (int j = 0; j < ViewModel.ProductsList.Count; ++j)
                {
                    values.Add(new PointValue(j, ViewModel.GetValue(ViewModel.ProductsList[j].ItemsList, i == 0 ? GetValueEnum.GetValueIn : GetValueEnum.GetValueOut, ViewModel.ProductsList[j].OrderRegistDate)));
                }

                Line line = new Line(values);
                line.SetColor(ChartUtils.Colors[i]);
                line.SetCubic(true);
                line.SetHasLabels(true);
                line.SetHasLines(true);
                line.SetHasPoints(true);
                lines.Add(line);
            }

            LineChartData lineChartData = new LineChartData(lines);

            return lineChartData;

        }

        private ColumnChartData GenerateColumnData()
        {
            int numSubcolumns = 1;
            int numColumns = ViewModel.ProductsList.Count;
            // Column can have many subcolumns, here by default I use 1 subcolumn in each of 8 columns.
            List<Column> columns = new List<Column>();
            List<SubcolumnValue> values;
            for (int i = 0; i < numColumns; ++i)
            {
                values = new List<SubcolumnValue>();
                for (int j = 0; j < numSubcolumns; ++j)
                {
                    values.Add(new SubcolumnValue(ViewModel.GetValueDiff(ViewModel.ProductsList[i].ItemsList, ViewModel.ProductsList[i].OrderRegistDate), ChartUtils.ColorGreen));
                }

                Column column = new Column(values);
                column.SetHasLabelsOnlyForSelected(true);
                columns.Add(column);
            }

            ColumnChartData columnChartData = new ColumnChartData(columns);
            return columnChartData;
        }
    }
}