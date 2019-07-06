using System;
using System.Collections.Generic;

namespace Engien
{
    public class GameEngien
    {
        private readonly AI m_PcAi = new AI();
        private ePlayers m_CurrentPlayer = ePlayers.FirstPlayer;
        private eGameMode m_Mode;
        private GameBoard.eBoardDemantions m_BoardSize;
        private GameBoard m_gameBoard;
        private List<Point> m_moveOptions = null;
        private int m_Player1Score = 0, m_Player2Score = 0;

        public ePlayers CurrentPlayer { get => m_CurrentPlayer; set => m_CurrentPlayer = value; }

        public int Player1Score { get => m_Player1Score; set => m_Player1Score = value; }

        public int Player2Score { get => m_Player2Score; set => m_Player2Score = value; }

        public delegate void MyAction<T, G>(T t1, G t2);

        public event Action<Point> Flip;

        public event MyAction<Point, ePlayers> Set;

        public event Action<string> GameOver;

        public GameEngien(eGameMode i_Mode, GameBoard.eBoardDemantions i_BoardSize)
        {
            m_Mode = i_Mode;
            m_BoardSize = i_BoardSize;
            m_gameBoard = new GameBoard(i_BoardSize, false);
            m_gameBoard.Flip += OnFlip;
            m_gameBoard.Set += OnSet;
        }

        public bool ShowOptions()
        {
            bool isNextPlayerIsAI;
            isNextPlayerIsAI = m_CurrentPlayer == ePlayers.SecondPlayer && m_Mode == eGameMode.PvC;
            eraseLastOptions();
            if (!m_gameBoard.IsThereOptionsToPlay(CurrentPlayer, ref m_moveOptions))
            {
                changePlayer();
                isNextPlayerIsAI = m_CurrentPlayer == ePlayers.SecondPlayer && m_Mode == eGameMode.PvC;
                if (!m_gameBoard.IsThereOptionsToPlay(CurrentPlayer, ref m_moveOptions))
                {
                    isNextPlayerIsAI = false;
                    OnGameOver("End");
                }
            }

            return isNextPlayerIsAI;
        }

        private void eraseLastOptions()
        {
            if (m_moveOptions != null)
            {
                foreach (Point p in m_moveOptions)
                {
                    OnSet(new Point(p.x, p.y), ePlayers.Empty);
                }
            }
        }

        public void NextMove(Point i_UserCordInput)
        {
            if (m_Mode == eGameMode.PvC && m_CurrentPlayer == ePlayers.SecondPlayer)
            {
                i_UserCordInput = m_PcAi.AiTurn(m_gameBoard, m_CurrentPlayer);
            }

            if (m_gameBoard.TryAddDiscToLocation(i_UserCordInput.x, i_UserCordInput.y, (Disc.eColors)m_CurrentPlayer))
            {
                changePlayer();

                m_gameBoard.CalcPlayersScore(out m_Player1Score, out m_Player2Score);
            }
        }

        private void changePlayer()
        {
            if (CurrentPlayer == ePlayers.FirstPlayer)
            {
                CurrentPlayer = ePlayers.SecondPlayer;
            }
            else
            {
                CurrentPlayer = ePlayers.FirstPlayer;
            }
        }

        public enum eGameMode
        {
            PvP,
            PvC
        }

        public enum ePlayers
        {
            FirstPlayer = 'x',
            SecondPlayer = 'o',
            Empty = ' ',
            PossibleMove = '.',
            GameOver = -1
        }

        protected virtual void OnFlip(Point i_PointToFlip)
        {
            Flip.Invoke(i_PointToFlip);
        }

        protected virtual void OnSet(Point i_PointToFlip, ePlayers i_PlayerType)
        {
            Set.Invoke(i_PointToFlip, i_PlayerType);
        }

        protected virtual void OnGameOver(string i_Massege)
        {
            GameOver.Invoke(i_Massege);
        }
    }
}
