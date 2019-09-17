using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exploding_Kittens
{
    [Serializable]
    public class CardPack
    {
        private List<int> SeverPack;
        public CardPack()
        {
            SeverPack = new List<int>();
            createRequirePack();
            // trộn bài
            SUFFLE();
        }

        public int getSize()
        {
            return SeverPack.Count;
        }

        private void createRequirePack()
        {
            // thêm những lá bắt buộc trong cardpack 1 - > 82
            for (int i = 1; i <= 82; i++)
                SeverPack.Add(i);
            for (int i = 92; i <= 106; i++)
                SeverPack.Add(i);
        }

        public int DRAW_A_CARD()
        {
            int firstCard = SeverPack[0];
            SeverPack.RemoveAt(0);
            return firstCard;
        }

        public int DRAW_FROM_THE_BOTTOM()
        {
            int lastCard = SeverPack[SeverPack.Count];
            SeverPack.RemoveAt(SeverPack.Count);
            return lastCard;
        }

        public List<int> SEE_THE_FUTURE(int count)
        {
            List<int> returnList = new List<int>();
            for (int i = 0; i < count; i++)
                returnList.Add(SeverPack[i]);
            return returnList;
        }

        public void SUFFLE()
        {
            Random rnd = new Random();
            for (int i = 0; i < SeverPack.Count; i++)
            {
                int index = rnd.Next(0, SeverPack.Count);
                int temp = SeverPack[i];
                SeverPack[i] = SeverPack[index];
                SeverPack[index] = temp;
                Thread.Sleep(15);
            }
        }

       
    }
}
