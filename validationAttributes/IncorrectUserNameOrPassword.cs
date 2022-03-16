using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Beauty_Store.Models;

namespace Beauty_Store.validationAttributes
{
    public class IncorrectUserNameOrPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int? || value is string)
            {
                var v1 = new int?();
                string v2 = null;
                using (userEntities db = new userEntities())
                {
                    var list = db.users.ToList();
                    if (value is int?)
                    {
                        v1 = value as int?;

                    }
                    if (value is string)
                    {
                        v2 = value as string;
                    }

                    foreach (user user in list)
                    {
                        if (user.user_id == v1 && user.user_name.Equals(v2))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            return true;
        }
    }
}