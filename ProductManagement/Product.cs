//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProductManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public string ProdID { get; set; }
        public string ProdName { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Origin { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
    }
}