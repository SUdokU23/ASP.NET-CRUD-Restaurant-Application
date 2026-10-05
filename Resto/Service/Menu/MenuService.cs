using Microsoft.EntityFrameworkCore;
using Resto.Data;
using Resto.Models;
using Resto.Service.Upload;
using Resto.ViewModels.Menu;

namespace Resto.Service.Menu;

public class MenuService
{
    private readonly UploadService _uploadService;
    private readonly AppDbContext _context;

    public MenuService(AppDbContext context, UploadService uploadService)
    {
        _uploadService = uploadService;
        _context = context;
    }

    public async Task<List<MenuViewModel>> GetItems()
    {
        var items = await _context.Items
            .Include(i => i.Category)
            .Select(i => new MenuViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Category = i.Category!.Name,
                Description = i.Description,
                Price = i.Price,
                ImageUrl = i.ImageUrl,
                Weight = i.Weight,
                
            })
            .AsNoTracking()
            .ToListAsync();

        return items;
    }
    
    public async Task<Item?> GetItemById(int id)
    {
        var item = await _context.Items
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == id);

        return item;
    }

    private bool IsPriceRight(double price)
    {
        return price > 0;
    }

    private bool IsWeightRight(double weight)
    {
        return weight > 0;
    }

    public bool IsPriceAndWeightValid(double price, double weight)
    {
        return !(IsPriceRight(price) && IsWeightRight(weight));
    }
    
    public async Task<List<MenuViewModel>> GetItemsByCategory(string category)
    {
        var items = await _context.Items
            .Include(i => i.Category)
            .Where(i => i.Category!.Name == category)
            .Select(i => new MenuViewModel
            {
                Name = i.Name,
                Category = i.Category!.Name,
                Description = i.Description,
                Price = i.Price,
                ImageUrl = i.ImageUrl,
                Weight = i.Weight,
                
            })
            .AsNoTracking()
            .ToListAsync();

        return items;
    }

    public async Task<List<string>> GetCategories()
    {
        var categories = await _context.Categories.Select(c => c.Name).AsNoTracking().ToListAsync();
        
        return categories;
    }
    
    
    public List<MenuViewModel> SearchItems (List<MenuViewModel> items, string search)
    {
        var result = items.Where(i => i.Name.ToLower().Contains(search.ToLower())).ToList();
        
        return result;
    }

    public async Task<bool> AddItem(AddItemViewModel model)
    {
        if (!_uploadService.IsFileValid(model.image))
        {
            return false;
        }
            
        string path = await _uploadService.UploadFile(model.image);
        
        if (path == "Error") return false;
        
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
        if (category == null)
        {
            return false;
        }

        model.Item.CategoryId = category.Id;
        model.Item.ImageUrl = path;
        _context.Items.Add(model.Item);
        
        return await _context.SaveChangesAsync() > 0;

    }
    
    public async Task<bool> Update(EditItemViewModel model)
    {
        var prev = _context.Items.FirstOrDefault(i => i.Id == model.Item.Id);
        
        if (prev == null)
        {
            return false;
        }
        if (model.image != null)
        {
            if (!_uploadService.IsFileValid(model.image))
            {
                return false;
            }
            await _uploadService.DeleteFile(prev.ImageUrl);
            
            string path = await _uploadService.UploadFile(model.image);
            
            if (path == "Error") return false;
            
            prev.ImageUrl = path;
        }
        
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
        if (category == null)
        {
            return false;
        }
        
        prev.Name = model.Item.Name;
        prev.CategoryId = category.Id;
        prev.Description = model.Item.Description;
        prev.IsAvailable = model.Item.IsAvailable;
        
        Console.WriteLine(model.Item.IsAvailable);
        
        prev.Weight = model.Item.Weight;
        prev.Price = model.Item.Price;
        
        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<bool> DeleteItem(int itemId)
    {
        var itemToBeDeleted = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

        if (itemToBeDeleted == null)
        {
            return false;
        }

        if (itemToBeDeleted.ImageUrl is "Undefined" or "")
        {
            _context.Items.Remove(itemToBeDeleted);
            
            return await _context.SaveChangesAsync() > 0;
        }

        if (!await _uploadService.DeleteFile(itemToBeDeleted.ImageUrl)) return false;
        _context.Items.Remove(itemToBeDeleted);
        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<bool> AddCategory(string name)
    {
        var newCategory = new Category
        {
            Name = name
        };
        
        _context.Categories.Add(newCategory);
        
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<Category> GetCategoryByName(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
    }
    
    public async Task<bool> DeleteCategory(Category category)
    {
        if (category == null)
        {
            return false;
        }
        
        _context.Categories.Remove(category);
        
        return await _context.SaveChangesAsync() > 0;
    }
}