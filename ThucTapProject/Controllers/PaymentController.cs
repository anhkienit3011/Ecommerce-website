using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;

namespace ThucTapProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase {
        private readonly AppDbContext _context;

        public PaymentController() {
            _context = new AppDbContext();
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetUrlPayment(int orderId) {
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            //string vnp_Api = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            string vnp_TmnCode = "ODC3QKDE";
            string vnp_HashSecret = "EDYFPFVNNULZPQNJRNCKNYOGKKRCHFYN";
            string vnp_Returnurl = "http://14.231.153.229";

            OrderInfo orderInfo = new OrderInfo();
            orderInfo.Amount = (long)_context.Order.FirstOrDefault(c => c.OrderId==orderId).ActualPrice;
            orderInfo.Status = "0";
            orderInfo.OrderId = DateTime.Now.Ticks;
            orderInfo.CreatedDate = DateTime.Now;

            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (orderInfo.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            
            /*if (bankcode_Vnpayqr.Checked == true) {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            } else if (bankcode_Vnbank.Checked == true) {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            } else if (bankcode_Intcard.Checked == true) {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }*/

            vnpay.AddRequestData("vnp_CreateDate", orderInfo.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext));

            /*            if (locale_Vn.Checked == true) {
                            vnpay.AddRequestData("vnp_Locale", "vn");
                        } else if (locale_En.Checked == true) {
                            vnpay.AddRequestData("vnp_Locale", "en");
                        }*/
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + orderInfo.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderInfo.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Ok(paymentUrl);
        }
    }
}
