//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolsApp.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportSalariesTmp
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int workShift { get; set; }
        public double sumDayWorking { get; set; }
        public double sumOverTimeHours { get; set; }
        public double sumBeLate { get; set; }
        public double furlough { get; set; }
        public double absenced { get; set; }
        public decimal Salaries { get; set; }
        public decimal allowance { get; set; }
        public decimal allowanceOther { get; set; }
        public decimal allowanceOverTimeHours { get; set; }
        public decimal Bonus { get; set; }
        public decimal advancePayment { get; set; }
        public decimal healthInsurance { get; set; }
        public decimal personalIncomeTax { get; set; }
        public decimal totalSalaries { get; set; }
        public decimal actualAmountReceived { get; set; }
        public string UserCreate { get; set; }
        public System.DateTime DateCreate { get; set; }
        public System.DateTime dateTime { get; set; }
    }
}
