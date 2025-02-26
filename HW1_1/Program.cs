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

        void AddContact(string name, string phoneNumber)
        {
            if (!Contact.IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException($"Invalid phone number format: {phoneNumber}");
            contacts.Add(new Contact(name, phoneNumber));
        }

        void RemoveContact(int id)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            contacts.RemoveAt(id);
        }

        void EditContactName(int id, string newName)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            var contact = contacts[id];
            contact.Name = newName;
            contacts[id] = contact;
        }

        void EditContactPhoneNumber(int id, string newPhoneNumber)
        {
            if (id < 0 || id >= contacts.Count)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid id");
            var contact = contacts[id];
            contact.PhoneNumber = newPhoneNumber;
            contacts[id] = contact;
        }

        void PrintAllContacts()
        {
            for (int i = 0; i < contacts.Count; i++)
                Console.WriteLine($"ID: {i}, Name: {contacts[i].Name}, Phone number: {contacts[i].PhoneNumber}");
        }

        Contact GetContact(int id)
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

        }
    }
}
