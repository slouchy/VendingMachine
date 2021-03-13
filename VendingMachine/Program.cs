using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    class Program
    {
        private static decimal _totalAmount = 0;

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
                    },
                }
            }
        };

        private static List<ProductModel> _products = new List<ProductModel>
        {
            new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 1,
                Price = 10,
            },
            new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 2,
                Price = 20,
            },
            new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 4,
                Price = 40,
            },
             new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 5,
                Price = 50,
            },
            new ProductModel
            {
                ShelfIdx = 0,
                LocationIdx = 6,
                Price = 60,
            },
               new ProductModel
            {
                ShelfIdx = 1,
                LocationIdx = 1,
                Price = 10,
            },
            new ProductModel
            {
                ShelfIdx = 1,
                LocationIdx = 2,
                Price = 20,
            },
            new ProductModel
            {
                ShelfIdx = 1,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                ShelfIdx = 1,
                LocationIdx = 4,
                Price = 40,
            },
            new ProductModel
            {
                ShelfIdx = 2,
                LocationIdx = 3,
                Price = 30,
            },
            new ProductModel
            {
                ShelfIdx = 2,
                LocationIdx = 4,
                Price = 40,
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
                isContinue = ChooseMainOptions();

                Console.WriteLine("\r\n===========");
            }
        }

        static bool ChooseMainOptions()
        {
            var isContinue = true;
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
                    Console.WriteLine("LEAVE vending machine");
                    isContinue = false;
                    break;
            }

            return isContinue;
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
            var userInput = string.Empty;
            var totalShelf = _shelfs.Count - 1;
            var selectedShelf = new ShelfModel(); ;
            var selectedProduct = new ProductModel();

            Console.WriteLine($"Choose Product Shelf Number (0~{totalShelf})");
            userInput = Console.ReadLine();
            if (!int.TryParse(userInput, out var tmp1) ||
                tmp1 < 0 ||
                tmp1 > totalShelf)
            {
                Console.WriteLine($"Not Found Shelf.");
                return;
            }
            else
            {
                selectedShelf = _shelfs.First(x => x.Idx == tmp1);
            }

            var (requireds, optionals) = GetShelfOptions(selectedShelf.Idx);
            var products = _products.Where(x => x.ShelfIdx == selectedShelf.Idx);
            var productOptions = products.Select(x => $"({x.LocationIdx}). ${x.Price}");
            Console.WriteLine($"Choose Product Number {string.Join(", ", products.Select(x => x.LocationIdx))}");
            userInput = Console.ReadLine();
            if (!int.TryParse(userInput, out var tmp2) ||
                tmp2 < 0 ||
                tmp2 > products.Max(x => x.LocationIdx))
            {
                Console.WriteLine($"Not Found Product.");
                return;
            }
            else
            {
                selectedProduct = products.First(x => x.LocationIdx == tmp2);
            }

            var userChooseResult = ChooseOptionalMixture(optionals);
            if (_totalAmount < selectedProduct.Price)
            {
                Console.WriteLine($"Shelf {selectedShelf.Idx}, Product ({selectedProduct.LocationIdx}) price : ${selectedProduct.Price}, but there is only : ${_totalAmount}");
                return;
            }

            _totalAmount -= selectedProduct.Price;
            Console.WriteLine($"Buy Shelf {selectedShelf.Idx}, location {selectedProduct.LocationIdx} product, with {string.Join(",", requireds.Select(x => x.Name))} and {string.Join(",", userChooseResult.Select(x => x.Name))}");
        }

        static (List<MixtureModel> required, List<MixtureModel> optional) GetShelfOptions(int shlfIdx)
        {
            var selected = _shelfs.First(x => x.Idx == shlfIdx);
            return (selected.Required, selected.Optional);
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
                Console.WriteLine($"Do you Add {item.Name} (Y/N)?");
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

        public decimal Price { get; set; }
    }

    public class ProductModel
    {
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
