using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using HubService.Application.Interfaces.Repositories;
using HubService.Application.Interfaces.Services;
using HubService.Application.Services;
using HubService.Domain.Entities;
using HubService.Application.RabbitMq.Interfaces; 

public class BetServiceTests
{
    [Fact]
    public async Task CreateBetAsync_ReturnsCorrectResult_And_PublishesBetEvent()
    {
        var mockBetRepository = new Mock<IBetRepository>();
        var mockMessageProducer = new Mock<IMessageProducer>();

        var betService = new BetService(mockBetRepository.Object, mockMessageProducer.Object);

        decimal testAmount = 100;
        bool testIsWin = true;

        mockBetRepository.Setup(repo => repo.CreateAsync(It.IsAny<BetEntity>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((BetEntity bet, CancellationToken token) =>
                         {
                             return bet;
                         });
        var resultDto = await betService.CreateBetAsync(testAmount, testIsWin);
        var resultBet = Assert.IsType<BetEntity>(resultDto.Data);

        Assert.True(resultDto.Success);
        Assert.Equal(testAmount, resultBet.Amount);
        Assert.Equal(testIsWin, resultBet.IsWin);
        Assert.True((DateTime.Now - resultBet.PlacedAt).TotalSeconds < 1, "PlacedAt should be set to a recent time.");

        mockMessageProducer.Verify(mp => mp.SendMessage(It.Is<BetEntity>(
            b => b.Amount == testAmount && b.IsWin == testIsWin)), Times.Once);
    }
}
