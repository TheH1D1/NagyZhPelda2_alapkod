using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NagyZhPelda2
{
    internal interface IStock
    {
        void Load<TProduct>(string filename) where TProduct : BabyProduct;
        void Save<TProduct>(string filename) where TProduct : BabyProduct;
        List<BabyProduct> List();
        List<TProduct> List<TProduct>() where TProduct : BabyProduct;
        BabyProduct? Get(string id);
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

        public List<BabyProduct> List()
        {
            return BabyProducts;
        }

        public List<TProduct> List<TProduct>() where TProduct : BabyProduct
        {
            return BabyProducts.OfType<TProduct>().ToList();
        }

        public BabyProduct? Get(string id)
        {
            return BabyProducts.FirstOrDefault(p => p.Id == id);
        }

        public void Add(BabyProduct product)
        {
            BabyProducts.Add(product);
        }

        public void Remove(string id)
        {
            var product = BabyProducts.FirstOrDefault(p => p.Id == id);
            
            if (product != null)
            {
                BabyProducts.Remove(product);
            }
        }

        public int Payment(Order order)
        {
            int totalPrice = 0;

            foreach (var product in order.OrderItems)
            {
                var babyProduct = BabyProducts.FirstOrDefault(p => p.Id == product.ProductId);

                if (babyProduct != null)
                {
                    totalPrice += babyProduct.Price * product.Quantity; 
                }
            }
            totalPrice = (int) (totalPrice * 1.27) + 320; // Áfa + bruttó 320Ft kezelési költség
            return totalPrice;
        }

        public bool Sale(Order order)
        {
            // Ellenőrzés
            foreach (var product in order.OrderItems)
            {
                var babyProduct = BabyProducts.FirstOrDefault(p => p.Id == product.ProductId);

                if (babyProduct == null || babyProduct.Quantity < product.Quantity)
                {
                    return false;
                }
            }

            // Levonás
            foreach (var product in order.OrderItems)
            {
                var babyProduct = BabyProducts.FirstOrDefault(s => s.Id == product.ProductId);

                if (babyProduct != null)
                {
                    babyProduct.Quantity -= product.Quantity;
                }
            }
            return true;
        }

        public void Upload(string id, int quantity)
        {
            foreach (var product in BabyProducts)
            {
                if (product.Id == id)
                {
                    product.Quantity += quantity;
                }
            }
        }

        // Az IStock interfészt megvalósító osztálynak legyen egy generikus Save metódusa, 
        // amely a megadott típusú termékeket menti ki a paraméterben megadott fájlba. TIPP: WHERE T : ŐSOSZTÁLY

        public void Save<TProduct>(string filename) where TProduct : BabyProduct
        {
            var list = List<TProduct>();

            var options = new JsonSerializerOptions{WriteIndented = true};

            string json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(filename, json);
        }
    }
}
