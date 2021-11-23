using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;

namespace WpfContactManager
{
    public class TreeViewGroup
    {
        public TreeViewGroup(Group g, ContactDatabase cd)
        {
            _g = g;
            _cd = cd;
        }

        public IList<Contact> Contacts => _cd.GetContactsInGroup(_g);
        public string Name => _g.Name;
        public Group Group => _g;

        private readonly Group _g;
        private readonly ContactDatabase _cd;
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            ContactDB = new ContactDatabase();
            ContactDB.Groups.Add(new Group(0, "New group")); // for testing
            SelectedTreeViewItem = null;
        }

        public ContactDatabase ContactDB
        {
            get => _cdb;
            set
            {
                _cdb = value;
                OnPropertyChanged("ContactDB");
                OnPropertyChanged("TreeViewGroups");
            }
        }
        private ContactDatabase _cdb;

        public IList<TreeViewGroup> TreeViewGroups => ContactDB.Groups.Select(g => new TreeViewGroup(g, ContactDB)).ToList();

        public TreeViewItem SelectedTreeViewItem
        {
            get => _stvi;
            set
            {
                _stvi = value;
                OnPropertyChanged("SelectedTreeViewItem");

                OnPropertyChanged("GroupEditPanelVisible");
                OnPropertyChanged("ContactEditPanelVisible");

                OnPropertyChanged("GroupNameFieldValue");
                OnPropertyChanged("ContactNameFieldValue");
                OnPropertyChanged("ContactCompanyFieldValue");
                OnPropertyChanged("ContactPhoneNumberFieldValue");
            }
        }
        private TreeViewItem _stvi;
        
        public Visibility GroupEditPanelVisible => IsGroupSelected(SelectedTreeViewItem) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ContactEditPanelVisible => IsContactSelected(SelectedTreeViewItem) ? Visibility.Visible : Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsGroupSelected(TreeViewItem tvi)
        {
            return GetSelectedGroup(tvi) != null;
        }

        private bool IsContactSelected(TreeViewItem tvi)
        {
            return GetSelectedContact(tvi) != null;
        }
    
        private Group GetSelectedGroup(TreeViewItem tvi)
        {
            if (tvi == null)
            {
                return null;
            }

            if (!(tvi.DataContext is TreeViewGroup tvg))
            {
                return null;
            }

            return tvg.Group;
        }

        private Contact GetSelectedContact(TreeViewItem tvi)
        {
            if (tvi == null)
            {
                return null;
            }

            Contact tvc = tvi.DataContext as Contact;
            return tvc;
        }

        public string GroupNameFieldValue
        {
            get
            {
                Group g = GetSelectedGroup(SelectedTreeViewItem);
                return g != null ? g.Name : "";
            }
            set
            {
                Group g = GetSelectedGroup(SelectedTreeViewItem);
                if (g != null)
                {
                    g.Name = value;
                    OnPropertyChanged("GroupNameFieldValue");
                    OnPropertyChanged("TreeViewGroups");
                }
            }
        }

        public string ContactNameFieldValue
        {
            get
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                return c != null ? c.Name : "";
            }
            set
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                if (c != null)
                {
                    c.Name = value;
                    OnPropertyChanged("ContactNameFieldValue");
                    OnPropertyChanged("TreeViewGroups");
                }
            }
        }

        public string ContactCompanyFieldValue
        {
            get
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                return c != null ? c.Company : "";
            }
            set
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                if (c != null)
                {
                    c.Company = value;
                    OnPropertyChanged("ContactCompanyFieldValue");
                }
            }
        }

        public string ContactPhoneNumberFieldValue
        {
            get
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                return c != null ? c.PhoneNumber : "";
            }
            set
            {
                Contact c = GetSelectedContact(SelectedTreeViewItem);
                if (c != null)
                {
                    c.PhoneNumber = value;
                    OnPropertyChanged("ContactPhoneNumberFieldValue");
                }
            }
        }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ViewModel();
            DataContext = _viewModel;
        }

        private void TreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedTreeViewItem = e.OriginalSource as TreeViewItem;
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
                    _viewModel.ContactDB = cd;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ContactDatabaseIO.SaveToFile(dialog.FileName, _viewModel.ContactDB);
            }
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
