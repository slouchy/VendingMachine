using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    class Program
    {
        private static decimal _totalAmount = 0;

        private static bool _isContinue = true;

        private static List<ShelfModel> _shelfs = new List<ShelfModel>
        {
            new ShelfModel
            {
                Idx = 0,
                Required = new List<MixtureModel>
                {
                    new MixtureModel
                    {
                        Idx=0,
                        Name = Mixture.Hot
                    }
                },
                Optional = new List<MixtureModel>
                {
                    new MixtureModel
                    {
                        Idx=0,
                        Name = Mixture.Lemon
                    },
                    new MixtureModel
                    {
                        Idx=1,
                        Name = Mixture.Milk
                    },
                }
            },
            new ShelfModel
            {
                Idx = 1,
                Required = new List<MixtureModel>{
                    new MixtureModel
                    {
                       Idx=0,
                       Name=Mixture.Cold,
                    },
                    new MixtureModel
                    {
                       Idx=1,
                       Name=Mixture.Ice,
                       Price = 5
                    },
                    new MixtureModel
                    {
                        Idx=2,
                        Name = Mixture.Lemon
                    }
                },
            },
            new ShelfModel
            {
                Idx = 2,
                Required = new List<MixtureModel>{
                    new MixtureModel
                    {
                       Idx=0,
                       Name=Mixture.Hot,
                    },
                    new MixtureModel
                    {
                        Idx=2,
                        Name = Mixture.Milk
                    }
                },
                Optional = new List<MixtureModel>
                {
                    new MixtureModel
                    {
                       Idx=1,
                       Name=Mixture.Ice,
                       Price=5
                    },
                }
            }
        };

        private static List<ProductModel> _products = new List<ProductModel>
        {
            new ProductModel
            {
                Id=1,
                ShelfIdx = 0,
                LocationIdx = 1,
                Price = 10,
            },
            new ProductModel
            {
                Id=2,
                ShelfIdx = 0,
                LocationIdx = 2,
                Price = 20,
            },
            new ProductModel
            {
                Id=3,
                ShelfIdx = 0,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                Id=4,
                ShelfIdx = 0,
                LocationIdx = 4,
                Price = 40,
            },
             new ProductModel
            {
                Id=5,
                ShelfIdx = 0,
                LocationIdx = 5,
                Price = 50,
            },
            new ProductModel
            {
                Id=6,
                ShelfIdx = 0,
                LocationIdx = 6,
                Price = 60,
            },
               new ProductModel
            {
                Id=7,
                ShelfIdx = 1,
                LocationIdx = 1,
                Price = 10,
            },
            new ProductModel
            {
                Id=8,
                ShelfIdx = 1,
                LocationIdx = 2,
                Price = 20,
            },
            new ProductModel
            {
                Id=9,
                ShelfIdx = 1,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                Id=10,
                ShelfIdx = 1,
                LocationIdx = 4,
                Price = 40,
            },
            new ProductModel
            {
                Id=11,
                ShelfIdx = 2,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                Id=12,
                ShelfIdx = 2,
                LocationIdx = 4,
                Price = 40,
            },
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Hello Vending machine !");
            while (_isContinue)
            {
                ShowMoney();
                ShowMainMenu();
                ChooseMainOptions();

                Console.WriteLine("\r\n===========");
            }
        }

        static void ChooseMainOptions()
        {
            var userInput = Console.ReadLine();
            switch (userInput.ToUpper())
            {
                case "A":
                case "COIN":
                    SetMachineMoney(10);
                    break;
                case "B":
                case "BUY":
                    BuyProduct();
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
                    ShowLeaveMessage();
                    break;
            }
        }

        private static void ShowLeaveMessage()
        {
            Console.WriteLine("LEAVE vending machine");
            _isContinue = false;
        }

        static void SetMachineMoney(decimal price)
        {
            Console.WriteLine($"Set {price} Coint To vending machine");
            _totalAmount += price;
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

        static void BuyProduct()
        {
            DisplayProducts();

            var userInput = Console.ReadLine();
            var selectedProduct = GetUserSelectedProduct(userInput);
            if (selectedProduct == null)
            {
                Console.WriteLine("Error: Not Found Product!!");
                return;
            }

            var (requireds, optionals) = GetShelfOptions(selectedProduct.ShelfIdx);
            var userChooseResults = ChooseOptionalMixture(optionals);
            var nPrice = GetAllMixturePrice(selectedProduct.Price, requireds, userChooseResults);

            if (_totalAmount < nPrice)
            {
                Console.WriteLine($"Error: ProductId ({selectedProduct.Id}) price : ${nPrice}, but there is only : ${_totalAmount}");
                return;
            }

            SetMachineMoney(-nPrice);
            ShowBoughtProductInfo(selectedProduct, nPrice, requireds, optionals);
        }

        private static void ShowBoughtProductInfo(ProductModel product, decimal nPrice, List<MixtureModel> requireds, List<MixtureModel> optionals)
        {
            Console.WriteLine($"ProductId ({product.Id}), shelf: {product.ShelfIdx}, Price: {nPrice}, with {string.Join(",", requireds.Select(x => x.Name))} and {string.Join(",", optionals.Select(x => x.Name))}");
        }

        private static decimal GetAllMixturePrice(decimal price, List<MixtureModel> requireds, List<MixtureModel> userChooseResult)
        {
            foreach (var item in requireds)
            {
                price += item.Price;
            }

            foreach (var item in userChooseResult)
            {
                price += item.Price;
            }

            return price;
        }

        private static ProductModel GetUserSelectedProduct(string userInput)
        {
            return _products.FirstOrDefault(x => x.Id.ToString() == userInput);
        }

        private static void DisplayProducts()
        {
            var products = _products.Select(x => $"ProductId ({x.Id}), Price ${x.Price}, Shelf {x.ShelfIdx}");
            Console.WriteLine($"Display ProductList:\r\n{string.Join("\r\n", products)}\r\n Choose By ProductId (1,2..):");
        }

        static (List<MixtureModel> required, List<MixtureModel> optional) GetShelfOptions(int shlfIdx)
        {
            var selected = _shelfs.First(x => x.Idx == shlfIdx);
            var requireds = selected.Required == null ? new List<MixtureModel>() : selected.Required;
            var optionals = selected.Optional == null ? new List<MixtureModel>() : selected.Optional;

            return (requireds, optionals);
        }

        static List<MixtureModel> ChooseOptionalMixture(List<MixtureModel> options)
        {
            if (options == null)
            {
                return new List<MixtureModel>();
            }

            var selectedResult = new List<MixtureModel>();
            foreach (var item in options)
            {
                Console.WriteLine($"Do you Add {item.Name} ,Price ${item.Price} (Y/N)?");
                var userInput = Console.ReadLine();
                if (userInput.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    selectedResult.Add(item);
                }
            }

            return selectedResult;
        }

    }

    public class ShelfModel
    {
        public int Idx { get; set; }

        public List<MixtureModel> Optional { get; set; }

        public List<MixtureModel> Required { get; set; }
    }

    public class MixtureModel
    {
        public int Idx { get; set; }

        public Mixture Name { get; set; }

        public decimal Price { get; set; } = 0;
    }

    public class ProductModel
    {

        public int Id { get; set; }
        public int ShelfIdx { get; set; }

        public int LocationIdx { get; set; }

        public decimal Price { get; set; }
    }

    public enum Mixture
    {
        Ice,
        Milk,
        Lemon,
        Hot,
        Cold,
    }
}
