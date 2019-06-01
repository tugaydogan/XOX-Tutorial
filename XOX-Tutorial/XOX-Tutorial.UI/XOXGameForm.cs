using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XOX_Tutorial.BLL;

namespace XOX_Tutorial.UI
{
    public partial class XOXGameForm : Form
    {
        public XOXGameForm()
        {
            InitializeComponent();
        }
        private Game game;

        private void btn_Start_Click(object sender, EventArgs e)
        {
            game = new Game();
            game.GameStarted += new XOXGameHandler(game_Started);
            game.TurnChanged += new XOXGameHandler(turn_Changed);
            game.GameOver += new XOXGameOverHandler(game_Over);
            game.StartGame();
            btn_Start.Enabled = false;
        }

        private void game_Started(Game game)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(100, 100);
                    btn.Location = new Point(105 * j + 10, 105 * i + 10);
                    btn.Tag = 3 * i + j;
                    btn.Click += Btn_Click;
                    this.grp_Game.Controls.Add(btn);
                }
            }
            lbl_player1.Text = game.Player1.Name + " : " + game.Player1.Score;
            lbl_player2.Text = game.Player2.Name + " : " + game.Player2.Score;
            changeActiveLabel();
        }

        private void changeActiveLabel()
        {
            if (game.ActivePlayer == game.Player1)
            {
                lbl_player1.Font = new Font("", 15);
                lbl_player2.Font = default(Font);
            }
            else
            {
                lbl_player1.Font = default(Font);
                lbl_player2.Font = new Font("", 15);
            }
        }

        private void Btn_Click(object sender,EventArgs e)
        {
            var btn = (sender as Button);
            var fieldIndex = Convert.ToInt32(btn.Tag);
            btn.Text = game.ActivePlayer.Symbol;
            game.PlayTurn(fieldIndex);
        }

        private void turn_Changed(Game game)
        {
            changeActiveLabel();
        }

        private void game_Over(Game game,List<List<int>> winningConditions)
        {
            string winnerPlayer="";
            game.ActivePlayer.WinTheGame();
            lbl_player1.Text = game.Player1.Name + " : " + game.Player1.Score;
            lbl_player2.Text = game.Player2.Name + " : " + game.Player2.Score;
            foreach(var condition in winningConditions)
            {
                grp_Game.Controls[condition[0]].BackColor = Color.Red;
                grp_Game.Controls[condition[1]].BackColor = Color.Red;
                grp_Game.Controls[condition[2]].BackColor = Color.Red;
            }
            
            if (game.IsOver)
            {
                DisableButtons();
                if(game.ActivePlayer == game.Player1)
                {
                    winnerPlayer = game.Player1.Name;
                }
                else if (game.ActivePlayer == game.Player2)
                {
                    winnerPlayer = game.Player2.Name;
                }
                else
                {
                    MessageBox.Show("Draw");
                }
            }
            PlayAgain(winnerPlayer);
        }

        private void RestartGame()
        {
            grp_Game.Controls.Clear();
            game_Started(game);
        }

        private void DisableButtons()
        {
            foreach (var item in grp_Game.Controls)
            {
                Button btn = item as Button;
                if (btn != null)
                {
                    btn.Enabled = false;
                }
            }
        }

        private void PlayAgain(string winnerPlayer)
        {
            var result = MessageBox.Show($"Game over. Winner = {winnerPlayer}", "Do you want to play again?", MessageBoxButtons.YesNo);
            if(result== DialogResult.Yes)
            {
                Application.Restart();
                RestartGame();
                btn_Start.Enabled = true;
            }
            else if (result == DialogResult.No)
            {
                Environment.Exit(1);
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
