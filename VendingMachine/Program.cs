using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    class Program
    {
        private static decimal _totalAmount = 0;
        private static List<MyProduct> _products = new List<MyProduct>()
        {
            new MyProduct
            {
                Location = 1,
                Key = 1,
                Price = 50
            },
            new MyProduct
            {
                Location = 1,
                Key = 2,
                Price = 30
            },
            new MyProduct
            {
                Location = 1,
                Key = 3,
                Price = 60
            },
            new MyProduct
            {
                Location = 1,
                Key = 4,
                Price = 10
            },new MyProduct
            {
                Location = 1,
                Key = 5,
                Price = 20
            },
            new MyProduct
            {
                Location = 1,
                Key = 6,
                Price = 40
            },
            new MyProduct
            {
                Location = 2,
                Key = 7,
                Price = 80
            },
            new MyProduct
            {
                Location = 2,
                Key = 8,
                Price = 10
            },new MyProduct
            {
                Location = 2,
                Key = 9,
                Price = 10
            },
            new MyProduct
            {
                Location = 2,
                Key = 10,
                Price = 20
            },
            new MyProduct
            {
                Location = 3,
                Key = 11,
                Price = 10
            },
            new MyProduct
            {
                Location = 3,
                Key = 12,
                Price = 60
            },
        };



        static void Main(string[] args)
        {
            bool isContinue = true;
            Console.WriteLine("Hello Vending machine !");
            while (isContinue)
            {
                ShowMoney();
                ShowMainMenu();
                var userInput = Console.ReadLine();
                switch (userInput.ToUpper())
                {
                    case "A":
                    case "COIN":
                        Console.WriteLine("ADD 10 Coint To vending machine");
                        _totalAmount += 10;
                        break;
                    case "B":
                    case "BUY":
                        Console.WriteLine("Choose Product page (1/2)");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out var tmp) && (tmp > 0 && tmp < 3))
                        {
                            ChooseProduct(tmp);
                        }
                        else
                        {
                            Console.WriteLine("product page error");
                        }

                        break;
                    case "C":
                    case "REFUND":
                        RefundMoney();
                        break;
                    case "D":
                    case "SETPRODUCT":
                        Console.WriteLine("Selected SETPRODUCT");
                        break;
                    case "E":
                    case "LEAVE":
                        if (_totalAmount != 0)
                        {
                            RefundMoney();
                            ShowMoney();
                        }

                        Console.WriteLine("LEAVE vending machine");
                        isContinue = false;
                        break;
                }

                Console.WriteLine("\r\n===========");
            }
        }

        static void ShowMoney()
        {
            Console.WriteLine($"Now Total : ${_totalAmount}");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("Options: (A)Coin, (B)Buy, (C)Refund, (D)SetProduct, (E)Leave");
        }

        static void RefundMoney()
        {
            Console.WriteLine($"Refund Money : ${_totalAmount}");
            _totalAmount = 0;

        }

        static void ChooseProduct(int page)
        {
            var productOptions = _products.Select(x => $"Product({x.Key}) Price:{x.Price}")
                .Skip((page - 1) * 5).Take(5);
            Console.WriteLine($"Buy : {string.Join(" , ", productOptions)}. Or Choose (A)Abandon choose product");
            Console.WriteLine($"Please enten number: ");

            var userInput = Console.ReadLine();
            var selectedProduct = _products.FirstOrDefault(x => userInput == x.Key.ToString());
            if (userInput.Equals("A", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("Abandon", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Leave choose product");
            }
            else if (selectedProduct.Key == 0)
            {
                Console.WriteLine("Can't find product");
            }
            else
            {
                var productPrice = selectedProduct.Price;
                var temperature = "Hot";
                var burdens = new List<string>();
                burdens.Add(GetAddBurden("ice"));
                burdens.Add(GetAddBurden("lemon"));

                Console.WriteLine("Choose Product is (C)Cold +$5 or (H)Hot ?");
                userInput = Console.ReadLine();
                if (userInput.Equals("C", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("Cold", StringComparison.OrdinalIgnoreCase))
                {
                    productPrice += 5;
                    temperature = "Cold";
                }

                if (_totalAmount < productPrice)
                {
                    Console.WriteLine($"Product ({selectedProduct.Key}) {temperature} price : ${productPrice}, but there is only : ${_totalAmount}");
                }
                else
                {
                    var addBurden = burdens.Any() ? $"Added {string.Join(", ", burdens)}" : string.Empty;
                    Console.WriteLine($"Got Product ({selectedProduct.Key}) {temperature} {addBurden}");
                    _totalAmount -= productPrice;
                }
            }
        }

        static string GetAddBurden(string burden)
        {
            Console.WriteLine($"Add {burden} (Y/N) ?");
            var userInput = Console.ReadLine();
            return userInput.Equals("Y", StringComparison.OrdinalIgnoreCase) ?
                burden :
                string.Empty;
        }
    }

    public class MyProduct
    {
        public int Location { get; set; }

        public int Key { get; set; }

        public decimal Price { get; set; }
    }
}
