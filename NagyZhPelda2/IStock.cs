using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NagyZhPelda2
{
    internal interface IStock
    {
        void Load<TProduct>(string filename) where TProduct : BabyProduct;
        void Save<TProduct>(string filename) where TProduct : BabyProduct;
        List<BabyProduct> List();
        List<TProduct> List<TProduct>() where TProduct : BabyProduct;
        BabyProduct Get(string id);
        void Add(BabyProduct product);
        void Remove(string id);
        int Payment(Order order);
        bool Sale(Order order);
        void Upload(string id, int quantity);
    }

    internal class Store : IStock
    {
        private List<BabyProduct> BabyProducts;

        public Store()
        {
            BabyProducts = new List<BabyProduct>(); // Ez példányosítja a tárolót!
        }

        public void Load<TProduct>(string filename) where TProduct : BabyProduct
        {
            var json = File.ReadAllText(filename);
            List<TProduct>? items = JsonSerializer.Deserialize<List<TProduct>>(json);

            if (items != null)
            {
                BabyProducts.AddRange(items);
            }
        }

        // Az osztálynak van egy List metódusa, amely a teljes terméklistát adja vissza, a List metódusból legyen
        // még egy, ami egy generikus metódus, és csak a megadott típusnak megfelelő termékek listáját adja vissza. 

        public List<BabyProduct> List()
        {
            return BabyProducts;
        }

        public List<TProduct> List<TProduct>() where TProduct : BabyProduct
        {
            return BabyProducts.OfType<TProduct>().ToList();
        }

        // Ezenkívül van egy Get metódusa, amely pedig azonosító szerint ad vissza egy terméket. Az Add
        // metódus egy új terméket ad hozzá a készlethez, a Remove metódus pedig azonosító alapján eltávolítja a
        // terméket a készletből. TIPP: IF(PRODUCT IS TPRODUCT) VAGY LIST.OFTYPE<TPRODUCT>().TOLIST()

        

        public void Save<TProduct>(string filename) where TProduct : BabyProduct
        {
            throw new NotImplementedException();
        }

        public BabyProduct Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Add(BabyProduct product)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public int Payment(Order order)
        {
            throw new NotImplementedException();
        }

        public bool Sale(Order order)
        {
            throw new NotImplementedException();
        }

        public void Upload(string id, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
