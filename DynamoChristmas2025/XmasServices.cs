using Dynamo.Controls;
using Dynamo.Graph.Workspaces;
using Dynamo.Wpf.Extensions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Xmas
{
    internal class XmasLightService
    {
        private readonly ViewLoadedParams _viewLoadedParams;
        private readonly DispatcherTimer _timer;
        private readonly SolidColorBrush[] _christmasBrushes;
        private int _colorIndex = 0;

        private readonly Dictionary<Guid, NodeView> _nodeViewCache = new();
        private WorkspaceModel _currentWorkspace;

        public XmasLightService(ViewLoadedParams viewLoadedParams)
        {
            _viewLoadedParams = viewLoadedParams;

            _christmasBrushes = new[]
            {
                new SolidColorBrush(Color.FromRgb(255, 0, 0)),      
                new SolidColorBrush(Color.FromRgb(0, 200, 0)),    
                new SolidColorBrush(Color.FromRgb(0, 80, 255)),    
                new SolidColorBrush(Color.FromRgb(255, 220, 0)),   
                new SolidColorBrush(Color.FromRgb(255, 255, 255)), 
                new SolidColorBrush(Color.FromRgb(140, 0, 255)),  
                new SolidColorBrush(Color.FromRgb(255, 50, 150)), 
                new SolidColorBrush(Color.FromRgb(180, 0, 180)), 
                new SolidColorBrush(Color.FromRgb(255, 150, 200)), 
                new SolidColorBrush(Color.FromRgb(100, 0, 150)) 
            };

            // Freeze brushes for performance
            foreach (var brush in _christmasBrushes)
            {
                brush.Freeze();
            }

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            _currentWorkspace = _viewLoadedParams.CurrentWorkspaceModel as WorkspaceModel;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _nodeViewCache.Clear();
        }


        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_currentWorkspace == null) return;

            var nodeViews = NodeViewsList(); // we could definitely cache this and update it on node added / node removed events... but this is clearly just for fun :D 

            for (int i = 0;     i < nodeViews.Count; i++)
            {
                var brushIdx = (i + _colorIndex) % _christmasBrushes.Length;
                NodeColorHelpers.SetNodeColor(nodeViews[i], _christmasBrushes[brushIdx]);
            }

            _colorIndex = (_colorIndex + 1) % _christmasBrushes.Length;
        }

        public  List<NodeView> NodeViewsList()
        {
            var allNodeViews = FindVisualChildren<NodeView>(_viewLoadedParams.DynamoWindow);
            return allNodeViews.ToList();
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T item in FindVisualChildren<T>(child))
                {
                    yield return item;
                }
            }
        }

    }
}