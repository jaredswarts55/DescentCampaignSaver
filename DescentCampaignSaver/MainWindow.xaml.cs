namespace DescentCampaignSaver
{
    using System;
    using System.Collections.Generic;
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
    using DescentCampaignSaver.Descent.Characters;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shop;

    using Mapping;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// The current save path.
        /// </summary>
        private string currentSavePath;

        private DescentCampaign campaign = new DescentCampaign();

        /// <summary>
        /// The playersToSet.
        /// </summary>

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods
        private void AutoCompleteBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var aBox = (AutoCompleteBox)sender;
                if (campaign.Overlord != null && aBox.SelectedItem != null)
                {
                    if (aBox.SelectedItem is OverlordRelic)
                        campaign.Overlord.OverlordRelics.Add((OverlordRelic)aBox.SelectedItem);
                    else if (aBox.SelectedItem is OverlordClassAbility)
                        campaign.Overlord.OverlordClassAbilities.Add((OverlordClassAbility)aBox.SelectedItem);
                }

                aBox.SelectedItem = null;
                aBox.Text = string.Empty;
            }
        }

        private void AutoCompleteBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var aBox = (AutoCompleteBox)sender;
                Player player = this.campaign.Players.FirstOrDefault(x => x.Name == aBox.Tag.ToString());
                if (player != null && aBox.SelectedItem != null)
                {
                    if (aBox.SelectedItem is ShopItem)
                    {
                        player.ShopItems.Add((ShopItem)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is SearchCardItem)
                    {
                        player.SearchCardItems.Add((SearchCardItem)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is PlayerRelic)
                    {
                        player.PlayerRelics.Add((PlayerRelic)aBox.SelectedItem);
                    }
                    else if (aBox.SelectedItem is ClassAbility)
                    {
                        player.ClassAbilites.Add((ClassAbility)aBox.SelectedItem);
                    }
                }

                aBox.SelectedItem = null;
                aBox.Text = string.Empty;
            }
        }

        private void AutoCompleteBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            this.ClearAutoCompleteBoxIfNecessary(acBox);
        }

        private void AutoCompleteBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            this.ClearAutoCompleteBoxIfNecessary(acBox);
        }

        private void AutoCompleteBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            acBox.Text = "Enter Items, Search Cards, Class Abilites and Relics Here...";
            acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Gray") };
        }


        private void AutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            acBox.Text = "Enter OverlordCharacter Abilities and Runes Here...";
            acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Gray") };
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var tb = (dynamic)sender;
            dynamic p1 = tb.Parent.TemplatedParent.Parent.Parent.TemplatedParent.Content;
            this.campaign.Players.Remove(p1);
        }

        private void ClearAutoCompleteBoxIfNecessary(AutoCompleteBox acBox)
        {
            if (acBox.Text == "Enter Items, Search Cards, Class Abilites and Relics Here..." || acBox.Text == "Enter OverlordCharacter Abilities and Runes Here...")
            {
                acBox.Text = string.Empty;
                acBox.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Black") };
            }
        }

        private void ClearStatusBarThreaded(object timeMs)
        {
            var time = (int)timeMs;
            Thread.Sleep(time);
            this.tb_StatusBar.Dispatcher.Invoke(new Action(() => { this.tb_StatusBar.Text = string.Empty; }));
        }

        private void CommandOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.OpenFunction();
        }

        private void CommandSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveAsFunction();
        }

        private void CommandSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveFunction();
        }

        private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
            {
                e.Cancel = true;
            }
        }

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

        private void SetCampaign(DescentCampaign c)
        {
            this.tc_Players.ItemsSource = c.Players;
            if (this.tc_Players.HasItems)
            {
                this.tc_Players.SelectedIndex = 0;
            }
            ic_Campaign.DataContext = c;
            this.campaign = c;
        }

        private void SetStatusBarMessage(string message, int timeMs)
        {
            this.tb_StatusBar.Text = message;
            var th = new Thread(this.ClearStatusBarThreaded);
            th.Start(timeMs);
        }

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

        private void TextBoxClearOnFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

        private void TextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

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

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Select(0, 0);
            tb.IsReadOnly = true;
            tb.Cursor = Cursors.Arrow;
        }

        private void TextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.IsReadOnly)
            {
                tb.IsReadOnly = false;
                e.Handled = true;
                tb.Cursor = Cursors.IBeam;
                tb.SelectAll();
            }
        }

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

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            CustomCommands.SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

            CustomCommands.SaveAsCommand.InputGestures.Add(
                new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));

            CustomCommands.OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));

            campaign.Overlord = new OverlordCharacter()
            {
                OverlordClassAbilities = new ObservableCollection<OverlordClassAbility>(),
                OverlordRelics = new ObservableCollection<OverlordRelic>(),
                UnspentExp = 0
            };
            campaign.UnspentPlayerGold = 0;
            campaign.Players = new ObservableCollection<Player>();

            ic_Campaign.DataContext = campaign;

            File.ReadLines(@"Data\ShopItems.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader().Into<ShopItem>()
                .ToList()
                .ForEach(item =>
                        {
                            MyResources.shopItems.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\Characters.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader().Into<DescentCharacter>()
                .ToList()
                .ForEach(item => MyResources.descentCharacters.Add(item));

            File.ReadLines(@"Data\ClassAbilities.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<ClassAbility>()
                .ToList()
                .ForEach(item =>
                        {
                            MyResources.classAbilities.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\PlayerRelics.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<PlayerRelic>()
                .ToList()
                .ForEach(item =>
                        {
                            MyResources.playerRelics.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });
            File.ReadLines(@"Data\OverlordRelics.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<OverlordRelic>()
                .ToList()
                .ForEach(item =>
                {
                    MyResources.overlordRelics.Add(item);
                    MyResources.overlordSearchableItems.Add(item);
                });

            File.ReadLines(@"Data\SearchCardItems.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<SearchCardItem>()
                .ToList()
                .ForEach(item =>
                        {
                            MyResources.searchCards.Add(item);
                            MyResources.allSearchableItems.Add(item);
                        });

            MyResources.itemNames = MyResources.shopItems.Select(x => x.Name).ToList();
            for (int i = 1; i <= 4; i++)
            {
                this.campaign.Players.Add(
                    new Player
                        {
                            Name = "Player " + i, 
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

        #endregion



    }
}