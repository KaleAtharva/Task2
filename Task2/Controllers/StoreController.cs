using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    public class StoreController : Controller
    {
        StoreDataAccessLayer objStore = new StoreDataAccessLayer();
        [Authorize(Roles = "Store Manager,Admin,Product Manager,Purchase")]
        public IActionResult Index()
        {
            List<Store> lstItems = new List<Store>();
            lstItems = objStore.GetAllItems().ToList();
            return View(lstItems);
        }
        [Authorize(Roles ="Store Manager,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Store Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string itemID, string item, [Bind] Purchase order)
        {
            int id = Convert.ToInt32(itemID);
            if (ModelState.IsValid)
            {
                if (!objStore.InvalidItemID(id, item))
                {
                    ViewBag.Message = String.Format("Item and the ID doesnt match");
                    return View();
                }
                objStore.AddRequirement(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }
    }
}
