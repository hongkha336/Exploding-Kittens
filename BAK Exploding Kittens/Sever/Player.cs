using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sever
{
    [Serializable]
    public class Player
    {
        private string playerName;
        private int playerID;
        private List<int> myCards;
        private int turn;

       

        public Player(int id, string playerName)
        {
            this.playerID = id;
            myCards = new List<int>();
            turn = 0;
            this.playerName = playerName;
        }
        
        public int getID()
        {
            return playerID;
        }

        public List<int> getListCards()
        {
            return myCards;
        }

        public void setTurn(int turn )
        {
            this.turn = turn;
        }

        public int getTurn()
        {
            return turn;
        }

        public void addCard(int idCard)
        {
            myCards.Add(idCard);
            myCards.Sort();
        }

        public void removeCard(int idCard)
        {
            myCards.Remove(idCard);
        }

        public void removeCards(List<int> cards)
        {
            foreach(int i in cards)
                myCards.Remove(i);
        }

        public int STEAL_A_CARD()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, myCards.Count);
            int temp = myCards[index];
            removeCard(temp);
            return temp;
        }

        public int CALL_A_CARD(int idCard)
        {
            try
            {
                removeCard(idCard);
                return idCard;
            }
            catch
            {
                return 0;
            }
        }

       
    }
}
