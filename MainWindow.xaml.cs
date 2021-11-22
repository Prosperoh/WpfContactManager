using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfContactManager
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ContactDB = new ContactDatabase();

            InitializeComponent();

            RebuildTreeView();
        }

        private void AddContactsFromGroupToTree(Group? group, TreeViewItem groupTreeViewItem)
        {
            Contact[] contactsInGroup = ContactDB.GetContactsInGroup(group);
            foreach (Contact contact in contactsInGroup)
            {
                TreeViewItem contactTreeViewItem = new TreeViewItem
                {
                    Header = contact.Name
                };
                _ = groupTreeViewItem.Items.Add(contactTreeViewItem);
            }
        }

        private void RebuildTreeView()
        {
            contactsTreeView.Items.Clear();

            foreach (Group group in ContactDB.Groups)
            {
                TreeViewItem groupTreeViewItem = new TreeViewItem
                {
                    Header = group.Name
                };
                _ = contactsTreeView.Items.Add(groupTreeViewItem);

                AddContactsFromGroupToTree(group, groupTreeViewItem);
            }

            TreeViewItem noGroupItem = new TreeViewItem
            {
                Header = "<No group>"
            };

            AddContactsFromGroupToTree(null, noGroupItem);
        }

        private ContactDatabase ContactDB { get; set; }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ContactDatabaseIO.ReadStatus result = ContactDatabaseIO.LoadFromFile(dialog.FileName, out ContactDatabase cd);
                if (result != ContactDatabaseIO.ReadStatus.Success)
                {
                    _ = MessageBox.Show("Failed to parse contact database!");
                }
                else
                {
                    ContactDB = cd;
                    RebuildTreeView();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ContactDatabaseIO.SaveToFile(dialog.FileName, ContactDB);
            }
        }
    }
}
