using System;
using AutoMapper;
using AddressBookApi.Controllers;
using AddressBookApi.Models.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Contracts;
using AddressBookApi.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AddressBookApi.Repositories;
using AddressBookApi.Services;
using System.Collections.Generic;
using ExceptionHandler;
using LoggerService;

namespace AddressBookApi.Test;

public class UserControllerTest
{
    private readonly DbContextOptions<AddressBookDataContext> _contextOptions;
    private readonly IUserService _service;
    private readonly UserController _controller;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly ILoggerManager _logger;

    public UserControllerTest()
    {
        _logger = new LoggerManager();
        _contextOptions = ContextFactory.DbContextOptionsInMemory();
        ContextFactory.CreateDataBaseInMemory(_contextOptions);
        _repository = new UserRepository(new AddressBookDataContext(_contextOptions), _logger);
        _service = new UserService(_logger, _repository , _mapper);
    }

    [Fact]
    public async Task GetUserById_Success()
    {
        UserController _controller = new UserController(_logger, _service);
        var result = _controller.GetUserById(Guid.Parse("e37beb34-f626-4c9e-8988-a8bb6ca1483d")) as ObjectResult;
        Assert.IsType<UserDto>(result.Value);
    }

    [Fact]
    public async Task GetUserById_NotFound()
    {
        UserController _controller = new UserController(_logger, _service);
        try
        {
            _controller.GetUserById(Guid.NewGuid());
        }
        catch (CustomException ex)
        {
            Assert.Equal("Id not found", ex.Message);
        }
    }

    [Fact]
    public async Task GetUser_Success()
    {
        UserController _controller = new UserController(_logger, _service);
        var result = _controller.GetUser() as ObjectResult;
        Assert.IsType<List<UserDto>>(result.Value);
    }

    [Fact]
    public async Task CreateUser_Success()
    {
        UserController _controller = new UserController(_logger, _service);
        var user = new CreateUserDto() { FirstName = "sneha", LastName = "ravi", EmailAddress = "sneha@test.com", PhoneNumber = "1234567890", Role = "admin", UserName = "sneka123", Password = "propel@123" };
        var result = _controller.CreateUser(user) as ObjectResult;
        Assert.Equal(result.StatusCode, 201);
    }

    [Fact]
    public async Task CreateUser_Conflict()
    {
        UserController _controller = new UserController(_logger, _service);

        var user1 = new CreateUserDto() { FirstName = "sneha", LastName = "ravi", EmailAddress = Guid.NewGuid().ToString()+"@gmail.com", PhoneNumber = "1234567890", Role = "admin", UserName = Guid.NewGuid().ToString(), Password = "propel@123" };
        var result1 = _controller.CreateUser(user1);
        try
        {
            var user2 = new CreateUserDto() { FirstName = "sneha", LastName = "ravi", EmailAddress = "john@test.com", PhoneNumber = "1234567890", Role = "admin", UserName = "sneka123", Password = "propel@123" };
            var result2 = _controller.CreateUser(user2);
        }
        catch (CustomException ex)
        {
            Assert.Equal("Conflict Occurs", ex.Message);
        }
    }

    [Fact]
    public async Task UpdateUser_Success()
    {
        UserController _controller = new UserController(_logger, _service);
        var user = new UserDto() { Id = Guid.Parse("e37beb34-f626-4c9e-8988-a8bb6ca1483d"), FirstName = "sneha", LastName = "ravi", Email = "sneha@testt.com", PhoneNumber = "1234567890", Role = "admin", UserName = Guid.NewGuid().ToString()};
        var result = _controller.UpdateUser(user.Id, user) as ObjectResult;
        Assert.Equal(result.StatusCode, 204);
    }

    [Fact]
    public async Task UpdateUser_NotFound()
    {
        UserController _controller = new UserController(_logger, _service);
        var user = new UserDto() { Id = Guid.Parse("e57beb34-f626-4c9e-8988-a8bb6ca1487d"), FirstName = "sneha", LastName = "ravi", Email = "sneka@test.com", PhoneNumber = "1234567890", Role = "admin", UserName = "sneka123" };
        try
        {
            _controller.UpdateUser(user.Id, user);
        }
        catch (CustomException ex)
        {
            Assert.Equal("User not found", ex.Message);
        }
    }

    [Theory]
    [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
    public async Task DeleteUser_NotFound(string guid)
    {
        UserController _controller = new UserController(_logger, _service);
        try
        {
            var result = _controller.DeleteUser(Guid.Parse(guid));
        }
        catch (CustomException ex)
        {
            Assert.Equal("User not found", ex.Message);
        }
    }

    [Theory]
    [InlineData("4c066687-6c57-4ef1-94ea-32f3c3786797")]
    public async Task DeleteUser_Success(string guid)
    {
        UserController _controller = new UserController(_logger, _service);
        var result = _controller.DeleteUser(Guid.Parse(guid)) as ObjectResult;
        Assert.Equal(result.StatusCode, 204);
    }

    [Fact]
    public async Task GetUserCount_Success()
    {
        UserController _controller = new UserController(_logger, _service);
        var result = _controller.GetUserCount() as ObjectResult;
        Assert.IsType<CountDto>(result.Value);
    }
}