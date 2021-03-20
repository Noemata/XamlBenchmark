using System.Diagnostics;
using Microsoft.UI.Xaml;

using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Graphics.Xaml;

using GraphicsTester.Scenarios;

namespace XamlBenchmarkWinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly XamlCanvas canvas = new XamlCanvas();
        private IDrawable drawable;
        const int TestIterations = 300;
        int testIncrement = -1;
        Stopwatch timer = new Stopwatch();

        public MainWindow()
        {
            this.InitializeComponent();
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

        private void OnRendering(object sender, object e)
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

                    Elapsed.Text = GlobalVariables.TotalElapsedMM.ToString();
                    Passes.Text = GlobalVariables.TotalPasses.ToString();

                    testIncrement = -1;
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
