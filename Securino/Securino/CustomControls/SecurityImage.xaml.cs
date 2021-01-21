// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityImage.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the SecurityImage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.CustomControls
{
    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     The security image.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecurityImage
    {
        /// <summary>
        ///     The disarmed property.
        /// </summary>
        public static readonly BindableProperty DisarmedProperty = BindableProperty.Create(
            nameof(Disarmed),
            typeof(bool),
            typeof(SecurityImage),
            default(bool),
            propertyChanged: OnDisarmedPropertyChanged);

        /// <summary>
        ///     The triggered property.
        /// </summary>
        public static readonly BindableProperty TriggeredProperty = BindableProperty.Create(
            nameof(Triggered),
            typeof(bool),
            typeof(SecurityImage),
            default(bool),
            propertyChanged: OnTriggeredPropertyChanged);

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityImage" /> class.
        /// </summary>
        public SecurityImage()
        {
            this.InitializeComponent();
            this.UpdateState(this.Triggered, this.Disarmed);
        }

        /// <summary>
        ///     The accent color.
        /// </summary>
        public static Color AccentColor => (Color)Application.Current.Resources["AccentColor"];

        /// <summary>
        ///     The error color.
        /// </summary>
        public static Color ErrorColor => (Color)Application.Current.Resources["ErrorColor"];

        /// <summary>
        ///     Gets or sets a value indicating whether disarmed.
        /// </summary>
        public bool Disarmed
        {
            get => (bool)this.GetValue(DisarmedProperty);
            set => this.SetValue(DisarmedProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether disarmed.
        /// </summary>
        public bool Triggered
        {
            get => (bool)this.GetValue(TriggeredProperty);
            set => this.SetValue(TriggeredProperty, value);
        }

        /// <summary>
        ///     The on disarmed property changed.
        /// </summary>
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnDisarmedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SecurityImage securityImage = (SecurityImage)bindable;

            securityImage?.UpdateState(securityImage.Triggered, (bool)newValue);
        }

        /// <summary>
        ///     The on triggered property changed.
        /// </summary>
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnTriggeredPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SecurityImage securityImage = (SecurityImage)bindable;

            securityImage?.UpdateState((bool)newValue, securityImage.Disarmed);
        }

        /// <summary>
        ///     The update state.
        /// </summary>
        /// <param name="isTriggered"> The is triggered. </param>
        /// <param name="isDisarmed"> The is disarmed. </param>
        private void UpdateState(bool isTriggered, bool isDisarmed)
        {
            if (isTriggered)
            {
                this.CircleImage.TintColor = ErrorColor;
                this.BadgeImage.TintColor = ErrorColor;
                this.BadgeImage.Source = Utilities.GetImageSource("ic_triggered.svg");
                return;
            }

            this.CircleImage.TintColor = isDisarmed ? ErrorColor : AccentColor;
            this.BadgeImage.TintColor = isDisarmed ? ErrorColor : AccentColor;
            this.BadgeImage.Source = Utilities.GetImageSource(isDisarmed ? "ic_not_secure.svg" : "ic_secure.svg");
        }
    }
}