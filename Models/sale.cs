//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beauty_Store.Models
{
    using Beauty_Store.validationAttributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class sale
    {
        [Display(Name ="Sale ID")]
        public int sale_id { get; set; }
        [Display(Name = "User ID")]
        public int userid { get; set; }
        [Display(Name = "Product ID")]
        public int productid { get; set; }
        [Required]
        [Display(Name = "Number of Products to be Delivered")]
        [MinimumValue(ErrorMessage = "Number of Products Cannot be Less Than 1")]
        public int no_of_products { get; set; }
        [Required]
        [Display(Name = "Order Date")]
        public System.DateTime Date { get; set; }
        [Display(Name = "Code")]
        public string sale_code { get; set; }
        [Display(Name = "Is Delivered?")]
        public bool delivery_status { get; set; }
    }
}