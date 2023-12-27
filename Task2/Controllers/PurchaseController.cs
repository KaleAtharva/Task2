using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    
    public class PurchaseController : Controller
    {
        PurchaseDataAccessLayer objStore = new PurchaseDataAccessLayer();

        [Authorize(Roles = "Admin,Store Manager,Product Manager,Purchase")]
        public IActionResult Index()
        {
            List<Purchase> lstItems = new List<Purchase>();
            lstItems = objStore.GetAllOrders().ToList();
            return View(lstItems);
        }
        [Authorize(Roles = "Purchase,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Purchase,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string itemID,string item)
        {
            int id = Convert.ToInt32(itemID);
            if(ModelState.IsValid)
            {
                if (!objStore.InvalidItemID(id, item)){
                    ViewBag.Message = String.Format("Item and the ID doesnt match");
                    return View();
                }
                objStore.AddtoStore(item);
                objStore.DeletePurchaseOrder(id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
