using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Beauty_Store.validationAttributes
{
    public class MinimumValue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is int?)
            {
                var v = value as int?;
                if (v.Value >= 1)
                {
                    return true;
                }
                if (v.Value < 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}