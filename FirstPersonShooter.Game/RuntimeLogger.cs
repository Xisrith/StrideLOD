using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;
using Stride.UI.Panels;

namespace FirstPersonShooter
{
    [RequireComponent(typeof(UIComponent))]
    public class RuntimeLogger : StartupScript
    {
        private UIElement _root;

        private ToggleButton _entityTreeTab;
        private StackPanel _entityTreeView;

        private bool _showEntityTreeView;

        public SpriteFont Font { get; set; }

        public override void Start()
        {
            _root = Entity.Get<UIComponent>().Page.RootElement;

            _entityTreeTab = _root.FindName("EntityTreeTab") as ToggleButton;
            _entityTreeView = _root.FindName("EntityTreeView") as StackPanel;

            _entityTreeTab.Checked += (object obj, RoutedEventArgs args) => ShowEntityTreeView();
            _entityTreeTab.Unchecked += (object obj, RoutedEventArgs args) => _entityTreeView.Visibility = Visibility.Collapsed;
        }

        private void ShowEntityTreeView()
        {
            _entityTreeView.Visibility = Visibility.Visible;

            var entities = SceneSystem.SceneInstance.RootScene.Entities;

            _entityTreeView.Children.Clear();
            foreach(var entity in entities)
            {
                _entityTreeView.Children.Add(new ToggleButton
                {
                    State = ToggleState.Checked,
                    Content = new TextBlock
                    {
                        Text = entity.Name,
                        TextColor = Color.Black,
                        TextSize = 16,
                        Font = Font
                    }
                });
            }
        }
    }
}
