using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploding_Kittens
{
    [Serializable]
    public class GameBucket
    {
        public CardPack PACK;
        private List<Player> PLAYERS;

        private bool isFirstTime = true;
        public int idPlayerTurn = 0; //idPlayerTurn = Playerindex -1;

        public GameBucket()
        {
            PACK = new CardPack();
            PLAYERS = new List<Player>();   
        }

        public int getPlayerSize()
        {
            return PLAYERS.Count;
        }

        public void setPlayer(Player p)
        {
            PLAYERS.Add(p);
        }


        // tăng lượt cho playerTurn tiếp theo
        public void nextTurn()
        {

            int turn = PLAYERS[idPlayerTurn].getTurn();

           if(isFirstTime)
            {
                isFirstTime = false;  
            }
           else
            {
                // trừ turn đi
                PLAYERS[idPlayerTurn].setTurn(turn - 1);
            }


            turn = PLAYERS[idPlayerTurn].getTurn();
            if(turn == 0)
            {
                if (idPlayerTurn < PLAYERS.Count - 1)
                    idPlayerTurn++;
                else
                    idPlayerTurn = 0;

                // set Turn cho lượt tiếp theo;
                turn = PLAYERS[idPlayerTurn].getTurn();
                PLAYERS[idPlayerTurn].setTurn(turn + 1);


            }
            

        }


        public Player getPlayerWithID(int id)
        {
            return PLAYERS[id - 1];
        }

        public void setPlayerWithID(Player p)
        {
             PLAYERS[p.getID() - 1] = p;
        }

    }
}
