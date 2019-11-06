using System;
using Xunit;
using AppointmentTracker.Services;
using AppointmentTracker.Models;
using AppointmentTracker.Controllers;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AppointmentTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace AppointmentTracker.Tests
{
    public class ControllerTests
    {
        //HELPER METHOD THAT INSTANTIATES THE IREPOSITORY
        private IRepository CreateIRepository()
        {
            var mockIRepo = new Mock<IRepository>();
            return mockIRepo.Object;
        }

        //APPOINTMENT CONTROLLER TEST
        [Fact]
        public void AppointmentIndex_ReturnsAViewResult()
        {
            //ARRANGE
            var testController = new AppointmentController(CreateIRepository());

            //ACT
            var result = testController.Index();

            //ASSERT
            Assert.IsType<ViewResult>(result);
        }

        //CUST CONTROLLER TEST
        [Fact]
        public void CustomerCreate_RedirectsToIndexPage_IfPostSuccessful()
        {
            //ARRANGE
            var testCustomer = new CustomerModel();
            var testController = new CustomerController(CreateIRepository());
            
            //ACT

            var result = testController.Create(testCustomer);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            //ASSERT
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        //CUST CONTROLLER TEST
        [Fact]
        public void CustomerCreate_ReturnsAViewResult_IfExceptionThrown()
        {
            //ARRANGE
            var testCustomer = new CustomerModel();
            var mockIRepo = new Mock<IRepository>();
            mockIRepo.Setup(c => c.AddCustomer(testCustomer)).Throws<Exception>();
            var testController = new CustomerController(mockIRepo.Object);

            //ACT
            var result = testController.Create(testCustomer);

            //ASSERT
            Assert.IsType<ViewResult>(result);
        }
    

        //SERVICE PROVIDER CONTROLLER TEST
        [Fact]
        public void ServiceProviderDetails_ReturnsAViewResult()
        {
            //ARRANGE
            var testServiceProvider = new ServiceProviderModel { Id = 1 };
            var testAppts = new List<AppointmentModel>
            {
                new AppointmentModel
                {
                    Id = 1,
                    AppointmentTime = new DateTime(2018, 12, 01, 11, 00, 00),
                    ProviderId = 1
                }
            };
            var mockIRepo = new Mock<IRepository>();
            mockIRepo.Setup(x => x.GetAppointmentsForProvider(testServiceProvider.Id)).Returns(testAppts);
            var testController = new ServiceProviderController(mockIRepo.Object);

            //ACT
            var result = testController.Details(1, testServiceProvider);

            //ASSERT
            Assert.IsType<ViewResult>(result);
            Assert.Equal(1, testServiceProvider.Id);
        }

        //HomeController Test
        [Fact]
        public void HomeIndex_ReturnsAViewResult()
        {
            //assemble
            var testHomeController = new HomeController();

            //act
            var result = testHomeController.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
    } 
}
