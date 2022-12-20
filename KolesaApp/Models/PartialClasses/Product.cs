using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolesaApp.Models
{
    public partial class Product
    {
        Core db = new Core();
        public string ImagePath
        {
            get
            {
                if (Image == null)
                {
                    return "..\\..\\Assets\\Images\\noPicture.png";
                }
                else
                {
                    return "/Assets/Images" + Image;
                }
            }
        }
        public string MaterialList
        {
            get
            {
                string materials = "Материалы: ";
                List<string> arrayMaterials = new List<string> { };
                List<ProductMaterial> arrayList = db.context.ProductMaterial.Where(x => x.ProductID == ID).ToList();
                foreach (var item in arrayList)
                {
                    arrayMaterials.Add(item.Material.Title.ToString());
                }
                materials += String.Join(", ", arrayMaterials);
                if (arrayMaterials.Count == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return materials;
                }
                
            }
        }
        public double CostProduct
        {
            get
            {
                double costProduct = 0;
                List<ProductMaterial> arrayActiveProduct = ProductMaterial.Where(x => x.ProductID == ID).ToList();
                foreach (var item in arrayActiveProduct)
                {
                    costProduct += Convert.ToDouble(item.Count) * Convert.ToDouble(item.Material.Cost);
                }
                return costProduct;
            }

        }
    }
}
