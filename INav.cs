using System;

namespace DataSupport
{
    public interface INav
    {
        decimal Amount { get; set; }
        DateTime NavDate { get; set; }
    }
}