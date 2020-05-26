using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Token
{
    public partial class TokenEntry : ContentView
    {
        public static readonly BindableProperty EntrysProperty =
                BindableProperty.Create(nameof(Grid), typeof(Grid), typeof(TokenEntry), default(Grid));
        public Grid Grid
        {
            get { return (Grid)GetValue(EntrysProperty); }
            private set { SetValue(EntrysProperty, value); }
        }

        public static readonly BindableProperty TokenProperty =
                BindableProperty.Create(nameof(Entry), typeof(Entry), typeof(TokenEntry), default(Entry));

        public Entry Entry
        {
            get { return (Entry)GetValue(TokenProperty); }
            private set { SetValue(TokenProperty, value); }
        }

        private static readonly BindableProperty _text =
                BindableProperty.Create(nameof(Text), typeof(string), typeof(TokenEntry), default(string));
        public string Text
        {
            get { return (string)GetValue(_text); }
            private set { SetValue(_text, value); }
        }


        public static readonly BindableProperty NumberEntrysProperty =
                BindableProperty.Create(nameof(NumberEntrys), typeof(int), typeof(TokenEntry), default(int), propertyChanged: NumberEntrysPropertyChanged);

        private static void NumberEntrysPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var vm = bindable as TokenEntry;
            vm.ViewPage();
        }

        public int NumberEntrys
        {
            get { return (int)GetValue(NumberEntrysProperty); }
            set { SetValue(NumberEntrysProperty, value); }
        }


        public static readonly BindableProperty VariablesProperty =
                BindableProperty.Create(nameof(Variables), typeof(List<IDictionary<string, object>>), typeof(TokenEntry), default(List<IDictionary<string, object>>));

        public List<IDictionary<string, object>> Variables
        {
            get { return (List<IDictionary<string, object>>)GetValue(VariablesProperty); }
            private set { SetValue(VariablesProperty, value); }
        }

        public TokenEntry()
        {
            Variables = new List<IDictionary<string, object>>();
            NumberEntrys = 4;
        }

        public void ViewPage()
        {
            Entry = new Entry();

            Grid = new Grid()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 8
            };

            for (var i = 1; i <= NumberEntrys; i++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

                var tapGestureRecognizer = new TapGestureRecognizer();

                tapGestureRecognizer.Tapped += Button_Clicked;

                var frame = new Frame
                {
                    Padding = 0,
                    CornerRadius = 6,
                    BackgroundColor = Color.White,
                    HasShadow = false,
                    Content = new Label { FontSize = 32 },
                    VerticalOptions = LayoutOptions.StartAndExpand
                };

                frame.GestureRecognizers.Add(tapGestureRecognizer);

                Variables.Add(new Dictionary<string, object>
                {
                    {i.ToString(), frame }
                });

                Grid.Children.Add(frame, i - 1, 0);

            }

            var c = new Grid();

            Entry = new Entry
            {
                Opacity = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Numeric,
                TextColor = Color.Transparent
            };

            c.Children.Add(Entry, 0, 0);
            c.Children.Add(Grid, 0, 0);


            Content = new ContentView { Content = c, BackgroundColor = Color.Transparent };
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var size = e.NewTextValue.Length;
            var write = true;

            if (!string.IsNullOrEmpty(e.OldTextValue) || (size > NumberEntrys))
            {
                write = e.NewTextValue?.Length > e.OldTextValue?.Length;
            }

            var v = new Frame();


            if (write)
            {

                if (size > NumberEntrys)
                {
                    var entry = (Entry)sender;

                    entry.Text = e.OldTextValue;
                }
                else
                {
                    v = Variables[size - 1][size.ToString()] as Frame;

                    var text = e.NewTextValue.LastOrDefault();

                    v.Content = new Label { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Text = text.ToString(), TextColor = Color.Black, FontSize = 32 };

                    Variables[size - 1][size.ToString()] = v;
                }

            }
            else
            {
                if (e.OldTextValue.Length <= NumberEntrys)
                {
                    v = Variables[size][(size + 1).ToString()] as Frame;

                    v.Content = new Label { Text = "", TextColor = Color.Black };
                }
            }

            Text = Entry.Text;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Entry.TextChanged += OnTextChanged;

            var selected = sender as Frame;

            var l = selected.Content as Label;

            var t = l.Text;

            Entry.Focus();
        }
    }
}

