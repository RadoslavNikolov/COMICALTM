﻿namespace Contests.App.Validators
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class ValidateImageAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return false;
            }
            var fileExtension = System.IO.Path.GetExtension(file.FileName);

            var imgExtList = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

            if(!imgExtList.Contains(fileExtension))
            {
                return false;
            }

            if (file.ContentLength > 1 * 1024 * 1024)
            {
                return false;
            }

            return true;
        }
    }
}