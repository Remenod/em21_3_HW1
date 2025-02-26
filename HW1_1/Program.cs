using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HW1_1
{
    struct Contact
    {
        private string _phoneNumber;
        private string _name;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (!IsValidPhoneNumber(value))
                    throw new ArgumentException($"Invalid phone number format: {value}");
                _phoneNumber = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                _name = value;
            }
        }

        public Contact(string name, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (!IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException($"Invalid phone number format: {phoneNumber}");

            _name = name;
            _phoneNumber = phoneNumber;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(\+380\d{9}|0\d{9})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
    class Service
    {
        private List<Contact> contacts = new List<Contact>();

        public void AddContact(string name, string phoneNumber)
        {
            if (!Contact.IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException($"Invalid phone number format: {phoneNumber}");
            contacts.Add(new Contact(name, phoneNumber));
        }

        public void RemoveContact(int id)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            contacts.RemoveAt(id);
        }

        public void EditContactName(int id, string newName)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            var contact = contacts[id];
            contact.Name = newName;
            contacts[id] = contact;
        }

        public void EditContactPhoneNumber(int id, string newPhoneNumber)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            var contact = contacts[id];
            contact.PhoneNumber = newPhoneNumber;
            contacts[id] = contact;
        }

        public void PrintAllContacts()
        {
            for (int i = 0; i < contacts.Count; i++)
                Console.WriteLine($"ID: {i}, Name: {contacts[i].Name}, Phone number: {contacts[i].PhoneNumber}");
        }

        public Contact GetContact(int id)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            return contacts[id];
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Service service = new Service();
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine("1. Додати контакт");
                Console.WriteLine("2. Видалити контакт");
                Console.WriteLine("3. Змінити ім'я контакту");
                Console.WriteLine("4. Змінити номер телефону контакту");
                Console.WriteLine("5. Показати всі контакти");
                Console.WriteLine("6. Показати контакт за ID");
                Console.WriteLine("7. Вийти");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть ім'я: ");
                        string name = Console.ReadLine();
                        Console.Write("Введіть номер телефону: ");
                        string phoneNumber = Console.ReadLine();
                        try
                        {
                            service.AddContact(name, phoneNumber);
                            Console.WriteLine("Контакт успішно додано!");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                        }
                        break;

                    case "2":
                        Console.Write("Введіть ID контакту для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            try
                            {
                                service.RemoveContact(removeId);
                                Console.WriteLine("Контакт успішно видалено!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неправильний формат ID.");
                        }
                        break;

                    case "3":
                        Console.Write("Введіть ID контакту для зміни імені: ");
                        if (int.TryParse(Console.ReadLine(), out int editNameId))
                        {
                            Console.Write("Введіть нове ім'я: ");
                            string newName = Console.ReadLine();
                            try
                            {
                                service.EditContactName(editNameId, newName);
                                Console.WriteLine("Ім'я контакту успішно змінено!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неправильний формат ID.");
                        }
                        break;

                    case "4":
                        Console.Write("Введіть ID контакту для зміни номера телефону: ");
                        if (int.TryParse(Console.ReadLine(), out int editPhoneId))
                        {
                            Console.Write("Введіть новий номер телефону: ");
                            string newPhoneNumber = Console.ReadLine();
                            try
                            {
                                service.EditContactPhoneNumber(editPhoneId, newPhoneNumber);
                                Console.WriteLine("Номер телефону успішно змінено!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неправильний формат ID.");
                        }
                        break;

                    case "5":
                        Console.WriteLine("\nСписок всіх контактів:");
                        service.PrintAllContacts();
                        break;

                    case "6":
                        Console.Write("Введіть ID контакту для перегляду: ");
                        if (int.TryParse(Console.ReadLine(), out int getId))
                        {
                            try
                            {
                                var contact = service.GetContact(getId);
                                Console.WriteLine($"ID: {getId}, Ім'я: {contact.Name}, Телефон: {contact.PhoneNumber}");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неправильний формат ID.");
                        }
                        break;

                    case "7":
                        isRunning = false;
                        Console.WriteLine("Програма завершена.");
                        break;

                    default:
                        Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }
    }

}
