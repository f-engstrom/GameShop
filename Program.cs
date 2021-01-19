using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using GameShop.Data;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static System.Console;

namespace GameShop
{
    class Program
    {
        public static GameShopContext context = new GameShopContext();

        static void Main(string[] args)
        {
            Menu();
        }
        static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {


                Clear();

                WriteLine("1. Registrera kund");
                WriteLine("2. Visa kundregister");
                WriteLine("3. Skapa order");
                WriteLine("4. Lista ordrar för kund");
                WriteLine("5. Lista alla ordrar");
                WriteLine("6. Registrera produkter");
                WriteLine("7. Lista produkter");
                WriteLine("8. Avsluta");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        AddCustomer();
                        break;

                    case ConsoleKey.D2:

                        ListCustomers();
                        break;

                    case ConsoleKey.D3:

                        CreateOrder();
                        break;


                    case ConsoleKey.D4:

                        ListOrdersForCustomer();

                        break;

                    case ConsoleKey.D5:

                        ListAllOrders();

                        break;


                    case ConsoleKey.D6:

                        RegisterProduct();

                        break;


                    case ConsoleKey.D7:

                        ListProducts();

                        break;



                    case ConsoleKey.D8:

                        shouldNotExit = false;

                        break;

                }

            }
        }

        private static void RegisterProduct()
        {

            Clear();

            bool customerExists = false;
            bool incorrectKey = true;
            bool doNotExitLoop = true;




            do
            {

                Write("artikelnr.: ");
                string productCode = ReadLine();
                Write("\nNamn: ");
                string productName = ReadLine();
                Write("\nBeskrivning: ");
                string productDescription = ReadLine();
                Write("\nPris: ");
                decimal productPrice = Convert.ToDecimal(ReadLine());



                Product product = new Product(productCode, productName, productDescription, productPrice);


                Console.WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo inputKey;

                do
                {

                    inputKey = ReadKey(true);

                    incorrectKey = !(inputKey.Key == ConsoleKey.J || inputKey.Key == ConsoleKey.N);



                } while (incorrectKey);


                if (inputKey.Key == ConsoleKey.J)
                {


                    var customers = context.Product.FirstOrDefault(r => r.ProductCode == productCode);

                    if (customers == null)
                    {
                        context.Product.Add(product);
                        context.SaveChanges();

                        doNotExitLoop = false;
                        Clear();
                        WriteLine("Artikel registrerad");
                        Thread.Sleep(1000);

                    }
                    else if (customers != null)
                    {
                        doNotExitLoop = false;
                        Clear();
                        WriteLine("Artikelnr. redan registrerat");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (doNotExitLoop);


        }

        private static void ListAllOrders()
        {
            Clear();

            var orders = context.Order.OrderByDescending(o => o.OrderDate).Select(o => new
            {
                OrderId = o.Id,
                CustomerName = $" {o.Customer.LastName}, {o.Customer.FirstName}",
                CustomerAddress = $"{o.Customer.Address.Street}, {o.Customer.Address.Postcode} {o.Customer.Address.City}",
                OrderPlaced = o.OrderDate,
                OrderFinished = o.OrderFinishedDate.HasValue ? o.OrderFinishedDate.ToString() : "Inte levererad",
                OrderdItems = o.Products.Select(po => new
                {
                    Item = po.Product.ProductName,
                    Amount = po.Amount,

                })

            }).ToList();

            foreach (var order in orders)
            {
                WriteLine($"Order id: {order.OrderId}");
                WriteLine($"Order datum: {order.OrderPlaced}");
                WriteLine($"Leveransdatum: {order.OrderFinished}");
                WriteLine($"Kund: {order.CustomerName}");
                WriteLine($"Leveransadress: {order.CustomerAddress}");

                foreach (var item in order.OrderdItems)
                {
                    WriteLine($"Produkt: {item.Item} Antal: {item.Amount} ");
                }
                WriteLine("".PadRight(50, '-'));


            }

            ReadKey();

        }

        private static void ListOrdersForCustomer()
        {

            bool customerExists = false;
            bool incorrectKey = true;
            bool doNotExitLoop = true;

            Clear();

            Write("Ange Kund (person nr.): ");
            string socialSecurityNumber = ReadLine();

            bool isCustomer = false;

            isCustomer = context.Customer.Any(r => r.SocialSecurityNumber == socialSecurityNumber);



            if (!isCustomer)
            {
                Clear();
                WriteLine("Kund ej registrerad");
                Thread.Sleep(1000);

            }
            else if (isCustomer)
            {

                do
                {


                    var customer = context.Customer.Where(r => r.SocialSecurityNumber == socialSecurityNumber).Select(c => new
                    {
                        CustomerName = $" {c.LastName}, {c.FirstName}",
                        CustomerAddress = $"{c.Address.Street}, {c.Address.Postcode} {c.Address.City}",
                        Orders = c.Orders.OrderByDescending(c => c.OrderDate).Select(o => new
                        {
                            OrderPlaced = o.OrderDate,
                            OrderFinished = o.OrderFinishedDate.HasValue ? o.OrderFinishedDate.ToString() : "Inte levererad",
                            OrderId = o.Id,
                            OrderdItems = o.Products.Select(po => new
                            {
                                Item = po.Product.ProductName,
                                Amount = po.Amount,

                            })
                        })
                    }).ToList();



                    Clear();

                    foreach (var cusomerdata in customer)
                    {
                        WriteLine($"\nKund: {cusomerdata.CustomerName}");
                        WriteLine($"Leveransadress: {cusomerdata.CustomerAddress}");
                        WriteLine("".PadRight(50, '-'));
                        foreach (var order in cusomerdata.Orders)
                        {
                            WriteLine($"\nOrder id: {order.OrderId}");
                            WriteLine($"\nOrder datum: {order.OrderPlaced}");
                            WriteLine($"\nLeveransdatum: {order.OrderFinished}");

                            foreach (var item in order.OrderdItems)
                            {
                                WriteLine($"\nProdukt: {item.Item} Antal: {item.Amount} ");
                            }
                            WriteLine("\n".PadRight(50, '-'));
                        }



                    }


                    WriteLine("(K)larrapportera order");
                    WriteLine("Esc för att gå tillbaka till menyn");


                    ConsoleKeyInfo inputKey;

                    do
                    {

                        inputKey = ReadKey(true);

                        incorrectKey = !(inputKey.Key == ConsoleKey.K || inputKey.Key == ConsoleKey.Escape);



                    } while (incorrectKey);

                    if (inputKey.Key == ConsoleKey.K)
                    {
                        Write("Order id: ");
                        int id = Convert.ToInt32(ReadLine());

                        Order order = context.Order.FirstOrDefault(r => r.Id == id);

                        if (!order.OrderFinishedDate.HasValue)
                        {
                            order.OrderDone();
                            context.SaveChanges();
                            Console.WriteLine("Leverans registrerad");
                            Thread.Sleep(1000);

                        }
                        else
                        {
                            Console.WriteLine("Order redan levererad");
                            Thread.Sleep(1000);
                        }



                    }

                    if (inputKey.Key == ConsoleKey.Escape)
                    {

                        doNotExitLoop = false;


                    }


                } while (doNotExitLoop);












            }
        }

        public static void AddCustomer()
        {
            Clear();

            bool customerExists = false;
            bool incorrectKey = true;
            bool doNotExitLoop = true;




            do
            {

                Console.WriteLine("First Name: ");
                string firstName = Console.ReadLine();
                Console.WriteLine("Last Name: ");
                string lastName = Console.ReadLine();
                Console.WriteLine("Social security number: ");
                string socialSecurityNumber = Console.ReadLine();

                Console.WriteLine("Street: ");
                string street = Console.ReadLine();
                Console.WriteLine("Postcode: ");
                string postcode = Console.ReadLine();
                Console.WriteLine("City: ");
                string city = Console.ReadLine();

                Address address = new Address(street, postcode, city);


                Customer customer = new Customer(firstName, lastName, socialSecurityNumber, address);

                Console.WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo inputKey;

                do
                {

                    inputKey = ReadKey(true);

                    incorrectKey = !(inputKey.Key == ConsoleKey.J || inputKey.Key == ConsoleKey.N);



                } while (incorrectKey);


                if (inputKey.Key == ConsoleKey.J)
                {


                    var customers = context.Customer.FirstOrDefault(r => r.SocialSecurityNumber == socialSecurityNumber);

                    if (customers == null)
                    {
                        context.Customer.Add(customer);
                        context.SaveChanges();

                        doNotExitLoop = false;
                        Clear();
                        WriteLine("Kund registrerad");
                        Thread.Sleep(1000);

                    }
                    else if (customers != null)
                    {
                        doNotExitLoop = false;
                        Clear();
                        WriteLine("Kund redan registrerad");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (doNotExitLoop);



        }

        public static void CreateOrder()
        {

            bool incorrectKey = true;
            bool doNotExitLoop = true;


            Clear();

            Write("Kund (person nr.): ");
            string socialSecurityNumber = ReadLine();


            Order order = new Order();

            var customer = context.Customer.FirstOrDefault(r => r.SocialSecurityNumber == socialSecurityNumber);


            if (customer == null)
            {
                Clear();
                WriteLine("Kund ej registrerad");
                Thread.Sleep(1000);

            }

            else if (customer != null)
            {


                do
                {


                    WriteLine($"{customer.FirstName} {customer.LastName},  {customer.SocialSecurityNumber}");

                    WriteLine("Artikel");
                    WriteLine("".PadRight(50, '-'));

                    foreach (var product in order.Products)
                    {
                        WriteLine(product.Product.ProductDescription);
                    }

                    WriteLine("(L)ägg till (S)kapa order");


                    ConsoleKeyInfo inputKey;

                    do
                    {

                        inputKey = ReadKey(true);

                        incorrectKey = !(inputKey.Key == ConsoleKey.L || inputKey.Key == ConsoleKey.S);



                    } while (incorrectKey);

                    if (inputKey.Key == ConsoleKey.L)
                    {
                        Write("Artikelnr: ");
                        string productCode = ReadLine();
                        WriteLine("Antal: ");
                        int amount = Convert.ToInt32(ReadLine());

                        var product = context.Product.FirstOrDefault(r => r.ProductCode == productCode);
                        // var currentOrder = customer.Orders.FirstOrDefault().Products;

                        ProductOrder productOrder = new ProductOrder(product, amount);

                        order.Products.Add(productOrder);


                    }

                    if (inputKey.Key == ConsoleKey.S)
                    {
                        customer.Orders.Add(order);
                        context.SaveChanges();
                        doNotExitLoop = false;
                        Console.WriteLine("Order skapad");
                        Thread.Sleep(1000);

                    }


                } while (doNotExitLoop);
            }


        }

        public static void ListCustomers()
        {
            Clear();

            var customers = context.Customer.Include(a => a.Address);

            WriteLine("Namn".PadRight(25, ' ') + "Adress");
            WriteLine("".PadRight(50, '-'));

            foreach (var customer in customers)
            {

                WriteLine($"{customer.FirstName} {customer.LastName}, {customer.SocialSecurityNumber}        {customer.Address.Street}, {customer.Address.Postcode} {customer.Address.City}");

            }

            ReadKey();

        }

        public static void ListProducts()
        {

            Clear();

            var products = context.Product;

            WriteLine("Produkter");

            foreach (var product in products)
            {
                
                WriteLine($"Artikelnummer: {product.ProductCode}");
                WriteLine($"Produktnamn: {product.ProductName}");
                WriteLine($"Beskrivning: {product.ProductDescription}");
                WriteLine($"Pris: " + product.ProductPrice.ToString("C", new CultureInfo("sv-SE")));
                WriteLine("".PadRight(50,'-'));

            }

            ReadKey();

        }

    }
}
