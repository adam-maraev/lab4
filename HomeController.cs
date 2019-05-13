using IGI_4.Models;
using IGI_4.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace IGI_4.Controllers
{
    public class HomeController : Controller
    {
        Repository repository;
        public HomeController(Repository repository)
        {
            this.repository = repository;
        }
        public IActionResult index()
        {
            return View();
        }
        
        #region Views
        public IActionResult Client(int? idFrom = null, int? idTo = null, string sort = null)
        {
            var model = repository.GetClients();
            if (idFrom != null) model = model.Where(x => x.ID >= idFrom);
            if (idTo != null) model = model.Where(x => x.ID <= idTo);
            if (sort != null) HttpContext.Session.SetString("ClientSort", sort);
            else sort = HttpContext.Session.GetString("ClientSort");
            switch (sort)
            {
                case "ID": model = model.OrderBy(x => x.ID); break;
                case "Name": model = model.OrderBy(x => x.Name); break;
                case "FirmName": model = model.OrderBy(x => x.FirmName); break;
                case "Phone": model = model.OrderBy(x => x.Phone); break;
                case "Adress": model = model.OrderBy(x => x.Adress); break;
            }
            return View(model);
        }
        public IActionResult Furniture(int? idFrom = null, int? idTo = null, string sort = null)
        {
            var model = repository.GetFurnitures();
            if (idFrom != null) model = model.Where(x => x.ID >= idFrom);
            if (idTo != null) model = model.Where(x => x.ID <= idTo);
            if (sort != null) HttpContext.Session.SetString("FurnitureSort", sort);
            else sort = HttpContext.Session.GetString("FurnitureSort");
            switch (sort)
            {
                case "ID": model = model.OrderBy(x => x.ID); break;
                case "Name": model = model.OrderBy(x => x.Name); break;
                case "Material": model = model.OrderBy(x => x.Material); break;
                case "Description": model = model.OrderBy(x => x.Description); break;
                case "Cost": model = model.OrderBy(x => x.Cost); break;
                case "Count": model = model.OrderBy(x => x.Count); break;
            }
            return View(model);
        }
        public IActionResult Order(int? idFrom = null, int? idTo = null, string sort = null)
        {
            var model = repository.GetOrders();
            if (idFrom != null) model = model.Where(x => x.ID >= idFrom);
            if (idTo != null) model = model.Where(x => x.ID <= idTo);
            if (sort != null) HttpContext.Session.SetString("OrderSort", sort);
            else sort = HttpContext.Session.GetString("OrderSort");
            switch (sort)
            {
                case "ID": model = model.OrderBy(x => x.ID); break;
                case "OrderDate": model = model.OrderBy(x => x.OrderDate); break;
                case "IsOrderComplete": model = model.OrderBy(x => x.IsOrderComplete); break;
                case "Discount": model = model.OrderBy(x => x.Discount); break;
                case "Client": model = model.OrderBy(x => x.Client.Name); break;
                case "Furniture": model = model.OrderBy(x => x.Furniture.Name); break;
                case "Worker": model = model.OrderBy(x => x.Worker.Name); break;
            }
            return View(model);
        }
        public IActionResult Worker(int? idFrom = null, int? idTo = null, string sort = null)
        {
            var model = repository.GetWorkers();
            if (idFrom != null) model = model.Where(x => x.ID >= idFrom);
            if (idTo != null) model = model.Where(x => x.ID <= idTo);
            if (sort != null) HttpContext.Session.SetString("WorkerSort", sort);
            else sort = HttpContext.Session.GetString("WorkerSort");
            switch (sort)
            {
                case "ID": model = model.OrderBy(x => x.ID); break;
                case "Name": model = model.OrderBy(x => x.Name); break;
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult AddClient()
        {
            Client model = null;
            if (HttpContext.Session.Keys.Contains("AddClient"))
            {
                model = JsonConvert.DeserializeObject<Client>(HttpContext.Session.GetString("AddClient"));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult AddFurniture()
        {
            Furniture model = null;
            if (HttpContext.Session.Keys.Contains("AddFurniture"))
            {
                model = JsonConvert.DeserializeObject<Furniture>(HttpContext.Session.GetString("AddFurniture"));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult AddOrder()
        {
            var model = new OrderViewModel
            {
                Order = null,
                Clients = repository.GetClients(),
                Furnitures = repository.GetFurnitures(),
                Workers = repository.GetWorkers()
            };
            if (HttpContext.Session.Keys.Contains("AddOrder"))
            {
                model.Order = JsonConvert.DeserializeObject<Order>(HttpContext.Session.GetString("AddOrder"));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult AddWorker()
        {
            Worker model = null;
            if (HttpContext.Session.Keys.Contains("AddWorker"))
            {
                model = JsonConvert.DeserializeObject<Worker>(HttpContext.Session.GetString("AddWorker"));
            }
            return View(model);
        }
        #endregion

        #region Add functional
        [HttpPost]
        public ActionResult AddClient(Client client, string action)
        {
            if (action == "Add")
            {
                HttpContext.Session.Remove("AddClient");
                repository.AddClient(client);
            }
            else
            {
                HttpContext.Session.SetString("AddClient", JsonConvert.SerializeObject(client));
            }
            return RedirectToAction("Client");
        }
        [HttpPost]
        public ActionResult AddFurniture(Furniture furniture, string action)
        {
            if (action == "Add")
            {
                HttpContext.Session.Remove("AddFurniture");
                repository.AddFurniture(furniture);
            }
            else
            {
                HttpContext.Session.SetString("AddFurniture", JsonConvert.SerializeObject(furniture));
            }
            return RedirectToAction("Furniture");
        }
        [HttpPost]
        public ActionResult AddOrder(Order order, string clientName, string furnitureName, string workerName, string action)
        {
            order.Client = repository.GetClientByName(clientName);
            order.Furniture = repository.GetFurnitureByName(furnitureName);
            order.Worker = repository.GetWorkerByName(workerName);
            if (action == "Add")
            {
                HttpContext.Session.Remove("AddOrder");
                repository.AddOrder(order);
            }
            else
            {
                HttpContext.Session.SetString("AddOrder", JsonConvert.SerializeObject(order));
            }

            return RedirectToAction("Order");
        }
        [HttpPost]
        public ActionResult AddWorker(Worker worker, string action)
        {
            if (action == "Add")
            {
                HttpContext.Session.Remove("AddWorker");
                repository.AddWorker(worker);
            }
            else
            {
                HttpContext.Session.SetString("AddWorker", JsonConvert.SerializeObject(worker));
            }
            return RedirectToAction("Worker");
        }
        #endregion
    }
}