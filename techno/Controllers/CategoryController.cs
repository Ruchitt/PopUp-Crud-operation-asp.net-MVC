using techno.Data;
using techno.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace exam_technomark.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Create(Category obj)
        {
            _db.categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET
		public IActionResult Edit(int id)
		{
            var categoryfromdb = _db.categories.Find(id);
            if (categoryfromdb == null)
            {
                return NotFound();
            }
			return View(categoryfromdb);
		}

        [HttpPost]
		public IActionResult Edit(Category obj)
		{
			_db.categories.Update(obj);
            _db.SaveChanges();
			return RedirectToAction("Index");
		}
		//GET
		public IActionResult AddChild()
		{
			var model = new Category
			{
				Values = _db.categories.Select(x => new SelectListItem
				{
					Value = x.CategoryId.ToString(),
					Text = x.Name
				})
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult AddChild(Category obj)
		{
			_db.categories.Add(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		//GET
		public IActionResult Delete(int id)
		{
			var categoryfromdb = _db.categories.Find(id);
			if (categoryfromdb == null)
			{
				return NotFound();
			}
			return View(categoryfromdb);
		}

		[HttpPost]
		public IActionResult Delete(Category obj)
		{
			var subCategories = _db.categories.Where(x => x.ParentCategoryId == obj.CategoryId);
			_db.categories.RemoveRange(subCategories);
			var category = _db.categories.Find(obj.CategoryId);
			_db.categories.Remove(category);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}









		#region API CALLS
		[HttpGet]
        public IActionResult GetAll()
        {
            var categorylist = _db.categories.ToList();
			var modifiedCategoryList = categorylist.Select(c => new { c.CategoryId, c.Name, c.Description, IsActive = c.IsActive ? "Active" : "InActive" ,c.ParentCategoryId,c.CreatedOn,c.ModifiedOn,c.Values});
			return Json(new {data = modifiedCategoryList });;
        }
        #endregion
    }
}
