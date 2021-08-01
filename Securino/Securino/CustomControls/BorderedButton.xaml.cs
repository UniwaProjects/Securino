// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BorderedButton.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the BorderedButton type.
//   Function: Right Image with text relocation and tint
//   Function: Left Image with text relocation and tint
//   Function: No shadow 
//   Function: Lock visual to enabled
//   Function: Flash on text change
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable ComplexConditionExpression

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
    ///     Contains the properties and implementations for the custom component.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BorderedButton
    {
        /// <summary>
        ///     The animation length.
        /// </summary>
        private const int AnimationLength = 100;

        /// <summary>
        ///     The small button scale.
        /// </summary>
        private const double SmallScale = 0.9;

        /// <summary>
        ///     The left image source of the button.
        ///     This will align the text from center (default) to the start position.
        /// </summary>
        public static readonly BindableProperty LeftImageSourceProperty = BindableProperty.Create(
            nameof(LeftImageSource),
            typeof(string),
            typeof(BorderedButton),
            default(string),
            propertyChanged: ImagePropertyChangedHandler);

        /// <summary>
        ///     The highlight color property. Defines the color shown for a highlighted button.
        /// </summary>
        public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
            nameof(AccentColor),
            typeof(Color),
            typeof(BorderedButton),
            default(Color));

        /// <summary>
        ///     The text property of the button. Will become uppercase if the is uppercase
        ///     is enabled.
        /// </summary>
        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(BorderedButton),
            default(string));

        /// <summary>
        ///     Button's command parameter.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(BorderedButton));

        /// <summary>
        ///     Button's command, executes on click.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(BorderedButton));

        /// <summary>
        ///     Button enabled property. Will be false if the command is not bind-ed or cannot execute.
        ///     Changing this property will change the button's enabled state. When the button is
        ///     disabled, it takes on the disabled color and cannot be pressed.
        /// </summary>
        public static readonly BindableProperty EnabledProperty = BindableProperty.Create(
            nameof(Enabled),
            typeof(bool),
            typeof(BorderedButton),
            default(bool),
            BindingMode.TwoWay);

        /// <summary>
        ///     The size of the button text.
        /// </summary>
        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(
            nameof(TextSize),
            typeof(double),
            typeof(BorderedButton),
            default(double));

        /// <summary>
        ///     Initializes a new instance of the <see cref="BorderedButton" /> class.
        ///     Calls toggle button to initialize button state.
        /// </summary>
        public BorderedButton()
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.AccentColor = (Color)Application.Current.Resources["AccentColor"];
        }

        /// <summary>
        ///     Exposes a clicked event
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        ///     Gets or sets the highlight color.
        /// </summary>
        public Color AccentColor
        {
            get => (Color)this.GetValue(AccentColorProperty);
            set => this.SetValue(AccentColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the button text.
        /// </summary>
        public string ButtonText
        {
            get => (string)this.GetValue(ButtonTextProperty);
            set => this.SetValue(ButtonTextProperty, value);
        }

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
        public string LeftImageSource
        {
            get => (string)this.GetValue(LeftImageSourceProperty);
            set => this.SetValue(LeftImageSourceProperty, value);
        }

        /// <summary>
        ///     The light color.
        /// </summary>
        public Color LightColor => (Color)Application.Current.Resources["LightColor"];

        /// <summary>
        ///     The primary color.
        /// </summary>
        public Color PrimaryColor => (Color)Application.Current.Resources["PrimaryColor"];

        /// <summary>
        ///     Gets or sets the text size.
        /// </summary>
        public double TextSize
        {
            get => (double)this.GetValue(TextSizeProperty);
            set => this.SetValue(TextSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether gesture executes.
        ///     Prevents button mash.
        /// </summary>
        private bool GestureExecutes { get; set; }

        /// <summary>
        ///     Event handler for the current command, check the can execute changed
        ///     and toggle the button state accordingly.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="e"> The event arguments. </param>
        public virtual void HandleCanExecuteChanged(object source, EventArgs e)
        {
            ICommand command = (ICommand)source;
            this.Enabled = command.CanExecute(this.CommandParameter);
        }

        /// <summary>
        ///     Tints button depending on state.
        /// </summary>
        public void ToggleButton()
        {
            if (this.Enabled)
            {
                this.LeftImage.TintColor = this.LightColor;
                this.LabelField.TextColor = this.LightColor;
                this.LargeBox.Color = this.AccentColor;
            }
            else
            {
                this.LeftImage.TintColor = this.LightColor;
                this.LabelField.TextColor = this.LightColor;
                this.LargeBox.Color = this.PrimaryColor;
            }
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
                    // Toggle button appearance, this will be after binding
                    this.ToggleButton();

                    // Subscribe to command changes
                    if (this.Command != null)
                    {
                        this.Command.CanExecuteChanged += this.HandleCanExecuteChanged;
                        this.Enabled = this.Command.CanExecute(this.CommandParameter);
                    }
                }
                else
                {
                    // Un subscribe from command changes
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
            BorderedButton view = (BorderedButton)bindable;
            if (view == null)
            {
                return;
            }

            view.LabelField.HorizontalTextAlignment = newValue == null ? TextAlignment.Center : TextAlignment.Start;

            // Set left image
            if (!string.IsNullOrEmpty(view.LeftImageSource))
            {
                view.LeftImage.Source = Utilities.GetImageSource(view.LeftImageSource);
                view.LeftImage.WidthRequest = (double)view.Resources["ImageSide"];
                view.LeftImage.Margin = new Thickness(0, 0, 10, 0);
            }
            else
            {
                view.LeftImage.Source = null;
                view.LeftImage.WidthRequest = 0;
                view.LeftImage.Margin = new Thickness(0, 0);
            }
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
    }
}