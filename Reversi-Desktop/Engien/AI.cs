using System.Collections.Generic;

namespace Engien
{
    public class AI
    {
        private int maxMin(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer, int i_Depht, int i_MaxDeapth)
        {
            if (i_Depht == i_MaxDeapth)
            {
                return evaluation(i_Board, i_CurrenPlayer);
            }
            else
            {
                List<Point> moveOptions = null;
                List<int> sonsEvaluations = new List<int>();
                GameEngien.ePlayers OtherPlayer;

                if (i_CurrenPlayer == GameEngien.ePlayers.FirstPlayer)
                {
                    OtherPlayer = GameEngien.ePlayers.SecondPlayer;
                }
                else
                {
                    OtherPlayer = GameEngien.ePlayers.FirstPlayer;
                }

                if (i_Board.IsThereOptionsToPlay(i_CurrenPlayer, ref moveOptions))
                {
                    foreach (Point location in moveOptions)
                    {
                        sonsEvaluations.Add(maxMin(i_Board.BoardDuplicatewithNewPoint(i_Board, location, i_CurrenPlayer), OtherPlayer, i_Depht + 1, i_MaxDeapth));
                    }

                    if (i_Depht % 2 == 1)
                    {
                        return returnMaxInt(sonsEvaluations);
                    }
                    else
                    {
                        return returnMinInt(sonsEvaluations);
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public Point AiTurn(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer)
        {
            GameEngien.ePlayers OtherPlayer;
            Point returnTurn = null;
            List<Point> moveOptions = null;
            List<int> sonsEvaluations = new List<int>();

            if (i_CurrenPlayer == GameEngien.ePlayers.FirstPlayer)
            {
                OtherPlayer = GameEngien.ePlayers.SecondPlayer;
            }
            else
            {
                OtherPlayer = GameEngien.ePlayers.FirstPlayer;
            }

            if (i_Board.IsThereOptionsToPlay(i_CurrenPlayer, ref moveOptions))
            {
                foreach (Point location in moveOptions)
                {
                    sonsEvaluations.Add(maxMin(i_Board.BoardDuplicatewithNewPoint(i_Board, location, i_CurrenPlayer), OtherPlayer, 1, 6));
                }
            }

            int maxvalue = returnMaxInt(sonsEvaluations);
            for (int i = 0; i < sonsEvaluations.Count; i++)
            {
                if (sonsEvaluations[i] == maxvalue)
                {
                    returnTurn = moveOptions[i];
                }
            }

            return returnTurn;
        }

        private int evaluation(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer)
        {
            int evalue = 0;

            evalue += cornerCheck(i_Board, i_CurrenPlayer, 0, 0);
            evalue += cornerCheck(i_Board, i_CurrenPlayer, 0, i_Board.Size - 1);
            evalue += cornerCheck(i_Board, i_CurrenPlayer, i_Board.Size - 1, 0);
            evalue += cornerCheck(i_Board, i_CurrenPlayer, i_Board.Size - 1, i_Board.Size - 1);
            evalue += lineCheck(i_Board, i_CurrenPlayer);
            evalue += subCheck(i_Board, i_CurrenPlayer);
            evalue += fullBoardCheck(i_Board, i_CurrenPlayer);

            return evalue;
        }

        private int returnMaxInt(List<int> i_IntList)
        {
            int max = i_IntList[0];
            foreach (int num in i_IntList)
            {
                if (max < num)
                {
                    max = num;
                }
            }

            return max;
        }

        private int returnMinInt(List<int> i_IntList)
        {
            int min = i_IntList[0];
            foreach (int num in i_IntList)
            {
                if (min > num)
                {
                    min = num;
                }
            }

            return min;
        }

        private int cornerCheck(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer, int i_Y, int i_X)
        {
            int returnevalue = 0;
            if (isDiscExists(i_Board, i_Y, i_X))
            {
                if (i_Board.BoardMatrix[i_Y, i_X].Color == (Disc.eColors)i_CurrenPlayer)
                {
                    returnevalue += 50;
                }
                else
                {
                    returnevalue -= 50;
                }
            }

            return returnevalue;
        }

        private bool isDiscExists(GameBoard i_Board, int i_Y, int i_X)
        {
            return i_Board.BoardMatrix[i_Y, i_X] != null;
        }

        private int lineCheck(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer)
        {
            int returnevalue = 0;

            for (int i = 2; i < i_Board.Size - 2; i++)
            {
                if (i_Board.BoardMatrix[0, i] != null)
                {
                    if (i_Board.BoardMatrix[0, i].Color == (Disc.eColors)i_CurrenPlayer)
                    {
                        returnevalue += 10;
                    }
                    else
                    {
                        returnevalue -= 10;
                    }
                }
            }

            for (int i = 2; i < i_Board.Size - 2; i++)
            {
                if (i_Board.BoardMatrix[i_Board.Size - 1, i] != null)
                {
                    if (i_Board.BoardMatrix[i_Board.Size - 1, i].Color == (Disc.eColors)i_CurrenPlayer)
                    {
                        returnevalue += 10;
                    }
                    else
                    {
                        returnevalue -= 10;
                    }
                }
            }

            for (int i = 2; i < i_Board.Size - 2; i++)
            {
                if (i_Board.BoardMatrix[i, 0] != null)
                {
                    if (i_Board.BoardMatrix[i, 0].Color == (Disc.eColors)i_CurrenPlayer)
                    {
                        returnevalue += 10;
                    }
                    else
                    {
                        returnevalue -= 10;
                    }
                }
            }

            for (int i = 2; i < i_Board.Size - 2; i++)
            {
                if (i_Board.BoardMatrix[i, i_Board.Size - 1] != null)
                {
                    if (i_Board.BoardMatrix[i, i_Board.Size - 1].Color == (Disc.eColors)i_CurrenPlayer)
                    {
                        returnevalue += 10;
                    }
                    else
                    {
                        returnevalue -= 10;
                    }
                }
            }

            return returnevalue;
        }

        private int subCheck(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer)
        {
            int NumOfBlackDiscs = 0, NumOfwhiteDiscs = 0, SubOfDiscs = 0, evalue = 0;
            i_Board.CalcPlayersScore(out NumOfBlackDiscs, out NumOfwhiteDiscs);
            SubOfDiscs = NumOfBlackDiscs - NumOfwhiteDiscs;

            if (i_CurrenPlayer == GameEngien.ePlayers.FirstPlayer)
            {
                evalue += SubOfDiscs;
            }
            else
            {
                evalue -= SubOfDiscs;
            }

            return evalue;
        }

        private int fullBoardCheck(GameBoard i_Board, GameEngien.ePlayers i_CurrenPlayer)
        {
            int evalue = 0, o_NumOfBlackDiscs = 0, o_NumOfwhiteDiscs = 0;
            const int winForSure = 1000000000;

            i_Board.CalcPlayersScore(out o_NumOfBlackDiscs, out o_NumOfwhiteDiscs);

            if (o_NumOfBlackDiscs + o_NumOfwhiteDiscs == i_Board.Size * i_Board.Size)
            {
                if (o_NumOfBlackDiscs > o_NumOfwhiteDiscs && i_CurrenPlayer == GameEngien.ePlayers.FirstPlayer)
                {
                    evalue += winForSure;
                }
                else if (o_NumOfBlackDiscs > o_NumOfwhiteDiscs && i_CurrenPlayer == GameEngien.ePlayers.SecondPlayer)
                {
                    evalue -= winForSure;
                }
                else if (o_NumOfBlackDiscs < o_NumOfwhiteDiscs && i_CurrenPlayer == GameEngien.ePlayers.SecondPlayer)
                {
                    evalue += winForSure;
                }
                else
                {
                    evalue -= winForSure;
                }
            }

            return evalue;
        }
    }
}
