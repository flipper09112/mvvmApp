﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace tabApp.Core.Helpers
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(this string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }
    }
}
