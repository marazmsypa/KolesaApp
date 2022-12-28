using KolesaApp.Models;
using Microsoft.Win32;
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

namespace KolesaApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        Core db = new Core();
        string ImagePath;
        bool isPictureDownload = false;
        public AddProductPage()
        {
            InitializeComponent();
            List<ProductType> list = db.context.ProductType.ToList();
            List<string> listTypes = new List<string> { };
            foreach (var item in list)
            {
                listTypes.Add(item.Title);
            }
            PtoductTypeComboBox.ItemsSource = listTypes;
        }

        private void DownloadPictureButton_Click(object sender, RoutedEventArgs e)
        {
            isPictureDownload = true;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
                ImagePath = dialog.FileName;
            ProductImage.Source = new BitmapImage(new Uri(dialog.FileName));
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(dialog.FileName));
        }

        private void AddProudctButton_Click(object sender, RoutedEventArgs e)
        {
            string newtext = "";
            foreach (char item in ProductNameInput.Text)
            {
                if (item == ' ')
                {
                    newtext += "_";
                }
                else
                {
                    newtext += item.ToString();
                }
            }

            if (isPictureDownload)
            {
                ImagePath = "../../Assets/Images/products/" + newtext + ".jpg";
                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(ProductImage.Source as BitmapSource));
                using (FileStream fileStream = new FileStream(ImagePath, FileMode.Create))
                    jpegBitmapEncoder.Save(fileStream);
                ImagePath = "/products/" + newtext + ".jpg";
            }
            else
            {
                ImagePath = null;
            }
            Product newProduct = new Product()
            {

                Title = ProductNameInput.Text,
                ArticleNumber = ArticleInput.Text,
                MinCostForAgent = Convert.ToDecimal(CostInput.Text),
                ProductTypeID = PtoductTypeComboBox.SelectedIndex + 1,
                ProductionPersonCount = Convert.ToInt32(CountInput.Text),
                ProductionWorkshopNumber = Convert.ToInt32(WorkshopNumberInput.Text),
                Description = DescriptionTextBox.Text,
                Image = ImagePath
            };
            db.context.Product.Add(newProduct);
            db.context.SaveChanges();
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Успешно добавлено");
            }
        }

    }
}
