using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string DisplayNameFor<TModelItem, TResult>(this IHtmlHelper<IEnumerable<TModelItem>> htmlHelper, Expression<Func<TModelItem, TResult>> expression, IStringLocalizer localizer) {
            return localizer[htmlHelper.DisplayNameFor(expression)];
        }

        public static LocalizedHtmlString DisplayNameFor<TModelItem, TResult>(this IHtmlHelper<IEnumerable<TModelItem>> htmlHelper, Expression<Func<TModelItem, TResult>> expression, IHtmlLocalizer localizer) {  
            return localizer[htmlHelper.DisplayNameFor(expression)];
        }

        public static string DisplayName(this IHtmlHelper htmlHelper, string expression, IStringLocalizer localizer) {
            return localizer[expression];
        }
        public static LocalizedHtmlString DisplayName(this IHtmlHelper htmlHelper, string expression, IHtmlLocalizer localizer) {
            return localizer[expression];
        }
    }
}