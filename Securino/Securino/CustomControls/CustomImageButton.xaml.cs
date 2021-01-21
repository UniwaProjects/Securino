// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomImageButton.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the CustomImageButton type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.CustomControls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Securino.CustomRenderers;
    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     The border-less button.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomImageButton
    {
        /// <summary>
        ///     The animation length.
        /// </summary>
        protected const int AnimationLength = 100;

        /// <summary>
        ///     The small button scale.
        /// </summary>
        protected const double SmallScale = 0.9;

        /// <summary>
        ///     Button's command parameter.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(CustomImageButton));

        /// <summary>
        ///     Button's command, executes on click.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(CustomImageButton));

        /// <summary>
        ///     The image name of the button.
        /// </summary>
        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(
            nameof(ImageName),
            typeof(string),
            typeof(CustomImageButton),
            default(string),
            propertyChanged: ImagePropertyChangedHandler);

        /// <summary>
        ///     Button enabled property. Will be false if the command is not bind-ed or cannot execute.
        ///     Changing this property will change the button's enabled state. When the button is
        ///     disabled, it takes on the disabled color and cannot be pressed.
        /// </summary>
        public static readonly BindableProperty EnabledProperty = BindableProperty.Create(
            nameof(Enabled),
            typeof(bool),
            typeof(CustomImageButton),
            default(bool),
            BindingMode.TwoWay);

        /// <summary>
        ///     The visual always enabled property.
        ///     That way disabling the button won't affect the appearance of the button.
        /// </summary>
        public static readonly BindableProperty VisualAlwaysEnabledProperty = BindableProperty.Create(
            nameof(VisualAlwaysEnabled),
            typeof(bool),
            typeof(BorderedButton),
            default(bool));

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomImageButton" /> class.
        /// </summary>
        public CustomImageButton()
        {
            this.InitializeComponent();
            this.BindingContext = this;
        }

        /// <summary>
        ///     Exposes a clicked event
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether enabled.
        ///     Also toggles the button state when changed.
        /// </summary>
        public bool Enabled
        {
            get => (bool)this.GetValue(EnabledProperty);
            set
            {
                this.SetValue(EnabledProperty, value);
                this.ToggleButton();
            }
        }

        /// <summary>
        ///     Gets or sets the left image source.
        /// </summary>
        public string ImageName
        {
            get => (string)this.GetValue(ImageNameProperty);
            set => this.SetValue(ImageNameProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether visual always enabled.
        /// </summary>
        public bool VisualAlwaysEnabled
        {
            get => (bool)this.GetValue(VisualAlwaysEnabledProperty);
            set
            {
                if (value)
                {
                    this.Enabled = true;
                }

                this.SetValue(VisualAlwaysEnabledProperty, value);
            }
        }

        /// <summary>
        ///     The default color.
        /// </summary>
        protected Color DefaultColor => (Color)Application.Current.Resources["AccentColor"];

        /// <summary>
        ///     The disabled color.
        /// </summary>
        protected Color DisabledColor => (Color)Application.Current.Resources["LightColor"];

        /// <summary>
        ///     Gets or sets a value indicating whether gesture executes.
        ///     Prevents button mash.
        /// </summary>
        protected bool GestureExecutes { get; set; }

        /// <summary>
        ///     Tints button depending on state.
        /// </summary>
        public void ToggleButton()
        {
            if (this.VisualAlwaysEnabled)
            {
                return;
            }

            // Set the color
            this.RootObject.TintColor = this.Enabled ? this.DefaultColor : this.DisabledColor;
        }

        /// <summary>
        ///     Subscribes or unsubscribes from events during the start or
        ///     end of the view's lifecycle.
        /// </summary>
        /// <param name="propertyName"> The property Name. </param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName.Equals("Renderer", StringComparison.OrdinalIgnoreCase))
            {
                if (DependencyService.Get<IRendererResolver>().HasRenderer(this))
                {
                    if (this.Command != null)
                    {
                        this.Command.CanExecuteChanged += this.HandleCanExecuteChanged;
                        this.Enabled = this.Command.CanExecute(this.CommandParameter);
                    }
                }
                else
                {
                    if (this.Command != null)
                    {
                        this.Command.CanExecuteChanged -= this.HandleCanExecuteChanged;
                    }
                }
            }
        }

        /// <summary>
        ///     Aligns the text at center for null new image or at start for a new image.
        /// </summary>
        /// <param name="bindable"> The bindable class. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void ImagePropertyChangedHandler(BindableObject bindable, object oldValue, object newValue)
        {
            CustomImageButton view = (CustomImageButton)bindable;
            if (view == null)
            {
                return;
            }

            view.RootObject.Source =
                !string.IsNullOrEmpty(view.ImageName) ? Utilities.GetImageSource(view.ImageName) : null;
        }

        /// <summary>
        ///     Executes when a button is tapped.
        ///     Animation, command and event are deployed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private async void GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // Mash guard
            if (this.GestureExecutes)
            {
                return;
            }

            this.GestureExecutes = true;

            // Event is invoked in any case
            this.Clicked?.Invoke(sender, e);

            // If the button is enabled play animation and trigger commands
            bool hasOnlyEvent = this.Clicked != null && this.Command == null;
            if (this.Enabled || hasOnlyEvent)
            {
                await this.RootObject.ScaleTo(SmallScale, AnimationLength);
                await this.RootObject.ScaleTo(1, AnimationLength);
                this.Command?.Execute(this.CommandParameter);
            }

            // Reset mash guard
            this.GestureExecutes = false;
        }

        /// <summary>
        ///     Event handler for the current command, check the can execute changed
        ///     and toggle the button state accordingly.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="e"> The event arguments. </param>
        private void HandleCanExecuteChanged(object source, EventArgs e)
        {
            ICommand command = (ICommand)source;
            this.Enabled = command.CanExecute(this.CommandParameter);
        }
    }
}