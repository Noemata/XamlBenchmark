using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Xaml;

using GraphicsTester.Scenarios;

namespace XamlBenchmarkWinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly XamlCanvas canvas = new XamlCanvas();
        private IDrawable drawable;
        const int TestIterations = GlobalVariables.TestCount;
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

        private void AddToClipboard()
        {
            DataPackageView dataPkgView = Clipboard.GetContent();

            if (dataPkgView.Contains(StandardDataFormats.Text))
            {
                // MP! fixme: This code fails in WinUI??
                Task<string> task = dataPkgView.GetTextAsync().AsTask();
                try { task.RunSynchronously(); } catch (InvalidOperationException) { }
                string fullResult = task.Result;
                dataPkgView.ReportOperationCompleted(DataPackageOperation.Copy);

                string testResult = $"( WinUI ) Elapsed: {Elapsed.Text}, Passes: {Passes.Text}";

                var dataPkg = new DataPackage { RequestedOperation = DataPackageOperation.Copy };

                if (string.IsNullOrEmpty(fullResult))
                    dataPkg.SetText($"{testResult}\n");
                else
                    dataPkg.SetText($"{fullResult}\n{testResult}\n");

                Clipboard.SetContent(dataPkg);
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
