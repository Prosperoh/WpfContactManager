using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfContactManager
{
    public class ContactDatabaseIO
    {
        public enum ReadStatus
        {
            Success,
            Error
        }

        private static ReadStatus LoadContacts(StreamReader reader, Group[] groups, out Contact[] contacts)
        {
            string line;
            line = reader.ReadLine();
            contacts = null;

            if (line == null)
            {
                return ReadStatus.Error;
            }

            int nContacts;

            try
            {
                nContacts = int.Parse(line);
            }
            catch
            {
                return ReadStatus.Error; // could not parse int
            }

            contacts = new Contact[nContacts];

            for (int i = 0; i < nContacts; i++)
            {
                line = reader.ReadLine();
                if (!TryParseContact(line, groups, out contacts[i]))
                {
                    return ReadStatus.Error;
                }
            }

            return ReadStatus.Success;
        }

        private static bool TryParseContact(string line, Group[] groups, out Contact contact)
        {
            contact = new Contact();
            string[] elements = line.Split(';');
            try
            {
                contact.Name = elements[0];
                contact.Company = elements[1];
                contact.PhoneNumber = elements[2];

                if (string.IsNullOrWhiteSpace(elements[3]))
                {
                    contact.Group = null;
                }
                else
                {
                    int id = int.Parse(elements[3]);
                    contact.Group = GetGroupById(groups, id);
                }
            }
            catch
            {
                return false; // parsing error
            }
            return true;
        }

        private static ReadStatus LoadGroups(StreamReader reader, out Group[] groups)
        {
            string line;
            line = reader.ReadLine();
            groups = null;

            if (line == null)
            {
                return ReadStatus.Error;
            }

            int nGroups;

            try
            {
                nGroups = int.Parse(line);
            }
            catch
            {
                return ReadStatus.Error; // could not parse int
            }

            groups = new Group[nGroups];

            for (int i = 0; i < nGroups; i++)
            {
                line = reader.ReadLine();
                if (!TryParseGroup(line, out groups[i]))
                {
                    return ReadStatus.Error;
                }
                groups[i].Id = i;
            }

            return ReadStatus.Success;
        }

        private static bool TryParseGroup(string line, out Group group)
        {
            group = new Group();
            string[] elements = line.Split(';');
            try
            {
                group.Name = elements[0];
            }
            catch
            {
                return false; // parsing error
            }
            return true;
        }

        private static Group GetGroupById(Group[] groups, int id)
        {
            return Array.Find(groups, g => { return g.Id == id; });
        }

        public static ReadStatus LoadFromFile(string filename, out ContactDatabase cd)
        {
            cd = null;

            using FileStream fs = File.OpenRead(filename);
            using StreamReader reader = new StreamReader(fs);

            ReadStatus result;
            result = LoadGroups(reader, out Group[] groups);
            if (result != ReadStatus.Success)
            {
                return result;
            }

            result = LoadContacts(reader, groups, out Contact[] contacts);
            if (result != ReadStatus.Success)
            {
                return result;
            }

            cd = new ContactDatabase
            {
                Groups = groups,
                Contacts = contacts
            };

            return ReadStatus.Success;
        }

        private static void WriteGroup(StreamWriter writer, Group g)
        {
            writer.WriteLine(g.Name);
        }

        private static void WriteContact(StreamWriter writer, Contact c)
        {
            string groupId = c.Group.HasValue ? c.Group.Value.Id.ToString() : "";
            writer.WriteLine($@"{c.Name};{c.Company};{c.PhoneNumber};{groupId}");
        }

        private static void SaveGroups(StreamWriter writer, ContactDatabase cd)
        {
            writer.WriteLine(cd.Groups.Length);
            foreach (Group g in cd.Groups)
            {
                WriteGroup(writer, g);
            }

        }

        private static void SaveContacts(StreamWriter writer, ContactDatabase cd)
        {
            writer.WriteLine(cd.Contacts.Length);
            foreach (Contact c in cd.Contacts)
            {
                WriteContact(writer, c);
            }
        }

        public static void SaveToFile(string filename, ContactDatabase cd)
        {
            using FileStream fs = File.OpenWrite(filename);
            using StreamWriter writer = new StreamWriter(fs);

            SaveGroups(writer, cd);
            SaveContacts(writer, cd);
        }
    }
}
