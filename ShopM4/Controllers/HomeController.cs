using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopM4_DataMigrations.Data;
using ShopM4_Models;
using ShopM4_Models.ViewModels;
using ShopM4_Utility;

using ShopM4_DataMigrations.Repository.IRepository;

namespace ShopM4.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    //private ApplicationDbContext db;
    private IRepositoryProduct repositoryProduct;
    private IRepositoryCategory repositoryCategory;

    public HomeController(ILogger<HomeController> logger,
        IRepositoryProduct repositoryProduct, IRepositoryCategory repositoryCategory)
    {
        _logger = logger;

        this.repositoryCategory = repositoryCategory;
        this.repositoryProduct = repositoryProduct;
    }

    public IActionResult Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel()
        {
            Products = repositoryProduct.GetAll(includeProperties:
            $"{PathManager.NameCategory},{PathManager.NameMyModel}"),
            Categories = repositoryCategory.GetAll()
        };


        return View(homeViewModel);
    }

    public IActionResult Details(int id)
    {
        List<Cart> cartList = new List<Cart>();

        if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
        {
            cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
        }


        DetailsViewModel detailsViewModel = new DetailsViewModel()
        {
            IsInCart = false,


            //Product = db.Product.Include(x => x.Category).Include(x => x.MyModel).
            //                     Where(x => x.Id == id).FirstOrDefault()

            Product = repositoryProduct.FirstOrDefault(
                filter: x => x.Id == id,
                includeProperties: $"{PathManager.NameCategory},{PathManager.NameMyModel}")
        };

        // проверка на наличие товара в корзине
        // если товар есть, то меняем свойство
        foreach (var item in cartList)
        {
            if (item.ProductId == id)
            {
                detailsViewModel.IsInCart = true; 
            }
        }

        return View(detailsViewModel);
    }

    [HttpPost]
    public IActionResult DetailsPost(int id)
    {
        List<Cart> cartList = new List<Cart>();

        if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
        {
            cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
        }

        cartList.Add(new Cart() { ProductId = id });

        HttpContext.Session.Set(PathManager.SessionCart, cartList);

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int id)
    {
        List<Cart> cartList = new List<Cart>();

        if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
        {
            cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
        }

        // get product from cart
        var item = cartList.Single(x => x.ProductId == id);

        if (item != null)
        {
            cartList.Remove(item);
        }

        // SET SESSION
        HttpContext.Session.Set(PathManager.SessionCart, cartList);

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

