using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Engien;

namespace UI
{
    public partial class FormGameSetting : Form
    {
        private GameBoard.eBoardDemantions m_GameSetting = GameBoard.eBoardDemantions.Six;

        public FormGameSetting()
        {
            InitializeComponent();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            changeGameSetting();
        }

        private void buttonPvC_Click(object sender, EventArgs e)
        {
            gameStart((int)m_GameSetting, GameEngien.eGameMode.PvC);
        }

        private void buttonPvP_Click(object sender, EventArgs e)
        {
            gameStart((int)m_GameSetting, GameEngien.eGameMode.PvP);
        }

        private void changeGameSetting()
        {
            if (m_GameSetting == GameBoard.eBoardDemantions.Tweleve)
            {
                m_GameSetting = GameBoard.eBoardDemantions.Six;
            }
            else
            {
                m_GameSetting = m_GameSetting + 2;
            }

            buttonBoardSize.Text = "Board Size " + (int)m_GameSetting + "X" + (int)m_GameSetting + " (click to change)";
        }

        private void gameStart(int i_BoardSize, GameEngien.eGameMode i_GameSetting)
        {
            GameForm game = new GameForm(i_BoardSize, i_GameSetting);
            this.Hide();
            game.ShowDialog();
            this.Close();
        }
    }
}
