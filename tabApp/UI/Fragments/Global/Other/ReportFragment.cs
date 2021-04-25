using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Github.Barteksc.Pdfviewer;
using Java.IO;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Other;
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.Global.Other
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ReportFragment : BaseFragment<ReportViewModel>
    {
        private MainActivity _activity;
        private PDFView _pdfView;
        private ImageView _withoutPdf;
        private Button _makeReport;
        private Button _shareButton;
        private RecyclerView _recyclerView;
        private TextView _reportDayLabel;
        private TextView _selectProductsLabel;
        private EditText _editTextDate;
        private RadioButton _specificDay;
        private RadioButton _allDays;
        private ReportItemsAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;

            View view = inflater.Inflate(Resource.Layout.ReportFragment, container, false);

            _pdfView = view.FindViewById<PDFView>(Resource.Id.pdfView);
            _withoutPdf = view.FindViewById<ImageView>(Resource.Id.withoutPdf);
            _makeReport = view.FindViewById<Button>(Resource.Id.makeReport);
            _shareButton = view.FindViewById<Button>(Resource.Id.shareButton);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _reportDayLabel = view.FindViewById<TextView>(Resource.Id.reportDayLabel);
            _selectProductsLabel = view.FindViewById<TextView>(Resource.Id.selectProductsLabel);
            _editTextDate = view.FindViewById<EditText>(Resource.Id.editTextDate);
            _specificDay = view.FindViewById<RadioButton>(Resource.Id.specificDay);
            _allDays = view.FindViewById<RadioButton>(Resource.Id.allDays);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _adapter = new ReportItemsAdapter(ViewModel.SelectedProducts, ViewModel);
            _recyclerView.SetAdapter(_adapter);

            return view;
        }

        public override void SetUI()
        {
            GenerateNewFileCommandCanExecuteChanged(null, null);
            SetDayUi();
            _editTextDate.Text = ViewModel.DateSelected?.ToString("dd/MM/yyyy");

            if (ViewModel.ReportArray == null) return;
            _pdfView.FromBytes(ViewModel.ReportArray)
                .EnableSwipe(true) // allows to block changing pages using swipe
                .SwipeHorizontal(false)
                .DefaultPage(0)
                // allows to draw something on the current page, usually visible in the middle of the screen
                //.onDraw(onDrawListener)
                // allows to draw something on all pages, separately for every page. Called only for visible pages
               // .onDrawAll(onDrawListener)
                //.onLoad(onLoadCompleteListener) // called after document is loaded and starts to be rendered
                //.onPageChange(onPageChangeListener)
                //.onPageScroll(onPageScrollListener)
                //.onError(onErrorListener)
               // .onPageError(onPageErrorListener)
               // .onRender(onRenderListener) // called after document is rendered for the first time
                                            // called on single tap, return true if handled, false to toggle scroll handle visibility
              //  .onTap(onTapListener)
               // .onLongPress(onLongPressListener)
               // .enableAnnotationRendering(false) // render annotations (such as comments, colors or forms)
               // .password(null)
              //  .scrollHandle(null)
             //   .enableAntialiasing(true) // improve rendering a little bit on low-res screens
                                          // spacing between pages in dp. To define spacing color, set view background
               // .spacing(0)
               // .autoSpacing(false) // add dynamic spacing to fit each page on its own on the screen
               // .linkHandler(DefaultLinkHandler)
               // .pageFitPolicy(FitPolicy.WIDTH) // mode to fit pages in the view
              //  .fitEachPage(false) // fit each page to the view, else smaller pages are scaled relative to largest page.
              //  .pageSnap(false) // snap pages to screen boundaries
              //  .pageFling(false) // make a fling change only a single page like ViewPager
              //  .nightMode(false) // toggle night mode
                .Load();
        }

        private void SetDayUi()
        {
            _editTextDate.Enabled = false;
            if (ViewModel.AllDays)
            {
                _reportDayLabel.Alpha = 0.3f;
                _editTextDate.Alpha = 0.3f;
                _selectProductsLabel.Alpha = 0.3f;
                _recyclerView.Alpha = 0.3f;
                _recyclerView.Enabled = false;
                _recyclerView.Clickable = false;
                _editTextDate.Clickable = false;
                _editTextDate.Enabled = false;
            }
            else
            {
                _reportDayLabel.Alpha = 1f;
                _editTextDate.Alpha = 1f;
                _selectProductsLabel.Alpha = 1f;
                _recyclerView.Alpha = 1f;
                _recyclerView.Enabled = true;
                _recyclerView.Clickable = true;
                _editTextDate.Clickable = true;
                _editTextDate.Enabled = true;
            }
        }

        public override void CleanBindings()
        {
            ViewModel.RemoveProductEvent -= RemoveProductEvent;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _makeReport.Click -= MakeReportClick;
            ViewModel.GenerateNewFileCommand.CanExecuteChanged -= GenerateNewFileCommandCanExecuteChanged;
            _allDays.Click -= AllDaysClick;
            _specificDay.Click -= SpecificDayClick;
            _editTextDate.Click -= EditTextDateClick;
            _shareButton.Click -= ShareButtonClick;
            ViewModel.OpenExternalApp -= OpenExternalApp;
        }

        public override void SetupBindings()
        {
            ViewModel.RemoveProductEvent += RemoveProductEvent;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _makeReport.Click += MakeReportClick;
            ViewModel.GenerateNewFileCommand.CanExecuteChanged += GenerateNewFileCommandCanExecuteChanged;
            _allDays.Click += AllDaysClick;
            _specificDay.Click += SpecificDayClick;
            _editTextDate.Click += EditTextDateClick;
            _shareButton.Click += ShareButtonClick;
            ViewModel.OpenExternalApp += OpenExternalApp;
        }

        private void RemoveProductEvent(object sender, EventArgs e)
        {
            int position = (int)sender;
            _adapter.NotifyItemRemoved(position);
        }

        private void OpenExternalApp(object sender, EventArgs e)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            File file = (File)sender;

            Intent target = new Intent(Intent.ActionView);
            target.SetDataAndType(Android.Net.Uri.FromFile(file), "application/pdf");
            target.SetFlags(ActivityFlags.NoHistory | ActivityFlags.GrantReadUriPermission);

            Intent intent = Intent.CreateChooser(target, "Open File");
            try
            {
                StartActivity(intent);
            }
            catch (ActivityNotFoundException es)
            {
                // Instruct the user to install a PDF reader here, or something
            }
        }

        private void ShareButtonClick(object sender, EventArgs e)
        {
            ViewModel.OpenPDFExtrenalAppCommand.Execute(null);
        }

        private void EditTextDateClick(object sender, EventArgs e)
        {
            ViewModel.SelectDayCommand.Execute(null);
        }

        private void SpecificDayClick(object sender, EventArgs e)
        {
            ViewModel.AllDays = false;
            ViewModel.GenerateNewFileCommand.RaiseCanExecuteChanged();
            SetDayUi();
        }

        private void AllDaysClick(object sender, EventArgs e)
        {
            ViewModel.AllDays = true;
            ViewModel.GenerateNewFileCommand.RaiseCanExecuteChanged();
            SetDayUi();
        }

        private void GenerateNewFileCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.GenerateNewFileCommand.CanExecute(null))
                _makeReport.Enabled = true;
            else
                _makeReport.Enabled = false;
        }

        private void MakeReportClick(object sender, EventArgs e)
        {
            ViewModel.GenerateNewFileCommand.Execute(null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.DateSelected):
                case nameof(ViewModel.ReportArray):
                    SetUI();
                    break;
            }
        }
    }
}