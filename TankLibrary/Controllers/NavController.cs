using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TankLibrary.Domain.Abstract;
using TankLibrary.Domain.Concrete;

namespace TankLibrary.Controllers
{
    public class NavController : Controller
    {
        private ITankRepository repository;

        public NavController(ITankRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(int? stage = null, bool abbreviationsMenu = false)
        {
            ViewBag.SelectedStage = stage;
            //IEnumerable<KeyValuePair<string, int>> stages = repository.Tanks
            //    .Select(x => x.Stage)
            //    .Distinct()
            //    .OrderBy(x => x)
            //    .Select(x => new KeyValuePair<string, int>(Consts.StagesArray[x], x));
            string[] stagesArray = Consts.GetStagesArray(abbreviationsMenu);
            IEnumerable<KeyValuePair<string, int>> stages = stagesArray.ToList()
                .Where((Value, Index) => Index > 0)
                .Select((Value, Index) => new KeyValuePair<string, int>(Value, Index + 1));

            return PartialView(stages);
        }
    }
}