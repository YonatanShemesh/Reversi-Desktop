using System;
using System.Drawing;
using System.Windows.Forms;
using Engien;

namespace UI
{
    public partial class GameForm : Form
    {
        private readonly Bitmap r_CoinRed = UI.Properties.Resources.CoinRed;
        private readonly Bitmap r_CoinYellow = UI.Properties.Resources.CoinYellow;
        private readonly int m_FormSize;
        private readonly Button[,] m_buttons;
        private readonly int r_ButtonSize = 50;
        private GameEngien m_GameEngien;
        private GameBoard.eBoardDemantions m_BoardSize;
        private GameEngien.eGameMode m_GameMode;
        private int s_ButtonNameCounter = 0;
        private int m_Player1Wins = 0;
        private int m_Player2Wins = 0;

        public GameForm(int i_BoardSize, GameEngien.eGameMode i_GameSetting)
        {
            InitializeComponent();
            m_FormSize = (r_ButtonSize * i_BoardSize) + 20;
            m_buttons = new Button[i_BoardSize, i_BoardSize];
            this.ClientSize = new System.Drawing.Size(m_FormSize, m_FormSize);
            setButtoms(i_BoardSize);
            m_BoardSize = (GameBoard.eBoardDemantions)i_BoardSize;
            m_GameMode = i_GameSetting;
            m_GameEngien = new GameEngien(m_GameMode, m_BoardSize);
            m_GameEngien.Flip += buttonFlip;
            m_GameEngien.Set += buttonSet;
            m_GameEngien.GameOver += gameOver;
            makeFirstLayout();
            m_GameEngien.ShowOptions();
            this.Text = "Othello - Yellow's turn";
        }

        public GameEngien.eGameMode GameSetting { get => m_GameMode; set => m_GameMode = value; }

        public GameBoard.eBoardDemantions BoardSize { get => m_BoardSize; set => m_BoardSize = value; }

