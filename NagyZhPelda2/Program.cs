#region parts
#define PART1 //Termékek adatszerkezete (6 pont)
#define PART2 //IStock interfész megvalósítása, Load metódus (4 pont)
#define PART3 //List és Get metódusok (4 pont)
#define PART4 //Add és Remove metódusok (4 pont)
#define PART5 //Rendelés adatszerkezete (Order osztály) és betöltése (LoadOrder) metódus (6 pont)
#define PART6 //Payment metódus (4 pont)
#define PART7 //Sale metódus (4 pont)
#define PART8 //Upload metódus (4 pont)
#define PART9 //Save metódus (4 pont)
#endregion

using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Transactions;

namespace NagyZhPelda2
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if PART1
            Console.WriteLine();
            Console.WriteLine($"----START OF PART1----");

            var baseClassType = typeof(BabyProduct);
            var childClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && p.IsSubclassOf(baseClassType))
                .ToList();

            Console.WriteLine($"Van-e szarmaztatva gyerekosztaly az absztrakt ososztalybol: {childClassTypes.Any()}");

            if (!childClassTypes.Any())
            {
                Console.WriteLine($"----END OF PART1 -> END OF PROGRAM----");
                return;
            }

            var toyType = GetClassType(childClassTypes, typeof(int), 3);
            var chairType = GetClassType(childClassTypes, typeof(bool), 1);
            var clothesType = GetClassType(childClassTypes, typeof(string), 3);

            Console.WriteLine($"A {toyType} osztalyban felul van-e definialva a ToString metodus: {toyType.GetMethod("ToString").DeclaringType == toyType}");
            Console.WriteLine($"A {chairType} osztalyban felul van-e definialva a ToString metodus: {chairType.GetMethod("ToString").DeclaringType == chairType}");
            Console.WriteLine($"A {clothesType} osztalyban felul van-e definialva a ToString metodus: {clothesType.GetMethod("ToString").DeclaringType == clothesType}");

            Console.WriteLine($"----END OF PART1----");
#endif

#if PART2
            Console.WriteLine();
            Console.WriteLine($"----START OF PART2----");

            var interfaceType = typeof(IStock);
            var stockClassType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(p => !p.IsInterface && interfaceType.IsAssignableFrom(p));

            Console.WriteLine($"Az IStock interfesz implementalva van-e: {stockClassType != default}");

            if (stockClassType == default)
            {
                Console.WriteLine($"----END OF PART2 -> END OF PROGRAM----");
                return;
            }

            var stock = (IStock)Activator.CreateInstance(stockClassType);

            Console.WriteLine($"Az IStock interfeszt implementalo {stockClassType} peldanyositasa sikeres-e: {stock != default}");

            MethodInfo loadMethod = typeof(IStock).GetMethod("Load");

            Console.WriteLine($"A Load metodus letezik-e a {stockClassType} osztalyban: {loadMethod != null}");
            if (loadMethod == null)
            {
                Console.WriteLine($"----END OF PART2 -> END OF PROGRAM----");
                return;
            }

            Console.WriteLine("Az adatok betoltese a json fajlokbol...");
            loadMethod.MakeGenericMethod(new Type[] { toyType }).Invoke(stock, new object[] { "toys.json" });
            loadMethod.MakeGenericMethod(new Type[] { chairType }).Invoke(stock, new object[] { "chairs.json" });
            loadMethod.MakeGenericMethod(new Type[] { clothesType }).Invoke(stock, new object[] { "clothes.json" });
            Console.WriteLine("Az adatok betoltese lefutott");
            Console.WriteLine($"----END OF PART2----");
#endif

