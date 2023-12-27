using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Drawing;
using Task2.Models;

namespace Task2.Controllers
{
    [Authorize(Roles ="Admin,Store Manager,Purchase,Product Manager")]
    public class ProductionController : Controller
    {
        ProductionDataAccessLayer obj = new ProductionDataAccessLayer();
        public IActionResult Index()
        {
            return View();
        }
        

        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(string itemID,string item,bool available,string quantity,Store store)
        {
            bool check = obj.checkItem(item);
            int quant = Convert.ToInt32(quantity);
            int id = Convert.ToInt32(itemID);
            if (ModelState.IsValid)
            {
                string inputItem = obj.ItemName(id);
                ViewData["item"] = inputItem;
                if (obj.InvalidItemID(id) && !check)
                {
                    ViewBag.Message = String.Format("An item with the ID already exists");
                    return View();
                }
                else if (check)
                {
                        int Q = obj.GetItemData(item, quant);
                        if (!obj.CheckAvailability(item))
                        {
                        obj.UpdateQuantity(item, quant+Q);
                    }
                    else if (quant <= Q)
                        {
                            int qua = Q - quant;            
                            obj.UpdateQuantity(item, qua);
                            if(qua==0)
                            obj.ItemUnavailable(item);
                        }
                        else if (Q == 0)
                        {
                            obj.UpdateQuantity(item, quant);
                            obj.ItemUnavailable(item);
                    }
                        else if (quant > Q)
                        {
                            int diff = Math.Abs(Q-quant);
                            obj.UpdateQuantity(item, diff);
                            obj.ItemUnavailable(item);
                            ViewBag.Quantity = String.Format(quant.ToString()+"Are Supplied"+"Purchase Order For"+diff.ToString()+"items is placed");
                            return RedirectToAction("Create");
                        }
                }
                else
                {
                    obj.AddStoreRequirement(store);
                    return RedirectToAction("Waiting");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Waiting()
        {
            return View();
        }
    }
}
