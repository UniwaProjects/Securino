// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageBase.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the PageBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Views
{
    using Prism.Navigation;

    using Securino.ViewModels;

    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     The page base.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBase : IDestructible
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PageBase" /> class.
        /// </summary>
        public PageBase()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the navigator can go back.
        /// </summary>
        public bool CanGoBack { get; set; }

        /// <summary>
        ///     The destroy callback.
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        ///     Does nothing, disables going back using physical button on Android.
        /// </summary>
        /// <returns> Always false </returns>
        protected override bool OnBackButtonPressed()
        {
            return !this.CanGoBack || ((ViewModelBase)this.BindingContext).IsCommandRunning;
        }
    }
}