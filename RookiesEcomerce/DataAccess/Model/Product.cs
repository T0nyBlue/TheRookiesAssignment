﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? ProductName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public float Price { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int ProductQuantity { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Guid LastModifyBy { get; set; }
        public DateTime LastModifyDate { get; set; } = DateTime.Now;
        public float TotalRating { get; set; } = 0;

        //Foreign Key
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductImg>? ProductImgs { get; set; }
        public List<ProductRating>? ProductRatings { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
