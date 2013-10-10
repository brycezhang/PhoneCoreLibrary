using System.ComponentModel;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;

namespace PhoneCoreLibrary.Navigation
{
    /// <summary>
    /// 可导航视图模型基类
    /// </summary>
    public abstract class NavigationViewModelBase : ViewModelBase
    {
        #region Loading

        private bool _isShowLoading;
        /// <summary>
        /// 是否显示Loading控件
        /// </summary>
        public bool IsShowLoading
        {
            get { return _isShowLoading; }
            set
            {
                _isShowLoading = value;
                RaisePropertyChanged(() => IsShowLoading);
            }
        }

        /// <summary>
        /// 显示Loading
        /// </summary>
        public void ShowLoading()
        {
            IsShowLoading = true;
        }

        /// <summary>
        /// 隐藏Loading控件
        /// </summary>
        public void HideLoading()
        {
            IsShowLoading = false;
        }

        #endregion

        #region 导航

        protected bool RemoveBackEntry { get; set; }

        public NavigationService NavigationService { get; set; }

        public NavigationContext NavigationContext { get; set; }

        public virtual void OnNavigatedTo(NavigationEventArgs e) { }

        public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e) { }

        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (RemoveBackEntry)
            {
                RemoveBackEntry = false;
                NavigationService.RemoveBackEntry();
            }
        }

        public virtual void OnBackKeyPress(CancelEventArgs e) { }

        #endregion
    }
}
