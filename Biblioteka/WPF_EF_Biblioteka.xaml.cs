using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Biblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_EF_Biblioteka.xaml
    /// </summary>
    public partial class WPF_EF_Biblioteka : Window
    {
        public WPF_EF_Biblioteka()
        {
            InitializeComponent();

            BibliotekaDBEntities db = new BibliotekaDBEntities();
            var books = from d in db.Ksiazkis
                        select new
                        {
                            Nazwa = d.Nazwa,
                            Autor = d.Autor,
                            Tematyka= d.Tematyka

                        };
            foreach (var item in books)
            {
                Console.WriteLine(item.Nazwa);
                Console.WriteLine(item.Autor);
                Console.WriteLine(item.Tematyka);
            }

            this.gridKsiazki.ItemsSource = books.ToList();
        }

        private int updatingKsiazkaID = 0;
        private void gridKsiazki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridKsiazki.SelectedIndex >= 0)
            {
                if (this.gridKsiazki.SelectedItems.Count >= 0)
                {
                    if (this.gridKsiazki.SelectedItems[0].GetType() == typeof(Ksiazki))
                    {
                        Ksiazki d = (Ksiazki)this.gridKsiazki.SelectedItems[0];
                        this.txtnazwa2.Text = d.Nazwa;
                        this.txtautor2.Text = d.Autor;
                        this.txttematyka2.Text = d.Tematyka;
                        this.updatingKsiazkaID = d.Id;
                    }
                }
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            BibliotekaDBEntities db = new BibliotekaDBEntities();

            Ksiazki NazwaObject = new Ksiazki()
            {
                Nazwa = txtnazwa.Text,
                Autor = txtautor.Text,
                Tematyka = txttematyka.Text
            };

            db.Ksiazkis.Add(NazwaObject);
            db.SaveChanges();
        }

        private void btnloadKsiazkis_Click(object sender, RoutedEventArgs e)
        {
            BibliotekaDBEntities db = new BibliotekaDBEntities();

            this.gridKsiazki.ItemsSource = db.Ksiazkis.ToList();
        }

        private void btnupdateKsiazkis_Click(object sender, RoutedEventArgs e)
        {
            BibliotekaDBEntities db = new BibliotekaDBEntities();

            var r = from d in db.Ksiazkis
                    where d.Id == this.updatingKsiazkaID
                    select d;

            Ksiazki obj = r.SingleOrDefault();

            if(obj != null)
            {
                obj.Nazwa = this.txtnazwa2.Text;
                obj.Autor = this.txtautor2.Text;
                obj.Tematyka = this.txttematyka2.Text;

                db.SaveChanges();
            }


            

        }

        private void btndeleteKsiazkis_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult msgBoxResult = MessageBox.Show("Jesteś pewien, że chcesz usunąć?",
                "Usuń Książkę",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
                );

            if (msgBoxResult == MessageBoxResult.Yes)
            {

                BibliotekaDBEntities db = new BibliotekaDBEntities();

                var r = from d in db.Ksiazkis
                        where d.Id == this.updatingKsiazkaID
                        select d;

                Ksiazki obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.Ksiazkis.Remove(obj);
                    db.SaveChanges();
                }
            }
        }
    }
}
