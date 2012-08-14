namespace DescentCampaignSaver
{
    using System.Windows.Input;

    /// <summary>
    /// The custom commands.
    /// </summary>
    internal class CustomCommands
    {
        /// <summary>
        /// The save command.
        /// </summary>
        public static RoutedCommand SaveCommand = new RoutedCommand();

        /// <summary>
        /// The save as command.
        /// </summary>
        public static RoutedCommand SaveAsCommand = new RoutedCommand();

        /// <summary>
        /// The open command.
        /// </summary>
        public static RoutedCommand OpenCommand = new RoutedCommand();
    }
}