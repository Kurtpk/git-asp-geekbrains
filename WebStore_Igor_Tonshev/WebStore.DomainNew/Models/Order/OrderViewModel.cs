﻿using System.ComponentModel.DataAnnotations;

namespace WebStore.DomainNew.Models.Order
{
    public class OrderViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
