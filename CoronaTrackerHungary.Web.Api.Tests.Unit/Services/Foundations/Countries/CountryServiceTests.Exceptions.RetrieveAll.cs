﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaTrackerHungary.Web.Api.Models.Countries;
using CoronaTrackerHungary.Web.Api.Models.Countries.Exceptions;
using Moq;
using Xunit;

namespace CoronaTrackerHungary.Web.Api.Tests.Unit.Services.Foundations.Countries
{
    public partial class CountryServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedCountryException =
                new FailedCountryServiceException(serviceException);

            var expectedCountryServiceException =
                new CountryServiceException(failedCountryException);

            this.apiBrokerMock.Setup(broker =>
                broker.GetAllCountriesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Country>> getAllCountriesTask =
                this.countryService.RetrieveAllCountrieasAsync();

            // then
            await Assert.ThrowsAsync<CountryServiceException>(() =>
                getAllCountriesTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetAllCountriesAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                expectedCountryServiceException))),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }


    }
}
