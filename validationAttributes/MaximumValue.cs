using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Beauty_Store.validationAttributes
{
    public class MaximumValue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is int?)
            {
                var v = value as int?;
                if (v.Value <= 50)
                {
                    return true;
                }
                if (v.Value > 50)
                {
                    return false;
                }
            }

            return true;
        }
    }
}