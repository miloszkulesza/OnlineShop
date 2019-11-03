﻿using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class OrderDetailsViewModel
    {
        public List<OrderPosition> OrderPositions { get; set; }
        public Order Order { get; set; }
    }
}