        private void makeFirstLayout()
        {
            resetButtons();

            m_buttons[((int)m_BoardSize / 2) - 1, ((int)m_BoardSize / 2) - 1].BackgroundImage = r_CoinRed;
            m_buttons[((int)m_BoardSize / 2) - 1, ((int)m_BoardSize / 2) - 1].BackgroundImageLayout = ImageLayout.Stretch;

            m_buttons[((int)m_BoardSize / 2) - 1, ((int)m_BoardSize / 2)].BackgroundImage = r_CoinYellow;
            m_buttons[((int)m_BoardSize / 2) - 1, ((int)m_BoardSize / 2)].BackgroundImageLayout = ImageLayout.Stretch;

            m_buttons[((int)m_BoardSize / 2), ((int)m_BoardSize / 2) - 1].BackgroundImage = r_CoinYellow;
            m_buttons[((int)m_BoardSize / 2), ((int)m_BoardSize / 2) - 1].BackgroundImageLayout = ImageLayout.Stretch;

            m_buttons[((int)m_BoardSize / 2), ((int)m_BoardSize / 2)].BackgroundImage = r_CoinRed;
            m_buttons[((int)m_BoardSize / 2), ((int)m_BoardSize / 2)].BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void resetButtons()
        {
            for (int y = 0; y < (int)m_BoardSize; y++)
            {
                for (int x = 0; x < (int)m_BoardSize; x++)
                {
                    m_buttons[y, x].BackgroundImage = null;
                }
            }
        }

        private void setButtoms(int i_BoardSize)
        {
            for (int y = 0; y < i_BoardSize; y++)
            {
                for (int x = 0; x < i_BoardSize; x++)
                {
                    m_buttons[y, x] = new Button();
                    m_buttons[y, x].Width = r_ButtonSize;
                    m_buttons[y, x].Height = r_ButtonSize;
                    m_buttons[y, x].Left = this.Left + 10 + (x * m_buttons[y, x].Width);
                    m_buttons[y, x].Top = this.Top + 10 + (y * m_buttons[y, x].Height);
                    m_buttons[y, x].Name = "Button" + s_ButtonNameCounter.ToString();
                    m_buttons[y, x].BackColor = Color.LightGray;
                    m_buttons[y, x].Enabled = false;
                    m_buttons[y, x].Click += Buttons_Click;

                    Controls.Add(m_buttons[y, x]);
                    s_ButtonNameCounter++;
                }
            }
        }

        private void buttonFlip(Engien.Point i_ButtonLocation)
        {
            if (m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImage == r_CoinYellow)
            {
                m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImage = r_CoinRed;
                m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImage = r_CoinYellow;
                m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void buttonSet(Engien.Point i_ButtonLocation, GameEngien.ePlayers i_PlayerType)
        {
            switch (i_PlayerType)
            {
                case GameEngien.ePlayers.FirstPlayer:
                    {
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImage = r_CoinYellow;
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImageLayout = ImageLayout.Stretch;
                        break;
                    }

                case GameEngien.ePlayers.SecondPlayer:
                    {
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImage = r_CoinRed;
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackgroundImageLayout = ImageLayout.Stretch;
                        break;
                    }

                case GameEngien.ePlayers.PossibleMove:
                    {
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackColor = Color.Green;
                        m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].Enabled = true;
                        break;
                    }

                case GameEngien.ePlayers.Empty:
                    {
                        if (m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackColor == Color.Green)
                        {
                            m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].BackColor = Color.LightGray;
                            m_buttons[i_ButtonLocation.y, i_ButtonLocation.x].Enabled = false;
                        }

                        break;
                    }
            }
        }

        private void gameOver(string i_Messege)
        {
            string msg;
            msg = createEndMsg();

            DialogResult dialogResult = MessageBox.Show(msg, "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                restartGame();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }

        private string createEndMsg()
        {
            string msg;
            if (m_GameEngien.Player1Score > m_GameEngien.Player2Score)
            {
                m_Player1Wins++;
                msg = string.Format(
@"Yellow Won!! ({0}\{1}) ({2}\{3})
Would you like another round?",
m_GameEngien.Player1Score,
m_GameEngien.Player2Score,
m_Player1Wins,
m_Player2Wins);
            }
            else if (m_GameEngien.Player1Score < m_GameEngien.Player2Score)
            {
                m_Player2Wins++;
                msg = string.Format(
@"Red Won!! ({0}\{1}) ({2}\{3})
Would you like another round?",
                    m_GameEngien.Player2Score,
                    m_GameEngien.Player1Score,
                    m_Player1Wins,
                    m_Player2Wins);
            }
            else
            {
                msg = string.Format(
@"Draw!! ({0}\{1}) ({2}\{3})
Would you like another round?",
                    m_GameEngien.Player1Score,
                    m_GameEngien.Player2Score,
                    m_Player1Wins,
                    m_Player2Wins);
            }

            return msg;
        }

        private void restartGame()
        {
            m_GameEngien = new GameEngien(m_GameMode, m_BoardSize);
            m_GameEngien.Flip += buttonFlip;
            m_GameEngien.Set += buttonSet;
            m_GameEngien.GameOver += gameOver;
            makeFirstLayout();
            m_GameEngien.ShowOptions();
            this.Text = "Othello - Yellow's turn";
        }

        private void setTitle()
        {
            string title;
            GameEngien.ePlayers currenPlayer = m_GameEngien.CurrentPlayer;
            if (currenPlayer == GameEngien.ePlayers.FirstPlayer)
            {
                title = "Othello - Red's turn";
            }
            else
            {
                title = "Othello - Yellow's turn";
            }

            this.Text = title;
        }

        private void Buttons_Click(object sender, EventArgs e)
        {
            bool aiTurn = false;
            Engien.Point location = null;

            location = GetXyCord(sender, location);

            do
            {
                setTitle();
                m_GameEngien.NextMove(location);
                aiTurn = m_GameEngien.ShowOptions();
            }
            while (aiTurn);
        }

        private Engien.Point GetXyCord(object sender, Engien.Point location)
        {
            for (int y = 0; y < (int)BoardSize; y++)
            {
                for (int x = 0; x < (int)BoardSize; x++)
                {
                    if (m_buttons[y, x] == (sender as Button))
                    {
                        location = new Engien.Point(x, y);
                        break;
                    }
                }
            }

            return location;
        }
    }
}
