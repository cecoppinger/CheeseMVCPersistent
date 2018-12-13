using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu()
                {
                    Name = viewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(viewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            if (id == 0)
                return Redirect("/Menu");

            Menu menu = context.Menus.Single(m => m.ID == id);
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(x => x.Cheese)
                .Where(x => x.MenuID == id)
                .ToList();

            ViewMenuViewModel viewModel = new ViewMenuViewModel()
            {
                Menu = menu,
                Items = items
            };

            return View(viewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu myMenu = context.Menus.Single(x => x.ID == id);
            AddMenuItemViewModel viewModel = new AddMenuItemViewModel(myMenu, context.Cheeses);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(x => x.CheeseID == viewModel.CheeseID)
                    .Where(x => x.MenuID == viewModel.MenuID)
                    .ToList();

                if(existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu()
                    {
                        CheeseID = viewModel.CheeseID,
                        MenuID = viewModel.MenuID
                    };

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();
                }

                return Redirect("/Menu/ViewMenu/" + viewModel.MenuID);
            }

            return View(viewModel);
        }
    }
}