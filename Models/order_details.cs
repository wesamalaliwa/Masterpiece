//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyMasterOrange.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_details
    {
        public int order_detail_id { get; set; }
        public string order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    
        public virtual order order { get; set; }
        public virtual product product { get; set; }
    }
}
