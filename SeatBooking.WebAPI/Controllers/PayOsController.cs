using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using SeatBooking.API.Configurations;
using SeatBooking.Application.Services;
using SeatBooking.Domain.DTO.Request;
using SeatBooking.Domain.Entities;
using SeatBooking.Infrastructure.Services;

namespace SeatBooking.WebAPI.Controllers
{
    public class PayOsController(PayOS payOs, ISeatService seatService) : BaseController
    {
        /*[HttpPost]
        public async Task<IActionResult> Checkout([FromQuery] List<int> userId, [FromQuery] Guid courseId, [FromQuery] string paymentMethod, [FromQuery] double fee, [FromQuery] string fullName, [FromQuery] string phoneNumber)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                var totalFee = 0.0;
                List<ItemData> items = new List<ItemData>();

                var course = _service.GetCourseById(courseId);
                ItemData data = new ItemData(course.Result.Data.Title, 1, (int)course.Result.Data.Tuitionfee);
                items.Add(data);
                totalFee += course.Result.Data.Tuitionfee;

                var baseUrl = "http://35.198.226.22:10000";
                //var baseUrl = "https://localhost:7021";

                var successUrl = $"{baseUrl}{ApiEndPointConstant.UserCourse.CourseUserEndpointJoin}?userId={userId}&courseId={courseId}&paymentMethod={paymentMethod}&fee={fee}&fullName={fullName}&phoneNumber={phoneNumber}";
                var cancelUrl = "http://68.183.186.61:3000";
                PaymentData paymentData = new PaymentData(orderCode, (int)totalFee, "Thanh toan hoc phi", items, cancelUrl, successUrl, buyerName: fullName, buyerPhone: phoneNumber);
                CreatePaymentResult createPayment = await _payOs.createPaymentLink(paymentData);

                return Ok(new
                {
                    message = "redirect",
                    url = createPayment.checkoutUrl
                });
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Redirect("http://68.183.186.61:3000");
            }
        }*/
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
            {
                return BadRequest("Invalid payment data.");
            }
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            List<ItemData> items = new List<ItemData>();
            var seats = seatService.GetPagination(1, paymentRequest.Seats).Result.Data;
            var booking = await seatService.CreateBooking(paymentRequest);
            string numbersString = string.Join(",", booking);
            foreach (var seat in seats)
            {
                ItemData data = new ItemData(seat.SeatInfo, 1, seat.SeatColor.Price);
                items.Add(data);
            }

        // {ApiEndPointConstant.UserCourse.CourseUserEndpointJoin}?userId={userId}&courseId={courseId}&paymentMethod={paymentMethod}&fee={fee}&fullName={fullName}&phoneNumber={phoneNumber}"
        
            var successUrl = $"https://seat-booking.azurewebsites.net/api/PayOs?idBooking={Uri.EscapeDataString(numbersString)}&amount={paymentRequest.TotalAmount}&showTime={paymentRequest.BookingShow}";
            var cancelUrl = "https://seat-booking-drab.vercel.app/";
            PaymentData paymentData = new PaymentData(orderCode, (int)paymentRequest.TotalAmount, $"thanh toan ghe ngoi dot {paymentRequest.BookingShow}", items, cancelUrl, successUrl);
            CreatePaymentResult createPayment = await payOs.createPaymentLink(paymentData);

            return Ok(new
            {
                message = "redirect",
                url = createPayment.checkoutUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> Success([FromQuery] string idBooking, [FromQuery] int amount, [FromQuery] int showTime)
        {
            var decodedIds = Uri.UnescapeDataString(idBooking); // Giải mã id
            var success = await seatService.CreateTransactionsFromNumbers(decodedIds,amount);
            if (success)
            {
                if (showTime == 1)
                {
                    return Redirect("https://seat-booking-drab.vercel.app/concert-dot-1");
                }
                return Redirect("https://seat-booking-drab.vercel.app/concert-dot-2");
            }
            return Redirect("https://seat-booking-drab.vercel.app/concert-dot-1");
        }
    }
}
