using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;




namespace MÓNÍTÓR_VÁSÁR
{
    class Monitor
    {
        public string Gyarto { get; set; }
        public string Tipus { get; set; }
        public double Meret { get; set; }
        public int Ar { get; set; }

        public Monitor(string sor)
        {
            string[] atmeneti = sor.Split(';');


            Gyarto = atmeneti[0];
            Tipus = atmeneti[1];
            Meret = Convert.ToDouble(atmeneti[2]);
            Ar = int.Parse(atmeneti[3]);


        }
        public void kiir()
        {
            Console.WriteLine($"{Gyarto,-8}/ {Tipus,13}/ {Meret,7}/ {Ar,6}");
        }


    }
    class Program
    {

        static void Main(string[] args)
        {
            List<Monitor> monitorok = new List<Monitor>();




            foreach (var item in File.ReadAllLines(@"../../../adatok.txt"))
            {
                monitorok.Add(new Monitor(item));
            }




            for (int i = 0; i < monitorok.Count; i++)
            {
                monitorok[i].kiir();
            }








            //Feladatok:




            //Lehetőleg minden kiírást a főprogram végezzen el. Próbálj minél több kódot újrahasznosítani. Minden feladatot meg kell oldani hagyományosan, és azután, ha tudod, LINQ-val is.




            //1. Hozz létre egy osztályt a monitorok adatai számára. Olvasd be a fájl tartalmát.




            //2. Írd ki a monitorok összes adatát virtuális metódussal, soronként egy monitort a képernyőre. A kiírás így nézzen ki
            //Gyártó: Samsung; Típus: S24D330H; Méret: 24 col; Nettó ár: 33000 Ft


            //2. Tárold az osztálypéldányokban a bruttó árat is (ÁFA: 27%, konkrétan a 27-tel számolj, ne 0,27-tel vagy más megoldással.)




            //3. Tételezzük fel, hogy mindegyik monitorból 15 db van készleten, ez a nyitókészlet. Mekkora a nyitó raktárkészlet bruttó (tehát áfával növelt) értéke?
            double nemtudom = 0;
            double teljes = 0;
            for (int i = 0; i < monitorok.Count; i++)
            {
                nemtudom = monitorok[i].Ar * 1.27 * 15;
                teljes = nemtudom += teljes;
            }
            Console.WriteLine($"\nA nyitókészlet teljes értéke vityadóval: {teljes} Ft");






            //Írj egy metódust, ami meghívásakor kiszámolja a raktárkészlet aktuális bruttó értékét. A főprogram írja ki az értéket.




            //4. Írd ki egy új fájlba, és a képernyőre az 50.000 Ft feletti nettó értékű monitorok összes adatát (a darabszámmal együtt) úgy
            //hogy a szöveges adatok nagybetűsek legyenek, illetve az árak ezer forintba legyenek átszámítva.
            static void OtvenFelett(List<Monitor> monitorok)
            {
                using (StreamWriter writer = new StreamWriter("above_50k_monitors.txt"))
                {
                    foreach (var monitor in monitorok)
                    {
                        if (monitor.Ar > 50000)
                        {
                            string formattedData = AdatFormazas(monitor);
                            writer.WriteLine(formattedData);
                            Console.WriteLine(formattedData);
                        }
                    }
                }
            }
            static string AdatFormazas(Monitor monitor)
            {
                double arEzerForint = monitor.Ar / 1000.0;
                return $"\nGyártó: {monitor.Gyarto.ToUpper()}; Típus: {monitor.Tipus.ToUpper()}; Méret: {monitor.Meret} col; Nettó ár: {arEzerForint} eFt";
            }


            OtvenFelett(monitorok);

           
            //5. Egy vevő keresi a HP EliteDisplay E242 monitort. Írd ki neki a képernyőre, hogy hány darab ilyen van a készleten.

            static void FindAndSuggestMonitor(List<Monitor> monitorok, string searchedMonitor)
            {
                int count = 0;
                foreach (var monitor in monitorok)
                {
                    if (string.Equals(monitor.Tipus, searchedMonitor, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                    }
                }




                if (count > 0)
                {
                    Console.WriteLine($"\nKészleten van {count} darab {searchedMonitor} monitor.");
                }
                else
                {
                    Console.WriteLine($"{searchedMonitor} monitor nincs készleten.");
                    double averagePrice = monitorok.Average(m => m.Ar);
                    var alternatives = monitorok.Where(m => m.Ar > averagePrice).OrderBy(m => m.Ar);
                    if (alternatives.Any())
                    {
                        Console.WriteLine($"\nAjánlott alternatívák:");
                        foreach (var alt in alternatives)
                        {
                            Console.WriteLine($"Gyártó: {alt.Gyarto}; Típus: {alt.Tipus}; Ár: {alt.Ar} Ft");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nSajnáljuk, nincs alternatív ajánlat.");
                    }
                }
            }

            FindAndSuggestMonitor(monitorok, "EliteDisplay E242");
            //Ha nincs a készleten, ajánlj neki egy olyan monitort, aminek az ára az átlaghoz fölülről közelít. Ehhez használd az átlagszámító függvényt (később lesz feladat).




            //6. Egy újabb vevőt csak az ár érdekli. Írd ki neki a legolcsóbb monitor méretét, és árát.

            static void FindCheapestMonitor(List<Monitor> monitorok)
            {
                var cheapestMonitor = monitorok.OrderBy(m => m.Ar).FirstOrDefault();
                if (cheapestMonitor != null)
                {
                    Console.WriteLine($"\nA legolcsóbb monitor mérete: {cheapestMonitor.Meret} col; Ára: {cheapestMonitor.Ar} Ft");
                }
                else
                {
                    Console.WriteLine("\nNincs monitor a készleten.");
                }
            }

            FindCheapestMonitor(monitorok);





            //7. A cég akciót hirdet. A 70.000 Ft fölötti árú Samsung monitorok bruttó árából 5%-ot elenged
            //Írd ki, hogy mennyit veszítene a cég az akcióval, ha az összes akciós monitort akciósan eladná.




            //8. Írd ki a képernyőre minden monitor esetén, hogy az adott monitor nettó ára a nettó átlag ár alatt van-e, vagy fölötte
            //esetleg pontosan egyenlő az átlag árral. Ezt is a főprogram írja ki.
            static void CheckNetPriceAgainstAverage(List<Monitor> monitorok)
            {
                double averageNetPrice = monitorok.Average(m => m.Ar / 1.27); // Calculate the average net price

                foreach (var monitor in monitorok)
                {
                    double netPrice = monitor.Ar / 1.27;
                    string status = netPrice < averageNetPrice ? "magasabb" :
                                    netPrice > averageNetPrice ? "alacsonyabb" : "egyenlő";
                    Console.WriteLine($"\n{monitor.Tipus} ára {status} az átlagos internetes árnál.");
                }
            }

            CheckNetPriceAgainstAverage(monitorok);




            //9. Modellezzük, hogy megrohamozták a vevők a boltot. 5 és 15 közötti random számú vásárló 1 vagy 2 random módon kiválasztott monitort vásárol,
            //ezzel csökkentve az eredeti készletet. Írd ki, hogy melyik monitorból mennyi maradt a boltban.
            //Vigyázz, hogy nulla darab alá ne mehessen a készlet. Ha az adott monitor éppen elfogyott, ajánlj neki egy másikat (lásd fent).
            static void SimulateCustomerPurchases(List<Monitor> monitorok)
            {
                Random random = new Random();
                List<Monitor> soldMonitors = new List<Monitor>();
                List<Monitor> monitorCopy = new List<Monitor>(monitorok); // Create a copy of the original list

                foreach (var monitor in monitorCopy)
                {
                    int quantitySold = random.Next(1, 3); // 1 or 2 monitors sold per customer
                    if (quantitySold >= monitorok.Count) quantitySold = monitorok.Count - 1;

                    for (int i = 0; i < quantitySold; i++)
                    {
                        if (monitorok.Count > 0)
                        {
                            monitorok.RemoveAt(random.Next(0, monitorok.Count));
                            soldMonitors.Add(monitor);
                        }
                    }
                }

                foreach (var monitor in monitorCopy) // Iterate over the copy
                {
                    Console.WriteLine($"Megmaradat {monitor.Gyarto} {monitor.Tipus} száma:  {monitorok.Count(m => m.Gyarto == monitor.Gyarto && m.Tipus == monitor.Tipus)}");
                }

                if (monitorok.All(m => m.Ar == 0))
                {
                    Console.WriteLine("All monitors are sold out.");
                }
            }
            SimulateCustomerPurchases(monitorok);



            //10. Írd ki a képernyőre, hogy a vásárlások után van-e olyan monitor, amelyikből mindegyik elfogyott (igen/nem).




            //11. Írd ki a gyártókat abc sorrendben a képernyőre. Oldd meg úgy is, hogy a metódus írja ki, és úgy is, hogy a főprogram.

            static void ManufacturersInAlphOrder(List<Monitor> monitorok)
            {
                var manufacturers = monitorok.Select(m => m.Gyarto).Distinct().OrderBy(m => m);
                Console.WriteLine("Gyártók abc sorrendben:");
                foreach (var manufacturer in manufacturers)
                {
                    Console.WriteLine(manufacturer);
                }
            }

            ManufacturersInAlphOrder(monitorok);


            //12. Csökkentsd a legdrágább monitor bruttó árát 10%-kal, írd ki ezt az értéket a képernyőre.

            static void ReducePrice(List<Monitor> monitorok)
            {
                var mostExpensiveMonitor = monitorok.OrderByDescending(m => m.Ar).FirstOrDefault();
                if (mostExpensiveMonitor != null)
                {
                    double discount = mostExpensiveMonitor.Ar * 0.10;
                    mostExpensiveMonitor.Ar -= (int)discount;
                    Console.WriteLine($"\nA legdrágább monitor ára 10%-kal csökkentve: {mostExpensiveMonitor.Ar} Ft");
                }
                else
                {
                    Console.WriteLine("Nincs monitor a készleten.");
                }
            }

            ReducePrice(monitorok);
        }
    }
}
