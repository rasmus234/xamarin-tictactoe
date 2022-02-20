using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TicTacToe
{
    public partial class MainPage : ContentPage
    {
        private string _currentSymbol = "X";

        public string CurrentSymbol
        {
            get => _currentSymbol;
            set
            {
                _currentSymbol = value;
                OnPropertyChanged();
            }
        }

        List<Button> buttons;
        private int count;
        private Button[,] board;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            GameGrid.Children.OfType<BoxView>().ForEach(view => view.BackgroundColor = Color.White);

            buttons = GameGrid.Children
                .OfType<Button>()
                .ToList();


            for (var i = 0; i < buttons.Count; i++)
            {
                var index = i;
                var button = buttons[i];
                button.Text = "";
                button.BackgroundColor = Color.White;
                button.Clicked += (sender, args) => Button_OnClicked(sender, args, index);
            }


            board = new[,]
            {
                {buttons[0], buttons[1], buttons[2]},
                {buttons[3], buttons[4], buttons[5]},
                {buttons[6], buttons[7], buttons[8]}
            };
        }

        private void Button_OnClicked(object sender, EventArgs e, int index)
        {
            if (!(sender is Button button)) return;
            Console.Out.WriteLine("Button clicked: " + index);
            button.Text = CurrentSymbol;
            CurrentSymbol = CurrentSymbol == "X" ? "O" : "X";
            count++;
            CheckForWin(board);
        }

        private void ResetGameButton_OnClicked(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            buttons.ForEach(button =>
            {
                button.Text = string.Empty;
                button.BackgroundColor = Color.White;
            });
            CurrentSymbol = new Random().Next(0, 2) == 0 ? "X" : "O";
            count = 0;
        }

        public void DisplayWin(string symbol)
        {
            DisplayAlert("Winner", $"{symbol} wins!", "OK");
            ResetGame();
        }

        private void CheckForWin(Button[,] board)
        {
            // Check for horizontal wins
            for (var i = 0; i < 3; i++)
            {
                if (board[i, 0].Text == board[i, 1].Text && board[i, 1].Text == board[i, 2].Text &&
                    board[i, 0].Text.Length > 0)
                {
                    DisplayWin(board[i, 0].Text);
                }
            }

            // Check for vertical wins
            for (var i = 0; i < 3; i++)
            {
                if (board[0, i].Text == board[1, i].Text && board[1, i].Text == board[2, i].Text &&
                    board[0, i].Text.Length > 0)
                {
                    DisplayWin(board[0, i].Text);
                }
            }

            // Check for diagonal wins
            if (board[0, 0].Text == board[1, 1].Text && board[1, 1].Text == board[2, 2].Text &&
                board[0, 0].Text.Length > 0)
            {
                DisplayWin(board[0, 0].Text);
            }

            if (board[0, 2].Text == board[1, 1].Text && board[1, 1].Text == board[2, 0].Text &&
                board[0, 2].Text.Length > 0)
            {
                DisplayWin(board[0, 2].Text);
            }

            if (count == 9)
            {
                DisplayWin("Tie");
            }
        }
    }
}