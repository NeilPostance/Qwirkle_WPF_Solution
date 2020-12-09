using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;
//using System.Windows.Forms;


namespace Qwirkle_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DataTable dt = new DataTable();
        

        public MainWindow()
        {
            InitializeComponent();
            InitGameBasics();
            
        }

        private void InitGameBasics() 
        {
            FillTheBag_Click(new object(), new RoutedEventArgs());
            GenerateEmptyBoard(dt);
            ShowGameBoard();

        }

        private void CreatePlayers_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(PlayerCount.Text, out int playerCount) == true) 
            {
                Task.Run(() =>
                {
                    //MessageBox.Show($"Generating {playerCount} players");
                    MainClass.InitializePlayers(playerCount);
                    //MessageBox.Show($"Generated {Game.ListOfPlayers.Count}");
                    Game.DeterminePlayerOrder();
                    ShowPlayers();
                    ListTilesInCurrentPlayerHand();
                    ShowScores();
                    ShowTileCount();
                    Update_lblCurrentPlayer();
                });
            }
            else
            {
                System.Windows.MessageBox.Show("You must enter an integer");
            }

            
        }

        private void Update_lblCurrentPlayer()
        {
            lblCurrentPlayer.Dispatcher.Invoke(() =>
            {
                lblCurrentPlayer.Content = Game.CurrentPlayer.Name;
            });
        }

        private void ShowPlayers()
        {
            PlayerList.Dispatcher.Invoke(() =>
            {
                PlayerList.ItemsSource = Game.ListOfOrderedPlayers;
            });
            
        }

        private void ShowScores()
        {
            DataTable dtPlayerScores = new DataTable();
            if (dtPlayerScores.Columns.Count == 0)
            {
                dtPlayerScores.Columns.Add("Name");
                dtPlayerScores.Columns.Add("Score");
            }
            dtPlayerScores.Rows.Clear();

            foreach (Player player in Game.ListOfOrderedPlayers)
            {
                dtPlayerScores.Rows.Add(player.Name, player.Score);
            }

            PlayerScores.Dispatcher.Invoke(() =>
            {
                PlayerScores.ItemsSource = dtPlayerScores.DefaultView;
            });
            
            
        }

        private void CountTheTiles_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                ShowTileCount();
            });
            
        }

        private void ShowTileCount() 
        {
            BagTileCount.Dispatcher.Invoke(() =>
            {
                BagTileCount.Text = $"The bag contains {Bag.tilesInBag.Count} tiles";
            });
            
        }

        private void FillTheBag_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Bag.FillTheBag(6, 6, 3);
            });
            
            
            ShowTileCount();
        }

        private void PlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update_ListTilesInHand((Player)PlayerList.SelectedItem);
        }

        private void Update_ListTilesInHand(Player player)
        {
            if (player != null)
            {
                TilesInHand.Dispatcher.Invoke(() =>
                {
                    TilesInHand.ItemsSource = player.tilesInHand;
                });
            }   
        }

        private void GenerateEmptyBoard(DataTable dt)
        {
            for (int col = 0; col < MainClass.GridColumns; col++)
            {
                dt.Columns.Add(col.ToString());
            }

            for (int row = 0; row < MainClass.GridRows; row++)
            {
                dt.Rows.Add();
            }
        }

        private void ShowGameBoard()
        {
            foreach(DataRow currentDataRow in dt.Rows)
            {
                for (int col = 0; col < MainClass.GridColumns; col++)
                {
                    Tile tile = new Tile(EnumColour.None, EnumShape.None);
                    tile = Grid.GetTile(dt.Rows.IndexOf(currentDataRow), col);
                    
                    currentDataRow[col] = Grid.GetPositionContent(dt.Rows.IndexOf(currentDataRow), col, out EnumColour outColour, out EnumShape outShape);
                    
                    
                }
            }
            dgGameBoard.Dispatcher.Invoke(() =>
            {
                dgGameBoard.ItemsSource = dt.DefaultView;
            });
            
        }

        private void PlaceTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgGameBoard.SelectedCells[0].IsValid)
                {
                    if (Game.CurrentPlayer.PlaceTile((Tile)TilesInCurrentPlayerHand.SelectedItem, dgGameBoard.Items.IndexOf(dgGameBoard.SelectedCells[0].Item), dgGameBoard.SelectedCells[0].Column.DisplayIndex)==false)
                    {
                        MessageBox.Show("Invalid tile placement");
                    }
                   
                        
                }

                ShowGameBoard();
                
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Select a cell first!");
            }
            
                
            
        }

        private void ListTilesInCurrentPlayerHand()
        {
            TilesInCurrentPlayerHand.Dispatcher.Invoke(() =>
            {
                TilesInCurrentPlayerHand.ItemsSource = Game.CurrentPlayer.tilesInHand;
            });
            
        }

            
        
        private void dgGameBoard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            

            int colIndex = dgGameBoard.SelectedCells[0].Column.DisplayIndex;
            int rowIndex = dgGameBoard.Items.IndexOf(dgGameBoard.SelectedCells[0].Item);
            lblCurrentCell.Content = colIndex + ":" + rowIndex;
            

           

        }

        private void dgGameBoard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int colIndex = dgGameBoard.SelectedCells[0].Column.DisplayIndex;
            int rowIndex = dgGameBoard.Items.IndexOf(dgGameBoard.SelectedCells[0].Item);
            var dgc = (DataGridCell)dgGameBoard.SelectedCells[0].Item;
            dgc.Background = Brushes.Blue;
            //dgGameBoard.b
            lblCurrentCell.Content = colIndex + ":" + rowIndex;
        }



        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Game.EndTurn();
                ShowPlayers();
                ShowScores();
                ListTilesInCurrentPlayerHand();
                PlayerList.Dispatcher.Invoke(() =>
                {
                    Update_ListTilesInHand((Player)PlayerList.SelectedItem);
                });
                
                ShowGameBoard();
                Update_lblCurrentPlayer();
                ShowTileCount();
            });
        }
    }

}

