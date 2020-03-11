using System.Collections.Generic;

namespace ProjectT
{
    public class Viewer
    {
        public string Name;
        public double Coins;

        public Viewer()
        {

        }

        public void setName(string name)
        {
            Name = name;
        }
        public void setCoins(double coins)
        {
            Coins = coins;
        }
    }

    class ViewerJsonHelper
    {
        public List<Viewer> List = new List<Viewer>();
    }
}