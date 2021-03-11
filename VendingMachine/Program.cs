using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    class Program
    {
        private static decimal _totalAmount = 0;
        private static Dictionary<int, decimal> _productPrices = new Dictionary<int, decimal>()
        {
            [1] = 50,
            [2] = 30,
            [3] = 60,
            [4] = 10,
            [5] = 20,
            [6] = 40,
            [7] = 80,
            [8] = 10,
            [9] = 10,
            [10] = 20,
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
            var productOptions = _productPrices.Select(x => $"Product({x.Key}) Price:{x.Value}")
                .Skip((page - 1) * 5).Take(5);
            Console.WriteLine($"Buy : {string.Join(" , ", productOptions)}. Or Choose (A)Abandon choose product");
            Console.WriteLine($"Please enten number: ");

            var userInput = Console.ReadLine();
            var selectedProduct = _productPrices.FirstOrDefault(x => userInput == x.Key.ToString());
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
                var productPrice = selectedProduct.Value;
                var temperature = "Hot";
                var ice = string.Empty;
                var lemon = string.Empty;

                Console.WriteLine("Add Ice (Y/N) ?");
                userInput = Console.ReadLine();
                if (userInput.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ice = "added ice";
                }

                Console.WriteLine("Add lemon (Y/N) ?");
                userInput = Console.ReadLine();
                if (userInput.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    lemon = "added lemon";
                }

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
                    Console.WriteLine($"Got Product ({selectedProduct.Key}) {temperature} {ice} {lemon}");
                    _totalAmount -= productPrice;
                }
            }
        }
    }
}
