using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TankLibrary.Models;

namespace TankLibrary.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfo pagingInfo, Func<int, string> pageUrl, int pageLinkCount = 11)
        {
            StringBuilder result = new StringBuilder();
            // Add prev/< page link
            appendOnePageLink(result, pagingInfo.CurrentPage > 1 ? pagingInfo.CurrentPage - 1 : 1,
                pageLinkCount > 7 ? "prev" : "<", pagingInfo, pageUrl, pagingInfo.CurrentPage > 1, false);

            // Add standard page link, process the situation of totle pages more than pageLinkCount
            // Calculate the first standard page link
            int startPage = 1;
            if(pagingInfo.TotalPages > pageLinkCount)
            {
                if(pagingInfo.CurrentPage >= pageLinkCount / 2 + 1)
                {
                    if(pagingInfo.CurrentPage + pageLinkCount / 2 <= pagingInfo.TotalPages)
                    {
                        startPage = pagingInfo.CurrentPage - pageLinkCount / 2;
                    }
                    else
                    {
                        startPage = pagingInfo.TotalPages - pageLinkCount + 1;
                    }
                }
                else
                {
                    startPage = 1;
                }
            } 
            // Add standard page link
            for (int i = startPage; i <= (pagingInfo.TotalPages < pageLinkCount ? pagingInfo.TotalPages : startPage + pageLinkCount - 1); i++)
            {
                appendOnePageLink(result, i, "", pagingInfo, pageUrl, i != pagingInfo.CurrentPage, true);
            }
            // Add Next/> page link
            appendOnePageLink(result,
                pagingInfo.CurrentPage < pagingInfo.TotalPages ? pagingInfo.CurrentPage + 1 : pagingInfo.TotalPages,
                pageLinkCount > 7 ? "next" : ">", pagingInfo, pageUrl, pagingInfo.CurrentPage < pagingInfo.TotalPages, false);

            return MvcHtmlString.Create(result.ToString());

        }

        private static void appendOnePageLink(StringBuilder result, int page, string pageName,
            PagingInfo pagingInfo, Func<int, string> pageUrl, bool bEnabled, bool bPage)
        {
            TagBuilder tag = new TagBuilder(bEnabled ? "a" : "span");
            if(bEnabled)
            {
                tag.MergeAttribute("href", pageUrl(page));
            }
            tag.InnerHtml = pageName == "" ? page.ToString() : pageName;
            if (page == pagingInfo.CurrentPage && bPage)
            {
                tag.AddCssClass("selected");
                tag.AddCssClass("btn-primary");
            }
            tag.AddCssClass("btn btn-default");
            if(!bEnabled)
            {
                tag.AddCssClass("disabled");
            }
            result.Append(tag.ToString());
        }
    }
}