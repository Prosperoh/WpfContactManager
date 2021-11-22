using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfContactManager
{
    public struct Group
    {
        public Group(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
    public struct Contact
    {
        public Contact(string name, string company, Group? group, string phoneNumber)
        {
            Name = name;
            Company = company;
            Group = group;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; set; }
        public string Company { get; set; }
        public Group? Group { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ContactDatabase
    {
        public ContactDatabase()
        {
            Contacts = new Contact[0];
            Groups = new Group[0];
        }

        public Contact[] Contacts { get; set; }
        public Group[] Groups { get; set; }
        
        public Contact[] GetContactsInGroup(Group? group)
        {
            Contact[] contactsInGroup = Contacts
                .Where(c => c.Group.Equals(group))
                .ToArray();
            
            Array.Sort(contactsInGroup, (c1, c2) => c1.Name.CompareTo(c2.Name));
            return contactsInGroup;
        }
    }
}
