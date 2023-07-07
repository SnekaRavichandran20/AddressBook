using AddressBookApi.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Entities
{
    public static class DataBaseMigration
    {
        public static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            AddressBookDataContext context = serviceProvider.GetRequiredService<AddressBookDataContext>();
            context.Database.Migrate();
            context.SaveChangesAsync(true);
        }
    }
}