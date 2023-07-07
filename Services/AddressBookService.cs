using AddressBookApi.Entities;
using AddressBookApi.Dtos;
using AutoMapper;
using Contracts;
using ExceptionHandler;
using System.Net;

namespace AddressBookApi.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IAddressBookRepository _addressBookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger, mapper and repository
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="mapper">mapper object</param>
        /// <param name="addressBookRepository">The repository object</param>
        public AddressBookService(ILoggerManager logger, IAddressBookRepository addressBookRepository, IMapper mapper)
        {
            _addressBookRepository = addressBookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Method to create a addressBook for a user
        /// </summary>
        /// <param name="addressBook">AddressBook details to be created</param>
        /// <param name="currentUser">The current logged in user</param>
        /// <returns name= "id">AddressBook id</returns>
        public Guid CreateAddressBook(CreateAddressBookDto addressBook, string currentUser)
        {
            AddressBook checkConflict = CheckConflict(addressBook.Name, null);
            if (checkConflict != null)
            {
                _logger.LogInfo("Conflict occurred");
                throw new ConflictCustomException("Conflict Occurs", "AddressBook Name already exist");
            }
            Guid addressBookId = _addressBookRepository.getUserIdByUserName(currentUser);
            AddressBook addressBookToCreate = new AddressBook()
            {
                Id = Guid.NewGuid(),
                Name = addressBook.Name,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                CreatedBy = addressBookId,
                ModifiedBy = Guid.Empty,
                Active = true,
                Emails = addressBook.Emails.Select(email => new Email
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = email.EmailAddress,
                    Type = email.Type,
                    Active = true,
                }).ToList(),
                Phones = addressBook.Phones.Select(phone => new Phone
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = phone.PhoneNumber,
                    Type = phone.Type,
                    Active = true,
                }).ToList(),
                Address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Line1 = addressBook.Address.Line1,
                    Line2 = addressBook.Address.Line2,
                    City = addressBook.Address.City,
                    Zipcode = addressBook.Address.Zipcode,
                    State = addressBook.Address.State,
                    Type = addressBook.Address.Type,
                    Country = addressBook.Address.Country,
                    Active = true,
                },
                Asset = new Asset()
                {
                    Id = Guid.NewGuid(),
                    File = addressBook.Asset.File,
                    Active = true,
                },
            };
            Guid id = _addressBookRepository.CreateAddressBook(addressBookToCreate);
            if (id == null)
            {
                throw new InternalServerCustomException("Internal Server Error", "Internal Server Error");
            }
            _logger.LogInfo($"AddressBook created for the user with id {id}");
            return id;
        }

        /// <summary>
        /// Method to fetch the total addressBook of a user
        /// </summary>
        /// <returns name= "addressBooksByDto">List of all addressBook of a user</returns>
        public List<AddressBookDto> GetAddressBook()
        {
            List<AddressBook> addressBooks = _addressBookRepository.GetAddressBook();
            List<AddressBookDto> addressBooksByDto = new List<AddressBookDto>();

            foreach (AddressBook addressBook in addressBooks)
            {
                AddressBookDto entity = new AddressBookDto()
                {
                    Id = addressBook.Id,
                    Name = addressBook.Name,
                    Emails = addressBook.Emails.Select(email => new EmailDto
                    {
                        EmailAddress = email.EmailAddress,
                        Type = email.Type
                    }).ToList(),
                    Address = new AddressDto()
                    {
                        Line1 = addressBook.Address.Line1,
                        Line2 = addressBook.Address.Line2,
                        City = addressBook.Address.City,
                        Zipcode = addressBook.Address.Zipcode,
                        State = addressBook.Address.State,
                        Type = addressBook.Address.Type,
                        Country = addressBook.Address.Country
                    },
                    Phones = addressBook.Phones.Select(phone => new PhoneDto
                    {
                        PhoneNumber = phone.PhoneNumber,
                        Type = phone.Type
                    }).ToList(),
                    Asset = new AssetDto()
                    {
                        File = addressBook.Asset.File
                    }
                };
                addressBooksByDto.Add(entity);
            }
            _logger.LogInfo("AddressBook details fetched successfully");
            return addressBooksByDto;
        }

        /// <summary>
        /// Method to get the addressBook detail by providing the address book and user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns name="addressBookByDto">AddressBook detail of the provided id</returns>
        public AddressBookDto GetAddressBookById(Guid id)
        {
            AddressBook addressBook = _addressBookRepository.GetAddressBookById(id);
            if (addressBook is null)
            {
                _logger.LogInfo($"The addressbook id {id} was not found");
                throw new NotFoundCustomException("Id not found", "Id not found"); ;
            }
            AddressBookDto addressBookByDto = new AddressBookDto()
            {
                Id = addressBook.Id,
                Name = addressBook.Name,
                Emails = addressBook.Emails.Select(email => new EmailDto
                {
                    EmailAddress = email.EmailAddress,
                    Type = email.Type
                }).ToList(),
                Address = new AddressDto()
                {
                    Line1 = addressBook.Address.Line1,
                    Line2 = addressBook.Address.Line2,
                    City = addressBook.Address.City,
                    Zipcode = addressBook.Address.Zipcode,
                    State = addressBook.Address.State,
                    Type = addressBook.Address.Type,
                    Country = addressBook.Address.Country
                },
                Phones = addressBook.Phones.Select(phone => new PhoneDto
                {
                    PhoneNumber = phone.PhoneNumber,
                    Type = phone.Type
                }).ToList(),
                Asset = new AssetDto()
                {
                    File = addressBook.Asset.File
                }
            };
            _logger.LogInfo("AddressBook detail fetched successfully");
            return addressBookByDto;
        }

        /// <summary>
        /// Method to update the addressbook detail by providing the id
        /// </summary>
        /// <param name="id">Id of the addressbook</param>
        /// <param name="addressbook">addressbook detail of the id</param>
        /// <param name="currentUser">addressbook detail of the id</param>
        /// <returns name="response">response whether the addressbook is updated or not</returns>
        public bool UpdateAddressBook(Guid id, EditAddressBookDto addressBook, string currentUser)
        {
            if (id != addressBook.Id)
            {
                _logger.LogInfo($"Mismatch occurred in the {id}");
                throw new BadRequestCustomException("Bad Request", "Bad Request");
            }
            AddressBook oldAddressBookData = _addressBookRepository.GetAddressBookById(id);
            if (oldAddressBookData is null)
            {
                _logger.LogInfo($"The addressbook id {id} was not found");
                throw new NotFoundCustomException("AddressBook not found", "AddressBook not found");
            }
            AddressBook checkConflict = CheckConflict(addressBook.Name, id);
            if (checkConflict != null)
            {
                _logger.LogInfo("Conflict Occurred");
                throw new ConflictCustomException("Conflict Occurs", "AddressBook Name already exist");
            }
            Guid addressBookId = _addressBookRepository.getUserIdByUserName(currentUser);
            oldAddressBookData.Name = addressBook.Name;
            oldAddressBookData.ModifiedBy = addressBookId;
            oldAddressBookData.ModifiedOn = DateTime.UtcNow;
            oldAddressBookData.Address.Line1 = addressBook.Address.Line1;
            oldAddressBookData.Address.Line2 = addressBook.Address.Line2;
            oldAddressBookData.Address.City = addressBook.Address.City;
            oldAddressBookData.Address.State = addressBook.Address.State;
            oldAddressBookData.Address.Zipcode = addressBook.Address.Zipcode;
            oldAddressBookData.Address.Type = addressBook.Address.Type;
            oldAddressBookData.Address.Country = addressBook.Address.Country;
            oldAddressBookData.Asset.File = addressBook.Asset.File;
            List<Phone> originalPhoneNumbers = oldAddressBookData.Phones.ToList();
            List<Phone> modifiedPhoneNumbers = new List<Phone>();
            foreach (EditPhoneDto phone in addressBook.Phones)
            {
                // if oldphonenumber null means, the incoming phone number is newly added
                if (phone.OldPhoneNumber != null)
                {
                    Phone newPhoneNumber = originalPhoneNumbers.Where(p => p.PhoneNumber == phone.OldPhoneNumber).First();
                    // if oldphonenumber is there, with the new phonenumber, it has to be modified
                    if (phone.NewPhoneNumber != null)
                    {
                        newPhoneNumber.PhoneNumber = phone.NewPhoneNumber;
                        newPhoneNumber.Type = phone.Type;
                    }
                    else
                    {
                        // if there is only oldphonenumber, but there is no new phonenumber, it has to be deleted
                        newPhoneNumber.Active = false;
                    }
                    modifiedPhoneNumbers.Add(newPhoneNumber);
                }
                else
                {
                    Phone newPhoneNumber = new Phone()
                    {
                        Id = Guid.NewGuid(),
                        PhoneNumber = phone.NewPhoneNumber,
                        Type = phone.Type
                    };
                    modifiedPhoneNumbers.Add(newPhoneNumber);
                }
            }
            oldAddressBookData.Phones = modifiedPhoneNumbers;

            List<Email> originalEmails = oldAddressBookData.Emails.ToList();
            List<Email> modifiedEmail = new List<Email>();
            foreach (EditEmailDto email in addressBook.Emails)
            {
                if (email.OldEmailAddress != null)
                {
                    Email newEmail = originalEmails.Where(e => e.EmailAddress == email.OldEmailAddress).First();
                    if (email.NewEmailAddress != null)
                    {
                        newEmail.EmailAddress = email.NewEmailAddress;
                        newEmail.Type = email.Type;
                    }
                    else
                    {
                        newEmail.Active = false;
                    }
                    modifiedEmail.Add(newEmail);
                }
                else
                {
                    Email newEmail = new Email()
                    {
                        Id = Guid.NewGuid(),
                        EmailAddress = email.NewEmailAddress,
                        Type = email.Type
                    };
                    modifiedEmail.Add(newEmail);
                }
            }
            oldAddressBookData.Phones = modifiedPhoneNumbers;
            bool response = _addressBookRepository.UpdateAddressBook(addressBook.Id, oldAddressBookData);
            _logger.LogInfo($"The addressbook of {id} updated successfully");

            return response;
        }

        /// <summary>
        /// Method to delete the addressbook by providing the id
        /// </summary>
        /// <param name="id">Id of the addressbook</param>
        /// <returns name="response">response whether the addressbook is deleted or not</returns>
        public bool DeleteAddressBook(Guid id)
        {
            AddressBook addressBookData = _addressBookRepository.GetAddressBookById(id);
            if (addressBookData is null)
            {
                _logger.LogInfo($"The addressbook id {id} was not found");
                throw new NotFoundCustomException("AddressBook not found", "AddressBook not found");
            }
            bool response = _addressBookRepository.DeleteAddressBook(id);
            _logger.LogInfo($"The addressbook of {id} deleted successfully");
            return response;
        }

        /// <summary>
        /// Method to get the number of addressbook
        /// </summary>
        /// <returns name="count">total number of addressbook</returns>
        public CountDto GetAddressBookCount()
        {
            int count = _addressBookRepository.GetAddressBookCount();
            CountDto countByDto = new CountDto()
            {
                Count = count
            };
            _logger.LogInfo($"The total number of addressbook were {count}");
            return countByDto;
        }

        /// <summary>
        /// Method to find conflict in addressbook
        /// </summary>
        public AddressBook CheckConflict(string addressBookName, Guid? id)
        {
            return _addressBookRepository.CheckConflict(addressBookName, id);
        }
    }
}