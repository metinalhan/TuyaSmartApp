using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Context;
using TuyaApp.Persistence.Repositories;
using TuyaApp.Persistence.Services;

namespace TuyaApp.Test
{
    public class UnitTest1
    {
        //private async Task<TuyaAppDbContext> GetDatabaseContext()
        //{
        //    var options = new DbContextOptionsBuilder<TuyaAppDbContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //        .Options;

        //    var databaseContext = new TuyaAppDbContext(options);           
        //    databaseContext.Database.EnsureCreated();

        //    if(await databaseContext.TuyaAccounts.CountAsync() == 0)
        //    {
        //        var account = new TuyaAccount
        //        {
        //            AccountName = "Test",
        //            ClientId = Guid.NewGuid().ToString(),
        //            Secret = Guid.NewGuid().ToString(),
        //            IsDefault = true                    
        //        };


        //        await databaseContext.TuyaAccounts.AddAsync(account);
        //        await databaseContext.SaveChangesAsync();
        //    }

        //    return databaseContext;
        //}

        [Fact]
        public async Task GetAllTuyaAccountsAsync_Should_ReturnListAllAccounts()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}