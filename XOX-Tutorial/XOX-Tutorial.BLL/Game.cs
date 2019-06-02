using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOX_Tutorial.BLL
{
    public delegate void XOXGameHandler(Game game);
    public delegate void XOXGameOverHandler(Game game, List<List<int>> winningConditions);

    public class Game
    {
        public event XOXGameHandler GameStarted;
        public event XOXGameHandler TurnChanged;
        public event XOXGameOverHandler GameOver;

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player ActivePlayer { get; set; }
        public bool IsOver { get; set; }
        public bool IsDraw { get; set; }
        public int TotalMoves { get; set; }

        public Game()
        {
            Player1 = new Player("Player-1", "X");
            Player2 = new Player("Player-2", "O");
        }

        private List<List<int>> gameOverConditions = new List<List<int>>()
        {
            new List<int>(){0,1,2},
            new List<int>(){3,4,5},
            new List<int>(){6,7,8},
            new List<int>(){0,3,6},
            new List<int>(){1,4,7},
            new List<int>(){2,5,8},
            new List<int>(){0,4,8},
            new List<int>(){2,4,6}
        };

        private string[] symbolArray = new string[9];

        public void StartGame()
        {
            ActivePlayer = Player1;
            GameStarted(this);
        }

        public void PlayTurn(int fieldIndex)
        {
            if(ActivePlayer == Player1)
            {
                symbolArray[fieldIndex] = Player1.Symbol;
                TotalMoves++;
                if (IsGameOver(fieldIndex))
                {
                    GameOver(this, GetWinningConditions());
                    return;
                }
            }
            else
            {
                symbolArray[fieldIndex] = Player2.Symbol;
                TotalMoves++;
                if (IsGameOver(fieldIndex))
                {
                    GameOver(this, GetWinningConditions());
                    return;
                }
            }
            ActivePlayer = ActivePlayer == Player1 ? Player2 : Player1;
            TurnChanged(this);
        }

        private List<List<int>> GetWinningConditions()
        {
            List<List<int>> result = new List<List<int>>();
            foreach(var condition in gameOverConditions)
            {
                if(symbolArray[condition[0]]==symbolArray[condition[1]] && symbolArray[condition[0]] == symbolArray[condition[2]])
                {
                    result.Add(condition);
                }
            }
            return result;
        }

        private bool IsGameOver(int fieldIndex)
        {
            foreach (var condition in gameOverConditions.Where(x => x.Contains(fieldIndex)))
            {
                if (symbolArray[condition[0]] == symbolArray[condition[1]] && symbolArray[condition[0]] == symbolArray[condition[2]])
                {
                    IsOver = true;
                    IsDraw = false;
                    break;
                }
                else if(symbolArray[condition[0]] == symbolArray[condition[1]] &&
                     symbolArray[condition[1]] == symbolArray[condition[2]] && TotalMoves == 9)
                {
                    IsOver = true;
                    IsDraw = false;
                    break;
                }
                else if(symbolArray[condition[0]] != symbolArray[condition[1]] &&
                     symbolArray[condition[1]] != symbolArray[condition[2]] && TotalMoves == 9)
                {
                    IsOver = true;
                    IsDraw = true;
                    break;
                }
            }
            return IsOver;
        }
    }
}
