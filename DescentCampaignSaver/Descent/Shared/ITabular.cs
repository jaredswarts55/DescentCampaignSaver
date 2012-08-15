namespace DescentCampaignSaver.Descent.Shared
{
    using System.Windows;

    /// <summary>
    /// The Tabular interface.
    /// </summary>
    public interface ITabular
    {
        #region Public Properties

        /// <summary>
        /// Gets the image.
        /// </summary>
        string Image { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        Visibility IsClosable { get;}

        #endregion
    }
}