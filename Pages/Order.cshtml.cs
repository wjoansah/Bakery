using Bakery.Data;
using Bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Bakery.Pages
{
    public class OrderModel : PageModel
    {
         private readonly BakeryContext _bakeryContext;
        public OrderModel(BakeryContext bakeryContext)
        {
            _bakeryContext = bakeryContext;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Product Product { get; set; }

        [BindProperty, EmailAddress, Required, Display(Name = "Your Email Address")]
        public string OrderEmail { get; set; }

        [BindProperty, Required(ErrorMessage = "Please supply a shipping address"), Display(Name = "Shipping Address")]
        public string OrderShipping { get; set; }

        [BindProperty, Display(Name = "Quantity")]
        public int OrderQuantity { get; set; } = 1;

        public void OnGet()
        {
            Product = _bakeryContext.Products.Find(Id == 0 ? 1 : Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Product = _bakeryContext.Products.Find(Id == 0 ? 1 : Id);
            if (ModelState.IsValid)
            {
                // TODO: try to send an email here later
                return RedirectToPage("OrderSuccess");
            }
            return Page();
        }

        public async void SendOrderConfirmationMail()
        {
                var body = $@"<p>Thank you, we have received your order for {OrderQuantity} unit(s) of {Product.Name}!</p>
        <p>Your address is: <br/>{OrderShipping.Replace("\n", "<br/>")}</p>
        Your total is ${Product.Price * OrderQuantity}.<br/>
        We will contact you if we have questions about your order.  Thanks!<br/>";
                using (var smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = @"c:\mailpickup";
                    var message = new MailMessage();
                    message.To.Add(OrderEmail);
                    message.Subject = "Fourth Coffee - New Order";
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.From = new MailAddress("sales@fourthcoffee.com");
                    await smtp.SendMailAsync(message);
                }
        }
    }

}
