using Home.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Utils
{
    public class MasterDataManager
    {
        private static MasterDataManager instance;
        private static master_dataEntities Master_Data_DB;

        private MasterDataManager() { }

        public static MasterDataManager getInstance()
        {
            if (instance == null)
            {
                instance = new MasterDataManager();
                Master_Data_DB = new master_dataEntities();
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

                cte.Name == NewCategory.Name && cte.ID != NewCategory.ID

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

        public void saveChanged()
        {
            Master_Data_DB.SaveChanges();
        }

    }
}
