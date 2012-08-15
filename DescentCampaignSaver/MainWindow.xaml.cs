namespace DescentCampaignSaver
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using DescentCampaignSaver.Descent;
    using DescentCampaignSaver.Descent.Heroes;
    using DescentCampaignSaver.Descent.Other;
    using DescentCampaignSaver.Descent.Overlord;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.Scenario;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shared;
    using DescentCampaignSaver.Descent.Shop;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// The bound.
        /// </summary>
        private ObservableCollection<ITabular> bound = new ObservableCollection<ITabular>();

        /// <summary>
        /// The campaign.
        /// </summary>
        private DescentCampaign campaign = new DescentCampaign();

        /// <summary>
        /// The current save path.
        /// </summary>
        private string currentSavePath;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class. 
        /// The playersToSet.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The auto complete box got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            this.ClearAutoCompleteBoxIfNecessary(acBox);
        }

        /// <summary>
        /// The auto complete box key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var aBox = (AutoCompleteBox)sender;
                Hero hero = this.campaign.Players.FirstOrDefault(x => x.Name == aBox.Tag.ToString());
                if (hero != null && aBox.SelectedItem != null)
                {
                    if (aBox.SelectedItem is ShopItem)
                    {
                        hero.ShopItems.Add((ShopItem)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is SearchCardItem)
                    {
                        hero.SearchCardItems.Add((SearchCardItem)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is PlayerRelic)
                    {
                        hero.PlayerRelics.Add((PlayerRelic)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is ClassAbility)
                    {
                        hero.ClassAbilites.Add((ClassAbility)aBox.SelectedItem);
                    }
                }

                aBox.SelectedItem = null;
                aBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// The auto complete box lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            acBox.Text = "Enter Items, Search Cards, Class Abilites and Relics Here...";
            acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Gray") };
        }

        /// <summary>
        /// The auto complete box preview key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var aBox = (AutoCompleteBox)sender;
                if (this.campaign.Overlord != null && aBox.SelectedItem != null)
                {
                    if (aBox.SelectedItem is OverlordRelic)
                    {
                        this.campaign.Overlord.OverlordRelics.Add((OverlordRelic)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is OverlordClassAbility)
                    {
                        this.campaign.Overlord.OverlordClassAbilities.Add((OverlordClassAbility)aBox.SelectedItem);
                    }
                }

                aBox.SelectedItem = null;
                aBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// The auto complete box preview mouse down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            this.ClearAutoCompleteBoxIfNecessary(acBox);
        }

        /// <summary>
        /// The auto complete box_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            acBox.Text = "Enter OverlordCharacter Abilities and Runes Here...";
            acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Gray") };
        }

        /// <summary>
        /// The button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var tb = (dynamic)sender;
            dynamic p1 = tb.Parent.TemplatedParent.Parent.Parent.TemplatedParent.Content;
            this.campaign.Players.Remove(p1);
            this.SetCampaign(this.campaign);
        }

        /// <summary>
        /// The clear auto complete box if necessary.
        /// </summary>
        /// <param name="acBox">
        /// The ac box.
        /// </param>
        private void ClearAutoCompleteBoxIfNecessary(AutoCompleteBox acBox)
        {
            if (acBox.Text == "Enter Items, Search Cards, Class Abilites and Relics Here..."
                || acBox.Text == "Enter OverlordCharacter Abilities and Runes Here...")
            {
                acBox.Text = string.Empty;
                acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Black") };
            }
        }

        /// <summary>
        /// The clear status bar threaded.
        /// </summary>
        /// <param name="timeMs">
        /// The time ms.
        /// </param>
        private void ClearStatusBarThreaded(object timeMs)
        {
            var time = (int)timeMs;
            Thread.Sleep(time);
            this.tb_StatusBar.Dispatcher.Invoke(new Action(() => { this.tb_StatusBar.Text = string.Empty; }));
        }

        /// <summary>
        /// The command open executed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CommandOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.OpenFunction();
        }

        /// <summary>
        /// The command save as executed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CommandSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveAsFunction();
        }

        /// <summary>
        /// The command save executed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CommandSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveFunction();
        }

        /// <summary>
        /// The data grid auto generating column.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// The menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            var mI = sender as MenuItem;
            if (mI.Header.ToString() == "E_xit")
            {
                this.Close();
            }

            if (mI.Header.ToString() == "Save _As")
            {
                this.SaveAsFunction();
            }

            if (mI.Header.ToString() == "_Open")
            {
                this.OpenFunction();
            }
        }

        /// <summary>
        /// The open function.
        /// </summary>
        private void OpenFunction()
        {
            var ofd = new OpenFileDialog { Filter = "D2E Campaigns (.d2e)|*.d2e" };
            if (ofd.ShowDialog() == true)
            {
                this.campaign = PlayerSerializer.DeSerialize(ofd.FileName);
                this.SetCampaign(this.campaign);
                this.currentSavePath = ofd.FileName;
                this.SetTitle();
            }
        }

        /// <summary>
        /// The save as function.
        /// </summary>
        private void SaveAsFunction()
        {
            var sfd = new SaveFileDialog
                {
                    FileName =
                        this.currentSavePath != null
                            ? Path.GetFileNameWithoutExtension(this.currentSavePath)
                            : "UntitledCampaign", 
                    DefaultExt = ".d2e", 
                    Filter = "D2E Campaigns (.d2e)|*.d2e"
                };

            if (sfd.ShowDialog() == true)
            {
                PlayerSerializer.Serialize(this.campaign, sfd.FileName);
                this.SetStatusBarMessage("Saved.", 3000);
                this.currentSavePath = sfd.FileName;
                this.SetTitle();
            }
        }

        /// <summary>
        /// The save function.
        /// </summary>
        private void SaveFunction()
        {
            if (this.currentSavePath == null)
            {
                this.SaveAsFunction();
            }
            else
            {
                PlayerSerializer.Serialize(this.campaign, this.currentSavePath);
                this.SetStatusBarMessage("Saved.", 3000);
                this.SetTitle();
            }
        }

        /// <summary>
        /// The set campaign.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        private void SetCampaign(DescentCampaign c)
        {
            this.bound.Clear();
            this.bound.Add(c);
            this.bound.Add(c.Overlord);
            c.Players.ToList().ForEach(player =>
                {
                    player.ClassAbilites.CollectionChanged += this.ExpRecalculation;
                    this.bound.Add(player); 
                });
            this.campaign = c;
            this.campaign.Scenarios.CollectionChanged += this.ExpRecalculation;
            this.tc_Players.ItemsSource = this.bound;
            if (this.tc_Players.HasItems)
            {
                this.tc_Players.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// The set status bar message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="timeMs">
        /// The time ms.
        /// </param>
        private void SetStatusBarMessage(string message, int timeMs)
        {
            this.tb_StatusBar.Text = message;
            var th = new Thread(this.ClearStatusBarThreaded);
            th.Start(timeMs);
        }

        /// <summary>
        /// The set title.
        /// </summary>
        private void SetTitle()
        {
            if (this.currentSavePath != null)
            {
                this.Title = Path.GetFileName(this.currentSavePath) + " - Descent 2nd Edition Campaign Saver";
            }
            else
            {
                this.Title = "Untitled.d2e - Descent 2nd Edition Campaign Saver";
            }
        }

        /// <summary>
        /// The text box clear on focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxClearOnFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

        /// <summary>
        /// The text box got mouse capture.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

        /// <summary>
        /// The text box key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                if (tb.IsReadOnly != true)
                {
                    tb.Select(0, 0);
                    tb.IsReadOnly = true;
                    tb.Cursor = Cursors.Arrow;
                    tb.CaretIndex = 0;
                    this.Focus();
                }
            }
        }

        /// <summary>
        /// The text box lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Select(0, 0);
            tb.IsReadOnly = true;
            tb.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// The text box mouse double click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.IsReadOnly && tb.DataContext is Hero)
            {
                tb.IsReadOnly = false;
                e.Handled = true;
                tb.Cursor = Cursors.IBeam;
                tb.SelectAll();
            }
        }

        /// <summary>
        /// The text box mouse down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (dynamic)sender;
            var tBox = sender as TextBox;
            if (tBox.IsReadOnly)
            {
                var tbItem = tb.Parent.TemplatedParent.Parent.Parent.TemplatedParent as TabItem;
                tbItem.Focus();
            }
        }

        /// <summary>
        /// The window loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            CustomCommands.SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

            CustomCommands.SaveAsCommand.InputGestures.Add(
                new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));

            CustomCommands.OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));

            this.campaign.Overlord = new OverlordCharacter()
                {
                    OverlordClassAbilities = new ObservableCollection<OverlordClassAbility>(), 
                    OverlordRelics = new ObservableCollection<OverlordRelic>(), 
                    UnspentExp = 0
                };
            this.campaign.UnspentPlayerGold = 0;
            this.campaign.Players = new ObservableCollection<Hero>();

            File.ReadLines(@"Data\ShopItems.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<ShopItem>().ToList().ForEach(
                    item =>
                        {
                            MyResources.shopItems.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\Characters.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<DescentCharacter>().ToList().ForEach(
                    item => MyResources.descentCharacters.Add(item));

            File.ReadLines(@"Data\ClassAbilities.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<ClassAbility>().ToList().ForEach(
                    item =>
                        {
                            MyResources.classAbilities.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\PlayerRelics.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<PlayerRelic>().ToList().ForEach(
                    item =>
                        {
                            MyResources.playerRelics.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\OverlordRelics.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<OverlordRelic>().ToList().ForEach(
                    item =>
                        {
                            MyResources.overlordRelics.Add(item);
                            MyResources.overlordSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\OverlordAbilites.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<OverlordClassAbility>().ToList().ForEach(
                    item =>
                        {
                            MyResources.overlordClassAbilities.Add(item);
                            MyResources.overlordSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\SearchCardItems.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<SearchCardItem>().ToList().ForEach(
                    item =>
                        {
                            MyResources.searchCards.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\Scenario.csv").Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray()).
                MapCsvUsingHeader().Into<Scenario>().ToList().ForEach(
                    item => MyResources.scenarios.Add(item));

            campaign.Scenarios = MyResources.scenarios;


            MyResources.itemNames = MyResources.shopItems.Select(x => x.Name).ToList();
            for (int i = 1; i <= 4; i++)
            {
                this.campaign.Players.Add(
                    new Hero
                        {
                            Name = "Hero " + i, 
                            CurrentFatigue = 0,
                            CurrentHealth = 0, 
                            ShopItems = new ObservableCollection<ShopItem>(), 
                            SearchCardItems = new ObservableCollection<SearchCardItem>(), 
                            PlayerRelics = new ObservableCollection<PlayerRelic>(), 
                            ClassAbilites = new ObservableCollection<ClassAbility>()
                        });
            }
            this.SetCampaign(this.campaign);
        }

        void ExpRecalculation(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.campaign.RecalculateExperience();
        }

        #endregion

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            this.campaign.RecalculateExperience();
        }
    }
}