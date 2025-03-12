using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Models;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PaymentController : Controller
{
    private readonly StripeSettings _stripeSettings;
    private readonly AppDbContext _context;

    public PaymentController(IOptions<StripeSettings> stripeSettings, AppDbContext context)
    {
        _stripeSettings = stripeSettings.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        _context = context;
    }

    public IActionResult Odenis()
    {
        return View();
    }

    public IActionResult Cixaris()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Medaxil(int? amount)
    {
        if (!amount.HasValue || amount <= 0)
        {
            TempData["Error"] = "Məbləğ düzgün daxil edilməyib.";
            return RedirectToAction("Odenis");
        }

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Balans artırma"
                        },
                        UnitAmount = amount.Value * 100 // Dollar → Cent çevrilməsi
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = Url.Action("PaymentSuccess", "Payment", new { amount = amount }, Request.Scheme),
            CancelUrl = Url.Action("PaymentCancel", "Payment", null, Request.Scheme)
        };

        var service = new SessionService();
        Session session = service.Create(options);

        

        return Json(new { id = session.Id });
    }

    public async Task<IActionResult> PaymentSuccess(int amount)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

        if (user == null)
        {
            TempData["Error"] = "İstifadəçi tapılmadı.";
            return RedirectToAction("Index", "Profile");
        }

        // Balansı artır
        user.Balance += amount;

        Odenis odenis = new Odenis
        {
            UserId = user.Id,
            Mebleg = amount,
        };
        await _context.Odenis.AddAsync(odenis);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Məbləğ uğurla əlavə edildi!";
        return RedirectToAction("Index", "Profile", new { username = User.Identity!.Name });
    }

    public IActionResult PaymentCancel()
    {
        TempData["Error"] = "Ödəniş ləğv edildi.";
        return RedirectToAction("Odenis");
    }

    [HttpPost]
    public async Task<IActionResult> Mexaric(int amount, string card)
    {
        if (string.IsNullOrEmpty(card) || card.Length != 16)
        {
            TempData["Error"] = "Kart nömrəsi düzgün daxil edilməyib.";
            return RedirectToAction("Cixaris");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
        if (user == null)
        {
            TempData["Error"] = "İstifadəçi tapılmadı.";
            return RedirectToAction("Cixaris");
        }

        if (amount <= 0)
        {
            TempData["Error"] = "Düzgün məbləğ daxil edin.";
            return RedirectToAction("Cixaris");
        }

        if (user.Balance < amount)
        {
            TempData["Error"] = "Balansınızda kifayət qədər məbləğ yoxdur.";
            return RedirectToAction("Cixaris");
        }

        // Balansı azalt
        user.Balance -= amount;

        Cixaris cixaris = new Cixaris
        {
            UserId = user.Id,
            Mebleg=amount,
            CardName = card,            
        };
        await _context.Cixaris.AddAsync(cixaris);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Məbləğ uğurla çıxarıldı! ${amount} bank hesabiniza 24 saat erzinde gelecek.";
        return RedirectToAction("Index", "Profile", new { username = User.Identity!.Name });
    }


}
