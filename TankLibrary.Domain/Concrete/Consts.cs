using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TankLibrary.Domain.Concrete
{
    public static class Consts
    {
        // for tank details and big screen stage menu 
        private static string[] stagesArray = { "All", "World War I", "Interwar", "World War II", "Modern" };
        // for small screen stage menu
        private static string[] stagesArrayAbbrev = { "All", "WWI", "Interwar", "WWII", "Modern" };

        public static string[] GetStagesArray(bool abbreviations = false)
        {
            return abbreviations ? stagesArrayAbbrev : stagesArray;
        }

        public static string GetStage(int? stage)
        {
            if (stage == null || stage < 1 || stage > 4)
            {
                return stagesArray[0];
            }
            else
            {
                return stagesArray[(int)stage];
            }
        }

        public static IEnumerable<SelectListItem> GetStagesSelectItems(int? stage)
        {
            SelectListItem[] items = new SelectListItem[4];
            for (int i = 1; i < stagesArray.Length; i++)
            {
                items[i - 1] = new SelectListItem { Selected = (stage == i), Text = stagesArray[i], Value = i.ToString() };
            }
            return items.ToList();
        }
    }
}
