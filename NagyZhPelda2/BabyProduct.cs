using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NagyZhPelda2
{
    internal abstract class BabyProduct
    {
       public string Id {get; set;} = string.Empty;
       public string Name {get; set;} = string.Empty;
       public int Price {get; set;}
       public int Quantity {get; set;}

        public BabyProduct()
        {
            
        }

        public BabyProduct(string id, string name, int price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{GetType().Name} - Id: {Id}, Name: {Name}, Price: {Price} Ft, Quantity: {Quantity}";
        }
    }
    
    internal class Toy : BabyProduct
    {
        public int Age {get; set;}

        public Toy()
        {
            
        }

        public Toy(string id, string name, int price, int quantity,int age) : base(id, name, price, quantity)
        {
            Age = age;
        }

        public override string ToString()
        {
            return base.ToString() + $", Age: {Age} ev";
        }
    }

    internal class Cloth : BabyProduct
    {
        public string Size {get; set;} = string.Empty;

        public Cloth()
        {
            
        }

        public Cloth(string id, string name, int price, int quantity,string size) : base(id, name, price, quantity)
        {
            Size = size;
        }


        public override string ToString()
        {
            return base.ToString() + $", Size: {Size}";
        }
    }

    internal class Chair : BabyProduct
    {
        public bool CanBeSet {get; set;}

        public Chair()
        {
            
        }

        public Chair(string id, string name, int price, int quantity,bool canbeset) : base(id, name, price, quantity)
        {
            CanBeSet = canbeset;
        }

        public override string ToString()
        {
            return base.ToString() + $", CanBeSet: {CanBeSet}";
        }
    }
}
