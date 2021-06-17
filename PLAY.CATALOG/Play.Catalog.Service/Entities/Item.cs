using System;
using Play.Common;

namespace Play.Catalog.Service.Entities
{
    public class Item : IEntity
    {
        //public Item(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate)
        //{
        //    this.Id = Id;
        //    this.Name = Name;
        //    this.Description = Description;
        //    this.Price = Price;
        //    this.CreatedDate = CreatedDate;
        //}

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset CreatedDate { get; set; }



    }
}
