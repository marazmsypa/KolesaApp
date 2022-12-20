using KolesaApp.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KolesaApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        int page = 1;
        Core db = new Core();
        int countElement = 10;
        List<ProductType> productTypes;
        bool reverseType;
        public ProductPage()
        {          
            InitializeComponent();
            //заполнение списка для фильтрации типами продуктов
            List<string> sortTypeList = new List<string>()
            {
                "Наименование","Остаток на складе","Стоимость"
            };
            SortComboBox.ItemsSource = sortTypeList;
            productTypes = new List<ProductType>
            {
                new ProductType()
                {
                    ID = 0,
                    Title = "Все типы"
                }
            };
            productTypes.AddRange(db.context.ProductType.ToList());
            FilterComboBox.ItemsSource = productTypes;
            UpdateUI();
        }
 
       
        /// <summary>
        /// Формирование данные для вывода 
        /// </summary>
        /// <returns>
        /// Возвращает все данные из таблицы
        /// </returns>
        private List<Product> GetRows()
        {
            List<Product> arrayProduct = db.context.Product.ToList();
            string searchData = SearchTextBox.Text.ToUpper();
            if (!String.IsNullOrEmpty(SearchTextBox.Text) && SearchTextBox.Text != SearchTextBox.Placeholder)
            {
                arrayProduct = arrayProduct.Where(x=> x.Title.ToUpper().Split().Contains(searchData)).ToList();
            }
            //фильтрация
            
            if (FilterComboBox.SelectedIndex == 0)
            {
                arrayProduct = arrayProduct.ToList();
            }
            else
            {
                int filter = Convert.ToInt32(FilterComboBox.SelectedValue) ;
                arrayProduct = arrayProduct.Where(x => x.ProductTypeID == filter).ToList();
            }
            //сортировка
            string value = SortComboBox.SelectedValue.ToString();
            if (value == "Наименование")
            {
                arrayProduct = arrayProduct.OrderBy(x => x.Title).ToList();
            }
            if (value == "Остаток на складе")
            {
                arrayProduct = arrayProduct.OrderBy(x => x.ProductionWorkshopNumber).ToList();
            }
            if (value == "Стоимость")
            {
                arrayProduct = arrayProduct.OrderBy(x => x.CostProduct).ToList();
            }
            //Изменение порядка сортировки
            if (reverseType)
            {
                arrayProduct.Reverse();
            }
            return arrayProduct;
        }
        private int GetPagesCount()
        {
            int countPage = 0;
            
            int count = GetRows().Count;
            if (count > countElement)
            {
                countPage = Convert.ToInt32(Math.Ceiling(count * 1.0 / countElement));
            }
            return countPage;
        }
        public void DisplayPagination(int page)
        {
            List<PageItem> source = new List<PageItem>();
            for (int i = 1; i <= GetPagesCount(); i++)
            {
                source.Add(new PageItem(i, i == page));
            }
            PaginationListView.ItemsSource = source;
            PrevTextBlock.Visibility = (page <= 1 ? Visibility.Hidden : Visibility.Visible);
            NextTextBlock.Visibility = (page >= GetPagesCount() ? Visibility.Hidden : Visibility.Visible);
        }
        private void NextTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page >= GetPagesCount())
            {
                page = GetPagesCount();
                NextTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                page += 1;
                NextTextBlock.Visibility = Visibility.Visible;
            }
            UpdateUI();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            page = Convert.ToInt32(textBlock.Text);
            UpdateUI();
        }

        private void PrevTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page <= 1)
            {
                page = 1;
                PrevTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                page -= 1;
                PrevTextBlock.Visibility = Visibility.Visible;

            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            CountRecordsTextBlock.Text = GetRows().Count().ToString() + " / " + db.context.Product.ToList().Count().ToString();
            DisplayPagination(page);
            List<Product> displayProduct = GetRows().Skip((page - 1) * countElement).Take(countElement).ToList();

            ProductsListView.ItemsSource = displayProduct;
            PaginationListView.Visibility = Visibility.Visible;
            if (GetRows().Count < 10)
            {
                PaginationListView.Visibility = Visibility.Collapsed;
                
                
            }
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            page = 1;   
            UpdateUI();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            page = 1;
            UpdateUI();
        }

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            reverseType = !reverseType;
            UpdateUI();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            UpdateUI();
        }
    }
}
