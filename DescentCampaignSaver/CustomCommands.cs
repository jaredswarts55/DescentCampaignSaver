using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DescentCampaignSaver
{
    class CustomCommands
    {
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand SaveAsCommand = new RoutedCommand();
        public static RoutedCommand OpenCommand = new RoutedCommand();
    }
}
