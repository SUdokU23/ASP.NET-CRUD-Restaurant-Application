using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resto.Service.Menu;
using Resto.ViewModels.Menu;

namespace Resto.Controllers;

public class HomeController(MenuService menuService) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(List<MenuViewModel> model)
    {
        model = await menuService.GetItems();

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.Descending = false;
        ViewBag.Search = string.Empty;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string category, bool descending, string search)
    {
        var model = new List<MenuViewModel>();

        if (category == "none")
        {
            model = await menuService.GetItems();
        }
        else
        {
            model = await menuService.GetItemsByCategory(category);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            model = menuService.SearchItems(model, search);
        }

        if (descending)
        {
            model = model.OrderByDescending(x => x.Price).ToList();
        }

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.SelectedCategory = category;
        ViewBag.Descending = descending;
        ViewBag.Search = search;

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> AddItem()
    {
        var model = new AddItemViewModel();

        ViewBag.Categories = await menuService.GetCategories();

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> AddItem(AddItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data entered!";

            ViewBag.SelectedCategory = model.CategoryName;
            ViewBag.Categories = await menuService.GetCategories();

            return View(model);
        }

        if (menuService.IsPriceAndWeightValid(model.Item.Price, model.Item.Weight))
        {
            TempData["Error"] = "Weight and Price must be greater than zero!";

            ViewBag.SelectedCategory = model.CategoryName;
            ViewBag.Categories = await menuService.GetCategories();

            return View(model);
        }

        if (await menuService.AddItem(model))
        {
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Something went wrong!";

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.SelectedCategory = model.CategoryName;

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> EditItem(int id)
    {
        var model = new EditItemViewModel
        {
            Item = await menuService.GetItemById(id),
        };

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.SelectedCategory = model.Item.Category?.Name;

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> EditItem(EditItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data entered!";

            ViewBag.SelectedCategory = model.CategoryName;
            ViewBag.Categories = await menuService.GetCategories();

            return View(model);
        }

        if (menuService.IsPriceAndWeightValid(model.Item.Price, model.Item.Weight))
        {
            TempData["Error"] = "Weight and Price must be greater than zero!";

            ViewBag.SelectedCategory = model.CategoryName;
            ViewBag.Categories = await menuService.GetCategories();

            return View(model);
        }

        if (await menuService.Update(model))
        {
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Something went wrong!";

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.SelectedCategory = model.CategoryName;

        return View(model);
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        if (await menuService.DeleteItem(itemId))
        {
            ViewBag.Categories = await menuService.GetCategories();
            ViewBag.Descending = false;
            ViewBag.Search = string.Empty;

            return RedirectToAction("Index");
        }

        TempData["Error"] = "Something went wrong!";

        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.Descending = false;
        ViewBag.Search = string.Empty;

        return RedirectToAction("Index");
    }
    
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Please enter a name";
            
            ViewBag.Categories = await menuService.GetCategories();
            ViewBag.Descending = false;
            ViewBag.Search = string.Empty;
            return RedirectToAction("Index");
        }

        if (await menuService.AddCategory(name))
        {
            ViewBag.Categories = await menuService.GetCategories();
            ViewBag.Descending = false;
            ViewBag.Search = string.Empty;
            
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong!";
        
        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.Descending = false;
        ViewBag.Search = string.Empty;
        
        return RedirectToAction("Index");
    }

    
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(string category)
    {
        var categoryToBeDeleted = await menuService.GetCategoryByName(category);
        if (categoryToBeDeleted == null)
        {
            TempData["Error"] = "Couldn't find the category";
            
            ViewBag.Categories = await menuService.GetCategories();
            ViewBag.Descending = false;
            ViewBag.Search = string.Empty;
            return RedirectToAction("Index");
        }

        if (await menuService.DeleteCategory(categoryToBeDeleted))
        {
            ViewBag.Categories = await menuService.GetCategories();
            ViewBag.Descending = false;
            ViewBag.Search = string.Empty;
            
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong!";
        
        ViewBag.Categories = await menuService.GetCategories();
        ViewBag.Descending = false;
        ViewBag.Search = string.Empty;
        return RedirectToAction("Index");
    }
}