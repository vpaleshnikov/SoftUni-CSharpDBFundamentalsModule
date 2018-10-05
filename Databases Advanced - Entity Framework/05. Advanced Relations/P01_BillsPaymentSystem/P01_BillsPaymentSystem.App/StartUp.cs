namespace P01_BillsPaymentSystem.App
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;
    using System;
    using System.Linq;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (var dbContext = new BillsPaymentSystemContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();

                Seed(dbContext);
            }

            Console.Write("Enter User's ID: ");
            var userId = int.Parse(Console.ReadLine());

            using (var dbContext = new BillsPaymentSystemContext())
            {
                GetUserInfoById(dbContext, userId);
            }

            using (var context = new BillsPaymentSystemContext())
            {
                Console.Write("Enter amount for bill pays: ");
                var amount = decimal.Parse(Console.ReadLine());

                try
                {
                    PayBills(userId, amount, context);

                    Console.WriteLine("Bills have been successfully paid.");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void PayBills(int userId, decimal amount, BillsPaymentSystemContext context)
        {
            decimal allАvailableMoneyOfUser = 0m;

            var bankAccounts = context
                .BankAccounts.Join(context.PaymentMethods,
                    (ba => ba.BankAccountId),
                    (p => p.BankAccountId),
                    (ba, p) => new
                    {
                        UserId = p.UserId,
                        BankAccountId = ba.BankAccountId,
                        Balance = ba.Balance
                    })
                .Where(ba => ba.UserId == userId)
                .ToList();

            var creditCards = context
                .CreditCards.Join(context.PaymentMethods,
                    (c => c.CreditCardId),
                    (p => p.CreditCardId),
                    (c, p) => new
                    {
                        UserId = p.UserId,
                        CreditCardId = c.CreditCardId,
                        LimitLeft = c.LimitLeft
                    })
                .Where(c => c.UserId == userId)
                .ToList();

            allАvailableMoneyOfUser += bankAccounts.Sum(b => b.Balance);
            allАvailableMoneyOfUser += creditCards.Sum(c => c.LimitLeft);

            if (allАvailableMoneyOfUser < amount)
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            bool isPayBills = false;
            foreach (var bankAccount in bankAccounts.OrderBy(b => b.BankAccountId))
            {
                var currentAccount = context.BankAccounts.Find(bankAccount.BankAccountId);

                if (amount <= currentAccount.Balance)
                {
                    currentAccount.Withdraw(amount);
                    isPayBills = true;
                }
                else
                {
                    amount -= currentAccount.Balance;
                    currentAccount.Withdraw(currentAccount.Balance);
                }

                if (isPayBills)
                {
                    context.SaveChanges();
                    return;
                }
            }

            foreach (var creditCard in creditCards.OrderBy(c => c.CreditCardId))
            {
                var currentCreditCard = context.CreditCards.Find(creditCard.CreditCardId);

                if (amount <= currentCreditCard.LimitLeft)
                {
                    currentCreditCard.Withdraw(amount);
                    isPayBills = true;
                }
                else
                {
                    amount -= currentCreditCard.LimitLeft;
                    currentCreditCard.Withdraw(currentCreditCard.LimitLeft);
                }

                if (isPayBills)
                {
                    context.SaveChanges();
                    return;
                }
            }
        }

        private static void GetUserInfoById(BillsPaymentSystemContext context, int userId)
        {
            var user = context
                .Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    Name = $"{u.FirstName} {u.LastName}",
                    BankAccounts = u.PaymentMethods
                    .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                    .Select(pm => new
                    {
                        ID = pm.BankAccountId,
                        Balance = pm.BankAccount.Balance,
                        Bank = pm.BankAccount.BankName,
                        Swift = pm.BankAccount.SwiftCode
                    })
                    .ToList(),
                    CreditCards = u.PaymentMethods
                    .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                    .Select(pm => new
                    {
                        ID = pm.CreditCardId,
                        Limit = pm.CreditCard.Limit,
                        MoneyOwed = pm.CreditCard.MoneyOwed,
                        LimitLeft = pm.CreditCard.LimitLeft,
                        ExpirationDate = pm.CreditCard.ExpirationDate
                    })
                    .ToList()
                })
                .FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                Environment.Exit(0);
            }

            Console.WriteLine($"User: {user.Name}");

            var bankAccounts = user.BankAccounts;

            if (bankAccounts.Any())
            {
                Console.WriteLine("Bank Accounts:");

                foreach (var bankAccount in bankAccounts)
                {
                    Console.WriteLine($"-- ID: {bankAccount.ID}" + Environment.NewLine +
                                      $"--- Balance: {bankAccount.Balance:F2}" + Environment.NewLine +
                                      $"--- Bank: {bankAccount.Bank}" + Environment.NewLine +
                                      $"--- SWIFT: {bankAccount.Swift}");
                }
            }

            var creditCards = user.CreditCards;

            if (creditCards.Any())
            {
                Console.WriteLine("Credit Cards:");

                foreach (var creditCard in creditCards)
                {
                    Console.WriteLine($"-- ID: {creditCard.ID}" + Environment.NewLine +
                                      $"--- Limit: {creditCard.Limit:F2}" + Environment.NewLine +
                                      $"--- Money Owed: {creditCard.MoneyOwed:F2}" + Environment.NewLine +
                                      $"--- Limit Left: {creditCard.LimitLeft:F2}" + Environment.NewLine +
                                      $"--- Expiration Date: {creditCard.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
                }
            }

            Console.WriteLine();
        }

        private static void Seed(BillsPaymentSystemContext context)
        {
            using (context)
            {
                var user = new User()
                {
                    FirstName = "Guy",
                    LastName = "Gilbert",
                    Email = "guy.gilbert@mail.com",
                    Password = "GuyGilbert123"
                };

                var creditCard = new CreditCard()
                {
                    ExpirationDate = DateTime.ParseExact("03.03.2020", "dd.MM.yyyy", null),
                    Limit = 800,
                    MoneyOwed = 100
                };

                var bankAccounts = new[] {
                    new BankAccount()
                    {
                        Balance = 2000,
                        BankName = "Unicredit Bulbank",
                        SwiftCode = "UNCRBGSF"
                    },
                    new BankAccount ()
                    {
                        Balance = 1000,
                        BankName = "First Investment Bank",
                        SwiftCode = "FINVBGSF"
                    }
                };

                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod()
                    {
                        User = user,
                        CreditCard = creditCard,
                        Type = PaymentMethodType.CreditCard
                    },
                    new PaymentMethod()
                    {
                        User = user,
                        BankAccount = bankAccounts[0],
                        Type = PaymentMethodType.BankAccount
                    },
                    new PaymentMethod()
                    {
                        User = user,
                        BankAccount = bankAccounts[1],
                        Type = PaymentMethodType.BankAccount
                    }
                };

                context.Users.Add(user);
                context.CreditCards.Add(creditCard);
                context.BankAccounts.AddRange(bankAccounts);
                context.PaymentMethods.AddRange(paymentMethods);

                context.SaveChanges();
            }
        }
    }
}