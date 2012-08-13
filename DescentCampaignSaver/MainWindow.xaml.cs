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
using DescentCampaignSaver.Descent.Characters;
using DescentCampaignSaver.Descent.Relics;
using DescentCampaignSaver.Descent.SearchItems;
using DescentCampaignSaver.Descent.Shop;
using Mapping;
using Microsoft.Win32;

namespace DescentCampaignSaver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CurrentSavePath;
        private ObservableCollection<Player> players = new ObservableCollection<Player>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CustomCommands.SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CustomCommands.SaveAsCommand.InputGestures.Add(new KeyGesture(Key.S,
                                                                          ModifierKeys.Control | ModifierKeys.Shift));
            CustomCommands.OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));

            File.ReadLines(@"Data\ShopItems.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<ShopItem>()
                .ToList()
                .ForEach(item =>
                {
                    MyResources.shopItems.Add(item);
                    MyResources.allSearchableItems.Add(item);
                });
            File.ReadLines(@"Data\Characters.csv")
                .Select(x => x.Split(',').Select(y => y.Trim('"')).ToArray())
                .MapCsvUsingHeader()
                .Into<DescentCharacter>()
                .ToList()
                .ForEach(item =>
                {
                    MyResources.descentCharacters.Add(item);
                });
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
                players.Add(new Player
                                {
                                    Name = "Player " + i,
                                    CurrentFatigue = 0,
                                    CurrentHealth = 0,
                                    ShopItems = new ObservableCollection<ShopItem>(),
                                    SearchCardItems = new ObservableCollection<SearchCardItem>(),
                                    PlayerRelics = new ObservableCollection<PlayerRelic>(),
                                    ClassAbilites = new ObservableCollection<ClassAbility>(),
                                    Character = new DescentCharacter
                                                    {
                                                        Name = "Ashrian",
                                                        Awareness = 1,
                                                        Knowledge = 2,
                                                        MaxFatigue = 5,
                                                        MaxHealth = 12,
                                                        Might = 4,
                                                        Will = 5
                                                    }
                                });
            }
            SetPlayers(players);
        }

        private void SetPlayers(ObservableCollection<Player> players)
        {
            tc_Players.ItemsSource = players;
            if (tc_Players.HasItems)
                tc_Players.SelectedIndex = 0;
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (dynamic)sender;
            var tBox = sender as TextBox;
            if (tBox.IsReadOnly)
            {
                var tbItem = tb.Parent.TemplatedParent.Parent.Parent.TemplatedParent as TabItem;
                tbItem.Focus();
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Select(0, 0);
            tb.IsReadOnly = true;
            tb.Cursor = Cursors.Arrow;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
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
                    Focus();
                }
            }
        }

        private void AutoCompleteBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var aBox = (AutoCompleteBox)sender;
                Player player = players.FirstOrDefault(x => x.Name == aBox.Tag.ToString());
                if (player != null && aBox.SelectedItem != null)
                {
                    if(aBox.SelectedItem is ShopItem)
                        player.ShopItems.Add((ShopItem) aBox.SelectedItem);
                    else if (aBox.SelectedItem is SearchCardItem)
                        player.SearchCardItems.Add((SearchCardItem) aBox.SelectedItem);
                    else if (aBox.SelectedItem is PlayerRelic)
                        player.PlayerRelics.Add((PlayerRelic) aBox.SelectedItem);
                    else if (aBox.SelectedItem is ClassAbility)
                        player.ClassAbilites.Add((ClassAbility) aBox.SelectedItem);
                }
                aBox.SelectedItem = null;
                aBox.Text = "";
            }
        }

        private void ClearAutoCompleteBoxIfNecessary(AutoCompleteBox acBox)
        {
            if (acBox.Text == "Enter Items, Search Cards, Class Abilites and Relics Here...")
            {
                acBox.Text = "";
                acBox.Foreground = new SolidColorBrush
                                       {
                                           Color = (Color)ColorConverter.ConvertFromString("Black")
                                       };
            }
        }

        private void AutoCompleteBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            ClearAutoCompleteBoxIfNecessary(acBox);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mI = sender as MenuItem;
            if (mI.Header.ToString() == "E_xit")
                Close();
            if (mI.Header.ToString() == "Save _As")
                SaveAsFunction();
            if (mI.Header.ToString() == "_Open")
                OpenFunction();
        }

        private void OpenFunction()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "XML Documents (.xml)|*.xml";
            if (ofd.ShowDialog() == true)
            {
                players = PlayerSerializer.DeSerialize(ofd.FileName);
                SetPlayers(players);
                CurrentSavePath = ofd.FileName;
                SetTitle();
            }
        }
        private void SetTitle()
        {
            if(CurrentSavePath!=null)
                this.Title = Path.GetFileName(CurrentSavePath) + " - Descent 2nd Edition Campaign Saver";
            else 
                this.Title = "Untitled.xml - Descent 2nd Edition Campaign Saver";
        }

        private void SaveFunction()
        {
            if (CurrentSavePath == null)
            {
                SaveAsFunction();
            }
            else
            {
                PlayerSerializer.Serialize(players, CurrentSavePath);
                SetStatusBarMessage("Saved.", 3000);
                SetTitle();
            }
        }

        private string SaveAsFunction()
        {
            var sfd = new SaveFileDialog();
            if (CurrentSavePath != null)
                sfd.FileName = Path.GetFileNameWithoutExtension(CurrentSavePath);
            else
                sfd.FileName = "Untitled";
            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML Documents (.xml)|*.xml";
            if (sfd.ShowDialog() == true)
            {
                PlayerSerializer.Serialize(players, sfd.FileName);
                SetStatusBarMessage("Saved.", 3000);
                CurrentSavePath = sfd.FileName;
                SetTitle();
                return sfd.FileName;
            }
            return null;
        }

        private void CommandSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFunction();
        }

        private void AutoCompleteBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            ClearAutoCompleteBoxIfNecessary(acBox);
        }

        private void CommandSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAsFunction();
        }

        private void CommandOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFunction();
        }

        private void TextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
                e.Cancel = true;
        }

        private void AutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var acBox = sender as AutoCompleteBox;
            acBox.Text = "Enter Items, Search Cards, Class Abilites and Relics Here...";
            acBox.Foreground = new SolidColorBrush(){
                Color =(Color) ColorConverter.ConvertFromString("Gray")
            };
        }

        private void TextBoxClearOnFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.Focus();
            tb.SelectAll();
        }

        private void SetStatusBarMessage(string message, int timeMs)
        {
            tb_StatusBar.Text = message;
            System.Threading.Thread th = new Thread(new ParameterizedThreadStart(ClearStatusBarThreaded));
            th.Start(timeMs);
        }
        private void ClearStatusBarThreaded(object timeMs)
        {
            var time = (int)timeMs;
            System.Threading.Thread.Sleep(time);
            tb_StatusBar.Dispatcher.Invoke(new Action(() => {
                tb_StatusBar.Text = "";
            }));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tb = (dynamic)sender;
            var p1 = tb.Parent.TemplatedParent.Parent.Parent.TemplatedParent.Content;
            players.Remove(p1);
        }
    }
}