#if PART3
            Console.WriteLine();
            Console.WriteLine($"----START OF PART3----");

            Console.WriteLine("Nem generikus List metodus ellenorzese:");
            PrintStock(stock.List());

            Console.WriteLine();
            Console.WriteLine("Generikus List metodus ellenorzese:");
            MethodInfo genericListMethod = typeof(IStock).GetMethods().FirstOrDefault(m => m.Name == "List" && m.GetGenericArguments().Length == 1);
            if (genericListMethod != null)
            {
                Console.WriteLine("Jatekok listazasa:");
                var toys = (IList?)genericListMethod.MakeGenericMethod([toyType]).Invoke(stock, []);
                if (toys != null)
                {
                    PrintStock(toys);
                }
                Console.WriteLine();
                Console.WriteLine("Etetoszekek listazasa:");
                var chairs = (IList?)genericListMethod.MakeGenericMethod([chairType]).Invoke(stock, []);
                if (chairs != null)
                {
                    PrintStock(chairs);
                }
                Console.WriteLine();
                Console.WriteLine("Ruhak listazasa:");
                var clothes = (IList?)genericListMethod.MakeGenericMethod([clothesType]).Invoke(stock, []);
                if (clothes != null)
                {
                    PrintStock(clothes);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Get metodus ellenorzese:");
            Console.WriteLine($"Id 1234: {stock.Get("1234")}");
            Console.WriteLine($"Id 0235: {stock.Get("0235")}");
            Console.WriteLine($"Id 9812: {stock.Get("9812")}");
            Console.WriteLine($"Id 0011: {stock.Get("0011")}");
            Console.WriteLine($"Id 8436: {stock.Get("8436")}");

            Console.WriteLine($"----END OF PART3----");
#endif

#if PART4
            Console.WriteLine();
            Console.WriteLine($"----START OF PART4----");

            Console.WriteLine("Uj jatek hozzaadasa");
            var newToy = (BabyProduct)Activator.CreateInstance(toyType, new object[] { "4387", "Ugralo panda", 4900, 57, 4 }); //azonosito, nev, ar, mennyiseg, eletkor
            if (newToy != null)
            {
                stock.Add(newToy);
            }

            Console.WriteLine("Uj etetoszek hozzaadasa");
            var newChair = (BabyProduct)Activator.CreateInstance(chairType, new object[] { "6297", "Alfoldi etetoszek", 34000, 14, true }); //azonosito, nev, ar, mennyiseg, allithato-e
            if (newChair != null)
            {
                stock.Add(newChair);
            }

            Console.WriteLine("Uj ruhazati termék hozzaadasa");
            var newClothes = (BabyProduct)Activator.CreateInstance(clothesType, new object[] { "8027", "Nike rugdalozo", 23990, 25, "52/56" }); //azonosito, nev, ar, mennyiseg, meret
            if (newClothes != null)
            {
                stock.Add(newClothes);
            }

            Console.WriteLine("Termekek a hozzadasok utan:");
            PrintStock(stock.List());

            Console.WriteLine();
            Console.WriteLine("0235 azonositoju termek torlese");
            stock.Remove("0235");
            Console.WriteLine("9812 azonositoju termek torlese");
            stock.Remove("9812");
            Console.WriteLine("8436 azonositoju termek torlese");
            stock.Remove("8436");

            Console.WriteLine("Termekek a torlesek utan:");
            PrintStock(stock.List());

            Console.WriteLine($"----END OF PART4----");
#endif

#if PART5
            Console.WriteLine();
            Console.WriteLine($"----START OF PART5----");

            var order1 = LoadOrder("order1.json");
            var order2 = LoadOrder("order2.json");
            var order3 = LoadOrder("order3.json");

            Console.WriteLine($"order1.json fájlbol az order1 objektum letrejott-e: {order1 != null}");
            Console.WriteLine($"order2.json fájlbol az order2 objektum letrejott-e: {order2 != null}");
            Console.WriteLine($"order3.json fájlbol az order3 objektum letrejott-e: {order3 != null}");

            Console.WriteLine($"----END OF PART5----");
#endif

#if PART6
            Console.WriteLine();
            Console.WriteLine($"----START OF PART6----");

            var order1Sum = order1 != null ? stock.Payment(order1) : -1;
            var order2Sum = order2 != null ? stock.Payment(order2) : -1;
            var order3Sum = order3 != null ? stock.Payment(order3) : -1;

            Console.WriteLine($"Az order1 vegosszege: {order1Sum} Ft");
            Console.WriteLine($"Az order2 vegosszege: {order2Sum} Ft");
            Console.WriteLine($"Az order3 vegosszege: {order3Sum} Ft");

            Console.WriteLine($"----END OF PART6----");
#endif

#if PART7
            Console.WriteLine();
            Console.WriteLine($"----START OF PART7----");

            var order1Sale = order1 != null ? stock.Sale(order1) : false;
            var order2Sale = order2 != null ? stock.Sale(order2) : false;
            var order3Sale = order3 != null ? stock.Sale(order3) : false;

            Console.WriteLine($"Az order1 eladasa sikeres-e: {order1Sale}");
            Console.WriteLine($"Az order2 eladasa sikeres-e: {order2Sale}");
            Console.WriteLine($"Az order3 eladasa sikeres-e: {order3Sale}");
            Console.WriteLine();
            Console.WriteLine("Az eladasok utan a keszlet:");
            PrintStock(stock.List());

            Console.WriteLine($"----END OF PART7----");
#endif

#if PART8
            Console.WriteLine();
            Console.WriteLine($"----START OF PART8----");

            //1234,0011,8027

            Console.WriteLine("Az 1234 azonositoju termek feltoltese 10 darabbal");
            stock.Upload("1234", 10);
            Console.WriteLine("A 0011 azonositoju termek feltoltese 15 darabbal");
            stock.Upload("0011", 15);
            Console.WriteLine("A 8027 azonositoju termek feltoltese 5 darabbal");
            stock.Upload("8027", 5);
            Console.WriteLine();
            Console.WriteLine("A keszlet a feltoltesek utan");
            PrintStock(stock.List());

            Console.WriteLine($"----END OF PART8----");
#endif

#if PART9
            Console.WriteLine();
            Console.WriteLine($"----START OF PART9----");

            MethodInfo saveMethod = typeof(IStock).GetMethod("Save");

            Console.WriteLine($"A Save metodus letezik-e a {stockClassType} osztalyban: {saveMethod != null}");
            if (saveMethod == null)
            {
                Console.WriteLine($"----END OF PART9 -> END OF PROGRAM----");
                return;
            }

            Console.WriteLine("Az adatok kimentese a json fajlokba...");
            saveMethod.MakeGenericMethod(new Type[] { toyType }).Invoke(stock, new object[] { "toys_out.json" });
            saveMethod.MakeGenericMethod(new Type[] { chairType }).Invoke(stock, new object[] { "chairs_out.json" });
            saveMethod.MakeGenericMethod(new Type[] { clothesType }).Invoke(stock, new object[] { "clothes_out.json" });
            Console.WriteLine("Az adatok kimentese lefutott");

            Console.WriteLine($"----END OF PART9----");
#endif
        }

        static Type GetClassType(List<Type> classTypes, Type propertyType, int propertyCount)
        {
            foreach (var type in classTypes)
            {
                var propertyTypes = type.GetProperties().Select(p => p.PropertyType).ToList();
                if (propertyTypes.Count(t => t == propertyType) == propertyCount)
                {
                    return type;
                }
            }
            return null;
        }

        static void PrintStock(IList stocks)
        {
            foreach (var item in stocks)
            {
                Console.WriteLine($"{item}");
            }
        }

        static Order LoadOrder(string filename)
        {
            var json = File.ReadAllText(filename);
            var order = JsonSerializer.Deserialize<Order>(json);

            return order ?? new Order();
        }
    }
}
