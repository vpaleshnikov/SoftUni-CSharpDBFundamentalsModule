namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //P01_AgeRestriction
                //var command = Console.ReadLine();
                //var result = GetBooksByAgeRestriction(db, command);
                //Console.WriteLine(result);

                //P02_GoldenBooks
                //var result = GetGoldenBooks(db);
                //Console.WriteLine(result);

                //P03_BooksByPrice
                //var result = GetBooksByPrice(db);
                //Console.WriteLine(result);

                //P04_NotReleasedIn
                //var year = int.Parse(Console.ReadLine());
                //var result = GetBooksNotRealeasedIn(db, year);
                //Console.WriteLine(result);

                //P05_BookTitlesByCategory
                //var input = Console.ReadLine();
                //var result = GetBooksByCategory(db, input);
                //Console.WriteLine(result);

                //P06_ReleasedBeforeDate
                //var input = Console.ReadLine();
                //var result = GetBooksReleasedBefore(db, input);
                //Console.WriteLine(result);

                //P07_AuthorSearch
                //var input = Console.ReadLine();
                //var result = GetAuthorNamesEndingIn(db, input);
                //Console.WriteLine(result);

                //P08_BookSearch
                //var input = Console.ReadLine();
                //var result = GetBookTitlesContaining(db, input);
                //Console.WriteLine(result);

                //P09_BookSearchByAuthor
                //var input = Console.ReadLine();
                //var result = GetBooksByAuthor(db, input);
                //Console.WriteLine(result);

                //P10_CountBooks
                //var lengthCheck = int.Parse(Console.ReadLine());
                //var count = CountBooks(db, lengthCheck);
                //Console.WriteLine(count);

                //P11_TotalBookCopies
                //var result = CountCopiesByAuthor(db);
                //Console.WriteLine(result);

                //P12_ProfitByCategory
                //var result = GetTotalProfitByCategory(db);
                //Console.WriteLine(result);

                //P13_MostRecentBooks
                //var result = GetMostRecentBooks(db);
                //Console.WriteLine(result);

                //P14_IncreasePrices
                //IncreasePrices(db);

                //P15_RemoveBooks
                //var result = RemoveBooks(db);
                //Console.WriteLine($"{result} books were deleted");
            }
        }

        //P01_AgeRestriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var enumValue = -1;

            if (command.ToLower() == "minor")
            {
                enumValue = 0;
            }
            else if (command.ToLower() == "teen")
            {
                enumValue = 1;
            }
            else if (command.ToLower() == "adult")
            {
                enumValue = 2;
            }

            var titles = context
                .Books
                .Where(b => b.AgeRestriction == (AgeRestriction)enumValue)
                .Select(b => b.Title)
                .OrderBy(t => t);

            var result = string.Join(Environment.NewLine, titles);
            return result;
        }

        //P02_GoldenBooks
        public static string GetGoldenBooks(BookShopContext context)
        {
            var titles = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, titles);
            return result;
        }

        //P03_BooksByPrice
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}");

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        //P04_NotReleasedIn
        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var titles = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, titles);
            return result;
        }

        //P05_BookTitlesByCategory
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(new[] { " ", "\t", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var titles = context
                .Books
                .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t);

            var result = string.Join(Environment.NewLine, titles);
            return result;
        }

        //P06_ReleasedBeforeDate
        public static string GetBooksReleasedBefore(BookShopContext context, string input)
        {
            var inputTokens = input.Split('-');
            var day = int.Parse(inputTokens[0]);
            var month = int.Parse(inputTokens[1]);
            var year = int.Parse(inputTokens[2]);

            var date = new DateTime(year, month, day);

            var books = context
                .Books
                .Where(b => b.ReleaseDate < date)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}");

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        //P07_AuthorSearch
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var autors = context
                .Authors
                .Where(n => n.FirstName.EndsWith(input))
                .Select(n => $"{n.FirstName} {n.LastName}")
                .OrderBy(n => n);

            var result = string.Join(Environment.NewLine, autors);
            return result;
        }

        //P08_BookSearch
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, titles);
            return result;
        }

        //P09_BookSearchByAuthor
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        //P10_CountBooks
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var count = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return count;
        }

        //P11_TotalBookCopies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context
                .Authors
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books
                    .Select(b => b.Copies)
                    .Sum()
                })
                .OrderByDescending(a => a.Copies)
                .Select(a => $"{a.Name} - {a.Copies}");

            var result = string.Join(Environment.NewLine, authors);
            return result;
        }

        //P12_ProfitByCategory
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
                })
                .OrderByDescending(b => b.TotalProfit)
                .ThenBy(b => b.CategoryName)
                .Select(b => $"{b.CategoryName} ${b.TotalProfit:f2}");

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        //P13_MostRecentBooks
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Book = c.CategoryBooks
                    .Select(cb => cb.Book)
                    .OrderByDescending(cb => cb.ReleaseDate)
                    .Take(3)
                });

            var result = new StringBuilder();

            foreach (var category in categories)
            {
                result.AppendLine($"--{category.Name}");

                foreach (var title in category.Book)
                {
                    result.AppendLine($"{title.Title} ({title.ReleaseDate.Value.Year})");
                }
            }

            return result.ToString().Trim();
        }

        //P14_IncreasePrices
        public static void IncreasePrices(BookShopContext context)
        {
            context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        //P15_RemoveBooks
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Copies < 4200);

            int result = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return result;
        }
    }
}
