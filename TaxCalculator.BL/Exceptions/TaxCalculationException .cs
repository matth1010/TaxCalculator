using System;

namespace TaxCalculator.BL.Exceptions
{
    public class TaxCalculatorException : Exception
    {
        public TaxCalculatorException(string message) : base(message)
        {
        }
    }
}