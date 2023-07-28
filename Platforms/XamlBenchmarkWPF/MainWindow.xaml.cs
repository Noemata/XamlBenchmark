using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Xaml;

using GraphicsTester.Scenarios;

namespace XamlBenchmarkWPF
{
    public partial class MainWindow : Window
    {
        private readonly XamlCanvas canvas = new XamlCanvas();
        private IDrawable drawable;
        const int TestIterations = GlobalVariables.TestCount;
        int testIncrement = -1;
        Stopwatch timer = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            canvas.Canvas = Canvas;

            foreach (var scenario in ScenarioList.Scenarios)
            {
                List.Items.Add(scenario);
            }

            List.SelectedIndex = 0;
            List.SelectionChanged += (source, args) => Drawable = List.SelectedItem as IDrawable;
            Drawable = List.SelectedItem as IDrawable;
            this.SizeChanged += (source, args) => Draw();

            CompositionTarget.Rendering += OnRendering;
        }

        public IDrawable Drawable
        {
            get => drawable;
            set
            {
                drawable = value;
                Draw();
            }
        }

        private void Draw()
        {
            if (drawable != null)
            {
                using (canvas.CreateSession())
                {
                    drawable.Draw(canvas, new RectangleF(0, 0, (float)Canvas.Width, (float)Canvas.Height));
                }
            }
        }

        private void AddToClipboard()
        {
            string fullResult = Clipboard.GetText();
            string testResult = $"(  WPF  ) Elapsed: {Elapsed.Text}, Passes: {Passes.Text}";

            if (string.IsNullOrEmpty(fullResult))
                Clipboard.SetDataObject($"{testResult}\n");
            else
                Clipboard.SetDataObject($"{fullResult}\n{testResult}");
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (testIncrement != -1)
            {
                int step = testIncrement++ % 3;

                switch (step)
                {
                    case 0:
                        List.SelectedIndex = ScenarioList.Scenarios.Count - 1;
                        break;
                    case 1:
                        List.SelectedIndex = 9;
                        break;

                    case 2:
                        List.SelectedIndex = 27;
                        break;
                }

                Drawable = List.SelectedItem as IDrawable;

                if (testIncrement >= TestIterations)
                {
                    timer.Stop();
                    GlobalVariables.TotalElapsedMM = timer.ElapsedMilliseconds;

                    Elapsed.Text = $"{GlobalVariables.TotalElapsedMM} ms";
                    Passes.Text = $"{GlobalVariables.TotalPasses}";

                    testIncrement = -1;

                    AddToClipboard();
                }
            }
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (testIncrement == -1)
            {
                testIncrement = 0;
                GlobalVariables.TotalPasses = 0;
                GlobalVariables.TotalElapsedMM = 0;
                timer.Reset();
                timer.Start();
            }
            else
            {
                testIncrement = -1;
                timer.Stop();
            }
        }
    }
}
