using Home.models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Home.Utils
{
    public class MasterDataManager
    {
        private static MasterDataManager instance;
        private static masterdataEntities Master_Data_DB;

        private MasterDataManager() { }

        public static MasterDataManager getInstance()
        {
            if (instance == null)
            {
                instance = new MasterDataManager();
                Master_Data_DB = new masterdataEntities();
            }

            return instance;
        }

        /// <summary>
        /// lấy id, name icon của category
        /// </summary>
        /// <returns></returns>
        public List<Category> getAllCategory()
        {

            return Master_Data_DB.Categories.Where(categories =>

                categories.Status == 0

            ).ToList();
        }

        /// <summary>
        /// add category
        /// </summary>
        /// <param name="newCategory"></param>
        /// <returns></returns>
        public bool addNewCategory(Category newCategory)
        {
            var categoryInDB = Master_Data_DB.Categories.Where(category =>
                category.Name.Equals(newCategory.Name)
            ).ToList();

            if (categoryInDB.Count() == 0)
            {
                Master_Data_DB.Categories.Add(newCategory);
                Master_Data_DB.SaveChanges();
                return true;
            }
            else if (categoryInDB[0].Status == -1)
            {
                categoryInDB[0].Status = 0;
                categoryInDB[0].Icon = newCategory.Icon;
                Master_Data_DB.SaveChanges();
                return true;
            }
            //var oldCate = Master_Data_DB.Categories.Where(category =>

            //    category.Name.Equals(newCategory.Name) && category.Status == 0
            //);

            //if (oldCate.Count() == 0)
            //{
            //    Master_Data_DB.Categories.Add(newCategory);
            //    Master_Data_DB.SaveChanges();

            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Xóa category nếu tồn tại và không còn chứa cosmetic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteCategory(int id)
        {
            var cosOf = Master_Data_DB.Cosmetics.Where(cosmetic =>

                cosmetic.Category == id && cosmetic.Status == 0
            );

            var cate = Master_Data_DB.Categories.Find(id);

            if (cate != null && cosOf.Count() == 0)
            {
                cate.Status = -1;
                Master_Data_DB.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// cập nhật category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool updateCategory(Category oldCategory, Category NewCategory)
        {
            var cate = Master_Data_DB.Categories.Find(oldCategory.ID);

            var checkExist = Master_Data_DB.Categories.Where(cte =>

                cte.Name == NewCategory.Name && cte.ID != oldCategory.ID

            ).ToList();

            if (cate != null && checkExist.Count() == 0)
            {
                cate.Name = NewCategory.Name;
                cate.Icon = NewCategory.Icon;
                Master_Data_DB.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all cosmetic 
        /// </summary>
        /// <returns></returns>
        public List<Cosmetic> getAllCosmetic()
        {
            return Master_Data_DB.Cosmetics.ToList();
        }



        /// <summary>
        /// lấy ra cosmetic theo từng category
        /// </summary>
        /// <param name="idCategory"></param>
        /// <returns></returns>
        public List<Cosmetic> getAllCosmeticOfCategory(int idCategory)
        {
            return Master_Data_DB.Cosmetics.Where(cosmetic =>

                cosmetic.Category == idCategory && cosmetic.Status == 0

            ).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cosmetic"></param>
        /// <returns></returns>
        public bool addNewCosmetic(Cosmetic cosmetic)
        {
            var checkExists = Master_Data_DB.Cosmetics.Where(cos =>

                cos.Name == cosmetic.Name && cos.Category == cosmetic.Category

            ).ToList();


            if (checkExists.Count() == 0)
            {
                if (Master_Data_DB.Cosmetics.Add(cosmetic) != null)
                {
                    Master_Data_DB.SaveChanges();
                    return true;
                }
            }
            // if exists but deleted, create new product too 
            else if (checkExists[0].Status == -1)
            {
                if (Master_Data_DB.Cosmetics.Add(cosmetic) != null)
                {
                    Master_Data_DB.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteCosmetic(int id)
        {
            var cos = Master_Data_DB.Cosmetics.Find(id);

            if (cos != null)
            {
                cos.Status = -1;
                Master_Data_DB.SaveChanges();

                return true;
            }

            return false;
        }


        public bool updateCosmetic(Cosmetic cosmetic)
        {
            var cos = Master_Data_DB.Cosmetics.Find(cosmetic.ID);

            var checkExists = Master_Data_DB.Cosmetics.Where(cosm =>

                cosm.Name == cosmetic.Name && cosm.Category == cosmetic.Category && cosm.ID != cosmetic.ID

            ).ToList();

            if (cos != null && checkExists.Count() == 0)
            {
                cos.Name = cosmetic.Name;
                cos.Image = cosmetic.Image;
                cos.Category = cosmetic.Category;
                cos.Price = cosmetic.Price;
                cos.Detail = cosmetic.Detail;
                cos.Origin = cosmetic.Origin;
                Master_Data_DB.SaveChanges();

                return true;
            }

            return false;
        }

        public Cosmetic getCosmeticById(int id)
        {
            var cos = Master_Data_DB.Cosmetics.Find(id);

            return cos != null ? cos : null;
        }

        public bool addNewOrder(Order order)
        {
            var ord = Master_Data_DB.Orders.Add(order);
            if (ord != null)
            {
                Master_Data_DB.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// load tất cả các đơn hàng
        /// </summary>
        /// <returns>
        ///     trả về danh sách các đơn hàng, bao gồm chi tiết các sản phẩm trong đơn hàng đó
        /// </returns>
        public List<Order> LoadAllOrder()
        {
            var jsonManager = new JavaScriptSerializer();
            var listOrder = Master_Data_DB.Orders.Where(odr => odr.Status != Order.C_DELETED).OrderByDescending(odr => odr.CreateAt).ToList();
            for (int i = 0; i < listOrder.Count; i++)
            {
                listOrder[i].ListProducts = jsonManager.Deserialize<List<ProductOfOrder>>(listOrder[i].Products);
            }

            return listOrder;
        }

        public Order getOrderById(int id)
        {
            var odr = Master_Data_DB.Orders.Find(id);
            if(odr != null)
            {
                var jsonManager = new JavaScriptSerializer();
                odr.ListProducts = jsonManager.Deserialize<List<ProductOfOrder>>(odr.Products);

                return odr;
            }

            return null;
        }

        /// <summary>
        /// cập nhật trạng thái đơn hàng
        /// </summary>
        public bool updateOrder(int id, int newStatus)
        {
            var odr = getOrderById(id);

            if (odr != null)
            {
                odr.Status = newStatus;
                Master_Data_DB.SaveChanges();

                return true;
            }

            return false;
        }

        public bool deleteOrder(int id)
        {
            var ord = Master_Data_DB.Orders.Find(id);
            if (ord != null)
            {
                ord.Status = Order.C_DELETED;
                Master_Data_DB.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Cosmetic> searchCosmeticsByName(string name)
        {
            return Master_Data_DB.Cosmetics.Where(cos =>
                   cos.Name.ToLower().Contains(name.ToLower())
            ).ToList();
        }

        public List<Order> searchOrdersByPhoneOrName(string key)
        {
            return Master_Data_DB.Orders.Where(odr =>
                   odr.CustomerTel.ToLower().Contains(key.ToLower()) || odr.CustomerName.ToLower().Contains(key.ToLower())
            ).OrderByDescending(odr => odr.CreateAt).ToList();
        }

        public int getNumberOfProductSoldByDate(DateTime date)
        {
            var listOrder = Master_Data_DB.Orders.Where(ord =>
                DbFunctions.TruncateTime(ord.CreateAt) == DbFunctions.TruncateTime(date) && ord.Status == Order.C_COMPLETED
            ).ToList();

            var jsonManager = new JavaScriptSerializer();
            int count = 0;
            foreach(var odr in listOrder)
            {
                var listProduct = jsonManager.Deserialize<List<ProductOfOrder>>(odr.Products);
                foreach (var pro in listProduct)
                {
                    count += pro.Quantity;
                }
            }

            return count;
        }

        public List<StatisticalByDate> getStatisticalByDate(DateTime from, DateTime to)
        {
            var data = new List<StatisticalByDate>();

            Debug.WriteLine(from.ToString("d"));
            while ( from <= to  )
            {
                var st = new StatisticalByDate
                {
                    Date = from,
                    Quantity = getNumberOfProductSoldByDate(from)
                };
                data.Add(st);
                from = from.AddDays(1);
                Debug.WriteLine(from.ToString("d"));
            }

            return data;
        }


        public int getRevenueByMonth(DateTime date)
        {
            var listOrder = Master_Data_DB.Orders.Where(ord =>
                ord.CreateAt.Month == date.Month 
                && ord.CreateAt.Year == date.Year
                && ord.Status == Order.C_COMPLETED
            ).ToList();

            int revenue = 0;
            foreach(var ord in listOrder)
            {
                revenue += ord.TotalPrice;
            }

            return revenue;
        }

        public List<StaticalByMonth> getStatisticalByMonth(DateTime from, DateTime to)
        {
            var data = new List<StaticalByMonth>();
            
            while (from <= to)
            {
                var st = new StaticalByMonth
                {
                    Month = from.Month,
                    Year = from.Year,
                    Revenue = getRevenueByMonth(from)
                };
                data.Add(st);
                from = from.AddMonths(1);
            }

            return data;
        }
    
        private int checkProductExistsInListProducts(ProductOfOrder product, List<ProductOfOrder> listProducts)
        {

            for(int i = 0; i < listProducts.Count; i++)
            {
                if(listProducts[i].ID == product.ID)
                {
                    return i;
                }
            }

            return -1;
        }

        public List<StatisticalProductsContributeByDate> getStatisticalProductsContributeByDate(DateTime fromDate, DateTime toDate)
        {
            var listOrder = Master_Data_DB.Orders.Where(ord =>
                    DbFunctions.TruncateTime(ord.CreateAt) >= DbFunctions.TruncateTime(fromDate) 
                &&  DbFunctions.TruncateTime(ord.CreateAt) <= DbFunctions.TruncateTime(toDate) 
                &&  ord.Status == Order.C_COMPLETED
            ).ToList();

            var jsonManager = new JavaScriptSerializer();

            int revenue = 0;
            var listProducts = new List<ProductOfOrder>();

            foreach(var ord in listOrder)
            {
                revenue += ord.TotalPrice;
                var products = jsonManager.Deserialize<List<ProductOfOrder>>(ord.Products);
                foreach(var pro in products)
                {
                    int index = checkProductExistsInListProducts(pro, listProducts);
                    if(index != -1)
                    {
                        listProducts[index].Quantity += pro.Quantity;
                    }
                    else
                    {
                        listProducts.Add(pro);
                    }
                }
            }

            var result = new List<StatisticalProductsContributeByDate>();
            foreach(var pro in listProducts)
            {
                result.Add(new StatisticalProductsContributeByDate {
                    Name = pro.Name,
                    Contribute = (float)(Math.Round((decimal)(pro.Total*1.0 / revenue), 2)*100)
                });
            }
           
            return result;
        }
        
        public void saveChanged()
        {
            Master_Data_DB.SaveChanges();
        }

    }
}
