using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfContactManager
{
    public class Group
    {
        public Group(int id = -1, string name = "")
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public Group Group { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ContactDatabase
    {
        public ContactDatabase()
        {
            Contacts = new List<Contact>();
            Groups = new List<Group>();
        }

        public IList<Contact> Contacts { get; set; }
        public IList<Group> Groups { get; set; }

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
            return Contacts.Where(c => c.Id == id).FirstOrDefault();
        }

        public Group GetGroupById(int id)
        {
            return Groups.Where(g => g.Id == id).FirstOrDefault();
        }
    }
}
