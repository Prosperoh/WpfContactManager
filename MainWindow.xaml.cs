using System.Windows;
using System.Windows.Controls;
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
            SelectedTreeViewItem = null;

            InitializeComponent();

            RebuildTreeView();
        }

        private ContactDatabase ContactDB { get; set; }
        private TreeViewItem? SelectedTreeViewItem { get; set; }

        private void AddContactsFromGroupToTree(Group? group, TreeViewItem groupTreeViewItem)
        {
            Contact[] contactsInGroup = ContactDB.GetContactsInGroup(group);
            foreach (Contact contact in contactsInGroup)
            {
                TreeViewItem contactTreeViewItem = new TreeViewItem
                {
                    Header = contact.Name,
                    Tag = contact.Id
                };
                contactTreeViewItem.Selected += treeViewItem_contactSelected;
                _ = groupTreeViewItem.Items.Add(contactTreeViewItem);
            }
        }

        private void UpdateGroupEditPanel(TreeViewItem groupTreeViewItem)
        {
            int groupId = (int)groupTreeViewItem.Tag;
            Group group = ContactDB.GetGroupById(groupId);
            groupNameField.Text = group.Name;
        }

        private void UpdateContactEditPanel(TreeViewItem contactTreeViewItem)
        {
            int contactId = (int)contactTreeViewItem.Tag;
            Contact contact = ContactDB.GetContactById(contactId);
            contactNameField.Text = contact.Name;
            contactCompanyField.Text = contact.Company;
            contactPhoneNumberField.Text = contact.PhoneNumber;
        }

        private void treeViewItem_groupSelected(object sender, RoutedEventArgs e)
        {
            contactEditPanel.Visibility = Visibility.Collapsed;
            groupEditPanel.Visibility = Visibility.Visible;

            SelectedTreeViewItem = (TreeViewItem)e.Source;
            UpdateGroupEditPanel(SelectedTreeViewItem);
        }

        private void treeViewItem_contactSelected(object sender, RoutedEventArgs e)
        {
            groupEditPanel.Visibility = Visibility.Collapsed;
            contactEditPanel.Visibility = Visibility.Visible;

            SelectedTreeViewItem = (TreeViewItem)e.Source;

            UpdateContactEditPanel(SelectedTreeViewItem);
            e.Handled = true; // to avoid fire the selected event for the parent node
        }

        private void RebuildTreeView()
        {
            contactsTreeView.Items.Clear();

            foreach (Group group in ContactDB.Groups)
            {
                TreeViewItem groupTreeViewItem = new TreeViewItem
                {
                    Header = group.Name,
                    Tag = group.Id
                };
                groupTreeViewItem.Selected += treeViewItem_groupSelected;
                _ = contactsTreeView.Items.Add(groupTreeViewItem);

                AddContactsFromGroupToTree(group, groupTreeViewItem);
            }

            TreeViewItem noGroupItem = new TreeViewItem
            {
                Header = "<No group>"
            };

            AddContactsFromGroupToTree(null, noGroupItem);
        }

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

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteContact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
