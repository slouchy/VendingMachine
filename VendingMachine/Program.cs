using System;

namespace VendingMachine
{
    class Program
    {
        private static decimal _totalAmount = 0;

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
                        Console.WriteLine("Selected BUY");
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
    }
}
