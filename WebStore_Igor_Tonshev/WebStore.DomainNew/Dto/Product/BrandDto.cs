﻿using System;
using System.Collections.Generic;
using System.Text;
using WebStore.DomainNew.Entities.Base;
using WebStore.DomainNew.Entities.Base.Interfaces;

namespace WebStore.DomainNew.Dto.Product
{
    public class BrandDto : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
