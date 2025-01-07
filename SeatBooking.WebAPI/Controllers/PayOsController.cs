using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using SeatBooking.API.Configurations;
using SeatBooking.Domain.DTO.Request;

namespace SeatBooking.WebAPI.Controllers
{
    public class PayOsController(PayOS payOs) : BaseController
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
        public IActionResult ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
            {
                return BadRequest("Invalid payment data.");
            }

            // Log the received data (for debugging purposes)
            Console.WriteLine("Payment Data Received:");
            Console.WriteLine($"Student Name: {paymentRequest.StudentName}");
            Console.WriteLine($"Selected Branch: {paymentRequest.SelectedBranch}");
            Console.WriteLine($"Seats: {string.Join(", ", paymentRequest.Seats)}");
            Console.WriteLine($"Total Amount: {paymentRequest.TotalAmount}");

            // Perform your business logic here, e.g.:
            // - Validate seat availability
            // - Save payment details to the database
            // - Process the payment

            // Return a success response
            return Ok(new
            {
                Message = "Payment processed successfully",
                Data = paymentRequest
            });
        }
    }
}
