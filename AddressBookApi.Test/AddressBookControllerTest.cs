using System;
using AutoMapper;
using AddressBookApi.Controllers;
using AddressBookApi.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Contracts;
using AddressBookApi.Dtos;
using System.Threading.Tasks;
using AddressBookApi.Repositories;
using AddressBookApi.Services;
using System.Collections.Generic;
using ExceptionHandler;
using System.Text;
using LoggerService;

namespace AddressBookApi.Test;

public class AddressBookControllerTest
{
    private readonly DbContextOptions<AddressBookDataContext> _contextOptions;
    private readonly IAddressBookService _service;
    private readonly AddressBookController _controller;
    private readonly IMapper _mapper;
    private readonly IAddressBookRepository _repository;
    private readonly ILoggerManager _logger;

    public AddressBookControllerTest()
    {
        _logger = new LoggerManager();
        _contextOptions = ContextFactory.DbContextOptionsInMemory();
        ContextFactory.CreateDataBaseInMemory(_contextOptions);
        _repository = new AddressBookRepository(_logger, new AddressBookDataContext(_contextOptions));
        _service = new AddressBookService(_logger, _repository, _mapper);
        _controller = new AddressBookController(_service, _logger);
    }

    [Fact]
    public async Task GetAddressBookById_Success()
    {
        var result = _controller.GetAddressBookById(Guid.Parse("c893a99f-c93d-4812-88e9-48922b039563")) as ObjectResult;
        Assert.IsType<AddressBookDto>(result.Value);
    }

    [Fact]
    public async Task GetAddressBookById_NotFound()
    {
        try
        {
            _controller.GetAddressBookById(Guid.NewGuid());
        }
        catch (CustomException ex)
        {
            Assert.Equal("Id not found", ex.Message);
        }
    }

    [Fact]
    public async Task GetAddressBook_Success()
    {
        var result = _controller.GetAddressBook() as ObjectResult;
        Assert.IsType<List<AddressBookDto>>(result.Value);
    }

    [Fact]
    public async Task CreateAddressBook_Success()
    {
        var addressBook = new CreateAddressBookDto()
        {
            Name = "John Office Contact Details",
            Emails = new List<EmailDto>() { new EmailDto
            {
                EmailAddress = "John@test.com",
                Type = "Office",
            },},
            Phones = new List<PhoneDto>() { new PhoneDto
            {
                PhoneNumber = "1234567890",
                Type = "Office",
            },},
            Address = new AddressDto()
            {
                Line1 = "zzz",
                Line2 = "xxx",
                City = "yyy",
                Zipcode = "12345",
                State = "TN",
                Type = "Office",
                Country = "India",
            },
            Asset = new AssetDto()
            {
                File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
            }
        };
        var result = _controller.CreateAddressBook(addressBook) as ObjectResult;
        Assert.Equal(result.StatusCode, 201);
    }

    [Fact]
    public async Task CreateAddressBook_Conflict()
    {
        var addressBook1 = new CreateAddressBookDto()
        {
            Name = Guid.NewGuid().ToString(),
            Emails = new List<EmailDto>() { new EmailDto
            {
                EmailAddress = "John@test.com",
                Type = "Office",
            },},
            Phones = new List<PhoneDto>() { new PhoneDto
            {
                PhoneNumber = "1234567890",
                Type = "Office",
            },},
            Address = new AddressDto()
            {
                Line1 = "xxx",
                Line2 = "xxx",
                City = "yyy",
                Zipcode = "12345",
                State = "TN",
                Type = "Office",
                Country = "India",
            },
            Asset = new AssetDto()
            {
                File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
            }
        };

        var result1 = _controller.CreateAddressBook(addressBook1);
        try
        {
            var addressBook2 = new CreateAddressBookDto()
            {
                Name = "John Contact Details",
                Emails = new List<EmailDto>() { new EmailDto
            {
                EmailAddress = "John@test.com",
                Type = "Office",
            },},
                Phones = new List<PhoneDto>() { new PhoneDto
            {
                PhoneNumber = "1234567890",
                Type = "Office",
            },},
                Address = new AddressDto()
                {
                    Line2 = "xxx",
                    City = "yyy",
                    Zipcode = "12345",
                    State = "TN",
                    Type = "Office",
                    Country = "India",
                },
                Asset = new AssetDto()
                {
                    File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
                }
            };
        }
        catch (CustomException ex)
        {
            Assert.Equal("AddressBook Name already exist", ex.Message);
        }
    }

    [Fact]
    public async Task UpdateAddressBook_Success()
    {
        var addressBook = new EditAddressBookDto()
        {
            Id = Guid.Parse("c893a99f-c93d-4812-88e9-48922b039563"),
            Name = "Test Contact Details",
            Emails = new List<EditEmailDto>() { new EditEmailDto
            {
                OldEmailAddress = "Test@test.com",
                NewEmailAddress = "Test@test.com",
                Type = "Office",
            },},
            Phones = new List<EditPhoneDto>() { new EditPhoneDto
            {
                OldPhoneNumber = "1234567890",
                NewPhoneNumber = "1234567890",
                Type = "Office",
            },},
            Address = new AddressDto()
            {
                Line1 = "140",
                Line2 = "xxx",
                City = "yyy",
                Zipcode = "12345",
                State = "TN",
                Type = "Office",
                Country = "India",
            },
            Asset = new AssetDto()
            {
                File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
            }
        };
        var result = _controller.UpdateAddressBook(addressBook.Id, addressBook) as ObjectResult;
        Assert.Equal(result.StatusCode, 204);
    }

    [Fact]
    public async Task UpdateAddressBook_NotFound()
    {
        var addressBook = new EditAddressBookDto()
        {
            Id = Guid.Parse("119472d0-2247-8a8e-89b8-3e6c979285ee"),
            Name = "John Contact Details",
            Emails = new List<EditEmailDto>() { new EditEmailDto
            {
                OldEmailAddress = "John@test.com",
                NewEmailAddress = "John@test.com",
                Type = "Office",
            },},
            Phones = new List<EditPhoneDto>() { new EditPhoneDto
            {
                OldPhoneNumber = "1234567890",
                                NewPhoneNumber = "1234567890",
                Type = "Office",
            },},
            Address = new AddressDto()
            {
                Line1 = "140",
                Line2 = "xxx",
                City = "yyy",
                Zipcode = "12345",
                State = "TN",
                Type = "Office",
                Country = "India",
            },
            Asset = new AssetDto()
            {
                File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
            }
        };

        try
        {
            _controller.UpdateAddressBook(addressBook.Id, addressBook);
        }
        catch (CustomException ex)
        {
            Assert.Equal("AddressBook not found", ex.Message);
        }
    }

    [Theory]
    [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
    public async Task DeleteAddressBook_NotFound(string guid)
    {
        try
        {
            var result = _controller.DeleteAddressBook(Guid.Parse(guid));
        }
        catch (CustomException ex)
        {
            Assert.Equal("AddressBook not found", ex.Message);
        }
    }

    [Theory]
    [InlineData("119172d0-2247-4a8e-89b8-3e6c979285ee")]
    public async Task DeleteAddressBook_Success(string guid)
    {
        AddressBookController _controller = new AddressBookController(_service, _logger);
        var result = _controller.DeleteAddressBook(Guid.Parse(guid)) as ObjectResult;
        Assert.Equal(result.StatusCode, 204);
    }

    [Fact]
    public async Task GetUserCount_Success()
    {
        AddressBookController _controller = new AddressBookController(_service, _logger);
        var result = _controller.GetAddressBookCount() as ObjectResult;
        Assert.IsType<CountDto>(result.Value);
    }
}