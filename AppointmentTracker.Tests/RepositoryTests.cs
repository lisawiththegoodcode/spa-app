using System;
using Xunit;
using AppointmentTracker.Services;
using AppointmentTracker.Models;
using System.Linq;
using System.Collections.Generic;
using Moq;
using AppointmentTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentTracker.Tests
{
    public class RepositoryTests
    {
        //HELPER METHOD THAT INSTANTIATES THE IREADONLYSPAAPPCONTEXT
        private IReadOnlySpaAppContext CreateTestReadOnlyContext()
        {
            var testReadOnlyContext = new Mock<IReadOnlySpaAppContext>();
            var testAppts = new List<AppointmentModel>
            {
                new AppointmentModel
                {
                    Id = 1,
                    AppointmentTime = new DateTime(2018, 12, 01, 11, 00, 00),
                    ProviderId = 1
                }
            };

            testReadOnlyContext.Setup(x => x.Appointments).Returns(testAppts.AsQueryable());
            return testReadOnlyContext.Object;
        }

        //HELPER METHOD THAT INSTANTIATES THE SPAAPPCONTEXT
        private SpaAppContext CreateSpaAppContext()
        {
            var testContext = new Mock<SpaAppContext>(new DbContextOptionsBuilder<SpaAppContext>().Options);
            testContext.Setup(c => c.Add(It.IsAny<AppointmentModel>())).Returns<EntityEntry<AppointmentModel>>(null);
            testContext.Setup(c => c.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(0));

            return testContext.Object;
        }
        
        //TESTS THE ISPROVIDERAVAILABLE() METHOD
        [Fact]
        public void IsProviderAvailableMethod_ReturnsFalseWhenAppointmentConflict()
        {
            // ARRANGE
            var proposedAppt = new AppointmentModel
            {
                Id = 2,
                AppointmentTime = new DateTime(2018, 12, 1, 11, 00, 00),
                ProviderId = 1
            };
            var testRepo = new Repository(null, CreateTestReadOnlyContext());

            // ACT
            var isAvailable = testRepo.IsProviderAvailable(proposedAppt);

            // ASSERT
            Assert.False(isAvailable);
        }

        //TESTS THE ISPROVIDERAVAILABLE() METHOD
        [Fact]
        public void IsProviderAvailableMethod_ReturnsTrueWhenNoAppointmentConflict()
        {
            // ARRANGE
            var proposedAppt = new AppointmentModel
            {
                Id = 2,
                AppointmentTime = new DateTime(2018, 1, 1, 11, 00, 00),
                ProviderId = 1
            };
            var testRepo = new Repository(null, CreateTestReadOnlyContext());

            // ACT
            var isAvailable = testRepo.IsProviderAvailable(proposedAppt);

            // ASSERT
            Assert.True(isAvailable);
        }

        //TESTS THE ADDAPPOINTMENT() METHOD
        [Fact]
        public void AddAppointmentMethod_ThrowsNoException()
        {
            // ARRANGE
            var newAppt = new AppointmentModel
            {
                Id = 1,
                AppointmentTime = new DateTime(2018, 12, 1, 11, 00, 00),
                ProviderId = 2
            };
            var testRepo = new Repository(CreateSpaAppContext(), CreateTestReadOnlyContext());
            string message = null;

            //ACT
            try
            {
                testRepo.AddAppointment(newAppt);
            }
            //ASSERT
            catch (Exception e)
            {
                message = e.ToString();
            }

            Assert.Null(message);
        }

        //TESTS THE ADDAPPOINTMENT() METHOD
        [Fact]
        public void AddAppointmentMethod_ThrowsExceptionWhenProviderIsNotAvailable()
        {
            // ARRANGE
            var newAppt = new AppointmentModel
            {
                Id = 2,
                AppointmentTime = new DateTime(2018, 12, 1, 11, 00, 00),
                ProviderId = 1
            };
            var testRepo = new Repository(CreateSpaAppContext(), CreateTestReadOnlyContext());

            //ACT & ASSERT
            Assert.Throws<System.Exception>(() => testRepo.AddAppointment(newAppt));
        }
    }
}
