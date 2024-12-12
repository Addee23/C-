using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var users = new List<User>();
        var books = new List<Book>();

        SeedData(users, books); //Fyller listorna med exempeldata.

        while (true)
        {
            Console.Clear(); // Rensar konsolen.
            Console.WriteLine("Bibliotekshanteringssystem");
            Console.WriteLine("[1] Logga in som admin");
            Console.WriteLine("[2] Logga in som användare");
            Console.WriteLine("[3] Avsluta");
            Console.Write("Välj ett alternativ: ");
            var input = Console.ReadLine(); //Användarens val

            switch (input)
            {
                case "1":
                    AdminMenu(users, books);
                    break;
                case "2":
                    UserMenu(users, books);
                    break;
                case "3":
                    Console.WriteLine("Programmet avslutas. Hej då!");
                    Environment.Exit(0); // Avslutar programmet direkt
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void SeedData(List<User> users, List<Book> books)  //Exempel läggs in i listorna
    {
        users.Add(new User { Username = "admin", Password = "admin", Role = "Admin" });
        users.Add(new User { Username = "user", Password = "password", Role = "User" });

        books.Add(new Book { Id = 1, Title = "Sagan om ringen", Author = "J.R.R. Tolkien", IsBorrowed = false });
        books.Add(new Book { Id = 2, Title = "Harry Potter", Author = "J.K. Rowling", IsBorrowed = false });
        books.Add(new Book { Id = 3, Title = "Moby Dick", Author = "Herman Melville", IsBorrowed = true });
        books.Add(new Book { Id = 4, Title = "The Secret", Author = "Rhonda Byrne", IsBorrowed = false });
    }

    static void AdminMenu(List<User> users, List<Book> books) //Administratörens login sida.
    {
        var admin = Login(users, "Admin");
        if (admin == null) //Felhantering
        {
            Console.WriteLine("Inloggningen misslyckades. Återvänder till huvudmenyn...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Välkommen, {admin.Username} (Admin)!");
            Console.WriteLine("[1] Lista alla böcker");
            Console.WriteLine("[2] Lägg till en bok");
            Console.WriteLine("[3] Ta bort en bok");
            Console.WriteLine("[4] Lägg till en användare"); // Nytt alternativ för att lägga till användare
            Console.WriteLine("[0] Logga ut");
            Console.Write("Välj ett alternativ: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ListBooks(books); //Visa listade böcker
                    break;
                case "2":
                    AddBook(books); //Lägg till bok
                    break;
                case "3":
                    RemoveBook(books); //Ta bort bok
                    break;
                case "4":
                    AddUser(users); // Lägg till användare
                    break;
                case "0":
                    Console.WriteLine("Loggar ut...");
                    Console.ReadKey();
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void UserMenu(List<User> users, List<Book> books) // User-menyn
    {
        var user = Login(users, "User");
        if (user == null)
        {
            Console.WriteLine("Inloggningen misslyckades. Återvänder till huvudmenyn...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Välkommen, {user.Username}!");
            Console.WriteLine("[1] Lista alla böcker");
            Console.WriteLine("[2] Låna en bok");
            Console.WriteLine("[3] Återlämna en bok");
            Console.WriteLine("[0] Logga ut");
            Console.Write("Välj ett alternativ: ");
            var input = Console.ReadLine();

            switch (input) //Alternativ för en inloggad user
            {
                case "1":
                    ListBooks(books);
                    break;
                case "2":
                    BorrowBook(books);
                    break;
                case "3":
                    ReturnBook(books);
                    break;
                case "0":
                    Console.WriteLine("Loggar ut...");
                    Console.ReadKey();
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static User Login(List<User> users, string role) //Lista med användare
    {
        Console.Write("Ange användarnamn: ");
        var username = Console.ReadLine();
        Console.Write("Ange lösenord: ");
        var password = Console.ReadLine();

        var user = users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == role); //Söker efter den första matchningen av villkoren.
        if (user != null)
        {
            Console.WriteLine($"Inloggad som {role}. Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
            return user;
        }

        Console.WriteLine("Felaktigt användarnamn eller lösenord. Återvänder till huvudmenyn...");
        Console.ReadKey();
        return null;
    }

    static void ListBooks(List<Book> books) //Lista med böcker. Metod för att visa listade böcker
    {
        Console.WriteLine("\n[Lista över böcker]");
        foreach (var book in books) //Går igenom hela listan Book
        {
            var status = book.IsBorrowed ? "Utlånad" : "Tillgänglig";
            Console.WriteLine($"ID: {book.Id}, Titel: {book.Title}, Författare: {book.Author}, Status: {status}");
        }
        Console.WriteLine("\nTryck på valfri tangent för att återvända till menyn...");
        Console.ReadKey();
    }

    static void AddBook(List<Book> books)//Lista med böcker. Metod för att lägga till en ny bok
                                            
    {
        Console.Write("Ange boktitel: ");
        var title = Console.ReadLine();
        Console.Write("Ange författare: ");
        var author = Console.ReadLine();
        int newId;
        if (books.Any()) //Metoden Any kontrollerar om det finns element i books.
        {
            newId = books.Max(b => b.Id) + 1; //hittar det högst existerande id och ökar det med 1.      

        }
        else
        {
            newId = 1;
        }

        Console.WriteLine("Bok tillagd! Tryck på valfri tangent för att återvända till menyn...");
        Console.ReadKey();
    }


    static void RemoveBook(List<Book> books)
    {
        Console.Write("Ange ID för boken att ta bort: ");
        string input = Console.ReadLine(); 

        // Kontrollera att hela inmatningen är numerisk genom att använda en loop
        bool isNumber = true;
        for (int i = 0; i < input.Length; i++) 
        {
            if (input[i] < '0' || input[i] > '9') // Kontrollera om varje tecken är mellan '0' och '9'
            {
                isNumber = false;
                break;
            }
        }

        if (isNumber && input.Length > 0) // Om strängen är numerisk och inte tom
        {
            int id = int.Parse(input); 

            // Leta efter boken
            Book bookToRemove = null;
            foreach (var book in books)
            {
                if (book.Id == id)
                {
                    bookToRemove = book;
                    break; 
                }
            }

            if (bookToRemove != null) // Om boken hittades
            {
                books.Remove(bookToRemove); // Ta bort boken från listan
                Console.WriteLine("Boken har tagits bort!");
            }
            else
            {
                Console.WriteLine("Ingen bok hittades med det ID:t.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt ID."); // Om inmatningen inte är numerisk
        }

        Console.WriteLine("Tryck på valfri tangent för att återvända till menyn...");
        Console.ReadKey();
    }


    static void BorrowBook(List<Book> books) //Lista med böcker
    {
        Console.Write("Ange ID för boken du vill låna: ");
        string input = Console.ReadLine(); 

        // Kontrollera om inmatningen är numerisk
        bool isNumber = true;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] < '0' || input[i] > '9') // Kontrollera om varje tecken är en siffra
            {
                isNumber = false;
                break;
            }
        }

        if (isNumber && input.Length > 0) // Kontrollera att inmatningen är numerisk och inte tom
        {
            int id = int.Parse(input); 
            Book bookToBorrow = null;

            // Leta efter en bok som inte är utlånad
            foreach (var book in books)
            {
                if (book.Id == id && !book.IsBorrowed)
                {
                    bookToBorrow = book;
                    break;
                }
            }

            if (bookToBorrow != null) // Om boken hittades och inte är utlånad
            {
                bookToBorrow.IsBorrowed = true;
                Console.WriteLine($"Du har lånat '{bookToBorrow.Title}'.");
            }
            else
            {
                Console.WriteLine("Boken är inte tillgänglig.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt ID.");
        }

        Console.WriteLine("Tryck på valfri tangent för att återvända till menyn...");
        Console.ReadKey();
    }


    static void ReturnBook(List<Book> books)
    {
        Console.Write("Ange ID för boken du vill återlämna: ");
        string input = Console.ReadLine(); 

        // Felkontroll. Kontrollerar om inmatningen är numerisk
        bool isNumber = true;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] < '0' || input[i] > '9') // Kontrollerar om varje tecken är en siffra
            {
                isNumber = false;
                break;
            }
        }

        if (isNumber && input.Length > 0) // Kontrollerar att inmatningen är numerisk och inte tom
        {
            int id = int.Parse(input); // Konverterar till heltal
            Book bookToReturn = null;

            // Leta efter en bok som är utlånad i listan books
            foreach (var book in books)
            {
                if (book.Id == id && book.IsBorrowed) //Om en matchande bok hittas och inte är nullmarkeras den som återlämnad.
                {
                    bookToReturn = book;
                    break;
                }
            } 

            if (bookToReturn != null) // Om boken hittades och är utlånad
            {
                bookToReturn.IsBorrowed = false;
                Console.WriteLine($"Du har återlämnat '{bookToReturn.Title}'.");
            }
            else
            {
                Console.WriteLine("Boken är inte utlånad eller finns inte.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt ID.");
        }

        Console.WriteLine("Tryck på valfri tangent för att återvända till menyn...");
        Console.ReadKey();
    }


    static void AddUser(List<User> users) //Lista med användare. Lägg till en användare. Tillgänglig endast för admin
    {
        Console.Write("Ange användarnamn för den nya användaren: ");
        var username = Console.ReadLine();
        Console.Write("Ange lösenord för den nya användaren: ");
        var password = Console.ReadLine();
        Console.Write("Ange roll för den nya användaren (Admin/User): ");
        var role = Console.ReadLine();

        // Kontrollera att rollen är korrekt
        if (role != "Admin" && role != "User")
        {
            Console.WriteLine("Ogiltig roll. Endast 'Admin' eller 'User' är tillåtna.");
            Console.ReadKey();
            return;
        }

        // Kontrollera att användarnamnet inte redan finns
        if (users.Any(u => u.Username == username))
        {
            Console.WriteLine("Användarnamnet är redan upptaget. Försök igen.");
            Console.ReadKey();
            return;
        }

        // Lägg till användaren
        users.Add(new User { Username = username, Password = password, Role = role });
        Console.WriteLine($"Ny användare '{username}' med rollen '{role}' har lagts till.");
        Console.ReadKey();
    }
}
