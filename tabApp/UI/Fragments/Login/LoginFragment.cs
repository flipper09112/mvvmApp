using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Login;

namespace tabApp.UI.Fragments.Login
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class LoginFragment : BaseFragment<LoginViewModel>
    {
        private MainActivity _activity;
        private EditText _username;
        private EditText _password;
        private Button _loginBtn;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _username = view.FindViewById<EditText>(Resource.Id.editTextTextPersonName);
            _password = view.FindViewById<EditText>(Resource.Id.editTextTextPassword);
            _loginBtn = view.FindViewById<Button>(Resource.Id.loginBtn);

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            _loginBtn.Enabled = ViewModel.ShowHomePage.CanExecute();
        }

        public override void SetupBindings()
        {
            ViewModel.ShowHomePage.CanExecuteChanged += ShowHomePageCanExecuteChanged;
            _loginBtn.Click += LoginBtnClick;
            _username.TextChanged += UsernameTextChanged;
            _password.TextChanged += PasswordTextChanged; ;
        }

        public override void CleanBindings()
        {
            ViewModel.ShowHomePage.CanExecuteChanged -= ShowHomePageCanExecuteChanged;
            _loginBtn.Click -= LoginBtnClick;
            _username.TextChanged -= UsernameTextChanged;
            _password.TextChanged -= PasswordTextChanged;
        }

        private void PasswordTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ViewModel.Password = _password.Text;
        }

        private void UsernameTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ViewModel.Username = _username.Text;
        }

        private void LoginBtnClick(object sender, EventArgs e)
        {
            ViewModel.ShowHomePage.Execute();
        }

        private void ShowHomePageCanExecuteChanged(object sender, EventArgs e)
        {
            _loginBtn.Enabled = ViewModel.ShowHomePage.CanExecute();
        }
    }
}