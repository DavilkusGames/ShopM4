using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopM4_DataMigrations.Data;
using ShopM4_Models;
using ShopM4_Models.ViewModels;
using ShopM4_Utility;
using ShopM4_Utility.BrainTree;
using ShopM4_DataMigrations.Repository.IRepository;
using System.Net.NetworkInformation;
using Braintree;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4_Models.ViewModels;

namespace ShopM4.Controllers
{
	public class OrderController : Controller
	{
		IRepositoryOrderHeader repositoryOrderHeader;
		IRepositoryOrderDetail repositoryOrderDetail;
		IBrainTreeBridge brainTreeBridge;

		[BindProperty]
		public OrderHeaderDetailViewModel OrderViewModel { get; set; }

		public OrderController(IRepositoryOrderHeader repositoryOrderHeader,
            IRepositoryOrderDetail repositoryOrderDetail, IBrainTreeBridge brainTreeBridge)
		{
			this.brainTreeBridge = brainTreeBridge;
			this.repositoryOrderDetail = repositoryOrderDetail;
			this.repositoryOrderHeader = repositoryOrderHeader;
		}

		public IActionResult Index(string searchName = null, string searchEmail = null,
					string searchPhone = null, string status = null)
		{
			OrderViewModel viewModel = new OrderViewModel()
			{
				OrderHeaderList = repositoryOrderHeader.GetAll(),
				StatusList = PathManager.StatusList.ToList().
							 Select(x => new SelectListItem { Text = x, Value = x })
            };

			if (searchName != null)
			{
				viewModel.OrderHeaderList = viewModel.OrderHeaderList.
					Where(x => x.FullName.ToLower().Contains(searchName.ToLower()));
            }

            if (searchEmail != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.Email.ToLower().Contains(searchEmail.ToLower()));
            }

            if (searchPhone != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.Phone.Contains(searchPhone));
            }

			if (status != null && status != "Choose Status")
			{
				viewModel.OrderHeaderList = viewModel.OrderHeaderList.
					Where(x => x.Status.Contains(status));
            }

            return View(viewModel);
		}


		public IActionResult Details(int id)
		{
			OrderViewModel = new OrderHeaderDetailViewModel()
			{
				OrderHeader = repositoryOrderHeader.FirstOrDefault(x => x.Id == id),
				OrderDetail = repositoryOrderDetail.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product")
			};


            return View(OrderViewModel);
		}


		[HttpPost]
		public IActionResult StartInProcessing()
		{
			// получаем объект из бд
			OrderHeader orderHeader = repositoryOrderHeader.
				FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

			orderHeader.Status = PathManager.StatusInProcess;

			repositoryOrderHeader.Save();

			return RedirectToAction("Index");
		}

        [HttpPost]
        public IActionResult StartOrderDone()
        {
			OrderHeader orderHeader = repositoryOrderHeader.
				FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

			orderHeader.Status = PathManager.StatusOrderDone;
            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StartOrderCancel()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

			
			var gateWay = brainTreeBridge.GetGateWay();

			// get transaction
			Transaction transaction = gateWay.Transaction.Find(orderHeader.TransactionId);

			// условия при которых не возвращаем
			if (transaction.Status == TransactionStatus.AUTHORIZED ||
				transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
			{
				var res = gateWay.Transaction.Void(orderHeader.TransactionId);
			}
            else // возврат средств
            {
				var res = gateWay.Transaction.Refund(orderHeader.TransactionId);
			}

			orderHeader.Status = PathManager.StatusDenied;
            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }
    }
}

