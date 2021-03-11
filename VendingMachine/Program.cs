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
        };
        private static decimal _productPrice = 50;


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
                        var minProdcutPrice = _productPrices.Min(x => x.Value);
                        if (_totalAmount < minProdcutPrice)
                        {
                            Console.WriteLine($"Min product price is ${minProdcutPrice}, but there is only total: ${_totalAmount}");
                        }
                        else
                        {
                            ChooseProduct();
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

        static void ChooseProduct()
        {
            IEnumerable<KeyValuePair<int, decimal>> filterProducts = _productPrices.Where(x => x.Value <= _totalAmount);
            var productOption = filterProducts.Select(x => $"Product ({x.Key}) Price: {x.Value}");
            Console.WriteLine($"Can Buy : {string.Join(" , ", productOption)}. Or Choose (A)Abandon choose product");
            Console.WriteLine($"Please enten number: ");
            var userInput = Console.ReadLine();

            var selectedProduct = filterProducts.FirstOrDefault(x => userInput == x.Key.ToString());

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
                Console.WriteLine($"Got Product ({selectedProduct.Key})");
                _totalAmount -= selectedProduct.Value;
            }
        }
    }
}
