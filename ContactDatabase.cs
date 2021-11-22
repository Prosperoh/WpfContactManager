using System;
using System.Linq;

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
        public Contact(int id, string name, string company, Group? group, string phoneNumber)
        {
            Id = id;
            Name = name;
            Company = company;
            Group = group;
            PhoneNumber = phoneNumber;
        }

        public int Id { get; set; }
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

        public Contact GetContactById(int id)
        {
            return Array.Find(Contacts, c => c.Id == id);
        }

        public Group GetGroupById(int id)
        {
            return Array.Find(Groups, g => g.Id == id);
        }
    }
}
