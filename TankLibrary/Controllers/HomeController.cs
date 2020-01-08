using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TankLibrary.Domain.Abstract;
using TankLibrary.Domain.Entities;
using TankLibrary.Models;

namespace TankLibrary.Controllers
{
    public class HomeController : Controller
    {
        private ITankRepository repository;
        public int PageSize = 10;
        private int defaultIdMax = 19;
        private int allowedRecordsMax = 18;

        public HomeController(ITankRepository tankRepository)
        {
            this.repository = tankRepository;
            var reader = new AppSettingsReader();
            PageSize = (int)reader.GetValue("rowsPerPage", typeof(int));
            defaultIdMax = (int)reader.GetValue("defaultIdMax", typeof(int));
            allowedRecordsMax = (int)reader.GetValue("allowedRecordsMax", typeof(int));
        }

        public ViewResult Index(int? stage, int page = 1, int curindex = 0, int tankId = 0, int showReset = 0)
        {
            ViewData["ShowReset"] = showReset;

            TankListViewModel model;

            // tankId > 0 only after an add action has done
            if (tankId > 0)
            {
                //locate the current page and current index to the new added tank (last page + last index)
                //if using a stage fileter, change the filter to this tank's stage 
                Tank tank = repository.Tanks.Where(t => t.Id == tankId).FirstOrDefault();
                PagingInfo pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = (stage == null ? repository.Tanks.Count() :
                            repository.Tanks.Where(t => t.Stage == tank.Stage).Count()),
                    CurrentSelIndex = curindex
                };
                //the Id of new tank must have the biggest number, so must be at last and on the last page
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
                pagingInfo.CurrentSelIndex = pagingInfo.TotalItems - (pagingInfo.TotalPages - 1) * PageSize - 1;

                model = new TankListViewModel
                {
                    Tanks = repository.Tanks
                    .Where(t => stage == null || t.Stage == tank.Stage)
                    .OrderBy(t => t.Id)
                    .Skip((pagingInfo.TotalPages - 1) * PageSize)
                    .Take(PageSize),
                    PageInfo = pagingInfo,
                    CurrentStage = (stage == null ? stage : tank.Stage)
                };
            }
            else
            {
                model = new TankListViewModel
                {
                    Tanks = repository.Tanks
                    .Where(t => stage == null || t.Stage == stage)
                    .OrderBy(t => t.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                    PageInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = (stage == null ? repository.Tanks.Count() :
                            repository.Tanks.Where(t => t.Stage == stage).Count()),
                        CurrentSelIndex = curindex
                    },
                    CurrentStage = stage
                };

                // Check whether the curidex and page valid after a delete operation
                // Get current page size
                int curPageCount = (page * PageSize <= model.PageInfo.TotalItems ? PageSize :
                    model.PageInfo.TotalItems - (page - 1) * PageSize);
                model.PageInfo.CurrentSelIndex = (model.PageInfo.CurrentSelIndex < curPageCount ?
                    model.PageInfo.CurrentSelIndex : model.PageInfo.CurrentSelIndex - 1);
                if (model.PageInfo.CurrentSelIndex < 0)
                {
                    model.PageInfo.CurrentSelIndex = 0;
                    if (page > 1) model.PageInfo.CurrentPage--;
                }
            }

            return View(model);
        }

        public ActionResult ImgAndDesc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var tanks = repository.Tanks.Where(x => x.Id == id);
            if (tanks.Count() == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Tank tank = tanks.ElementAt(0);

            return PartialView("ImageAndDescription", tank);
        }

        public ActionResult Detail(int? id, int? liststage, int page, int curindex, bool fromEdit = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var tanks = repository.Tanks.Where(x => x.Id == id);
            if (tanks.Count() == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Tank tank = tanks.ElementAt(0);

            ViewBag.CurrentStage = liststage;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentIndex = curindex;
            ViewBag.DefaultIdMax = defaultIdMax;
            ViewBag.FromEdit = fromEdit;
            return View(tank);
        }

        [HttpGet]
        public ActionResult Edit(int? id, int? liststage, int page, int curindex)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Tank tank;
            if (id == -1) // Add a new tank
            {
                if (repository.Tanks.Count() >= allowedRecordsMax)
                {
                    return RedirectToAction("Index", new { showReset = -1, page = page });
                }
                var newStage = liststage ?? 0;
                tank = new Tank { Id = 0, Stage = newStage };
            }
            else // Edit a tank
            {
                var tanks = repository.Tanks.Where(x => x.Id == id);
                if (tanks.Count() == 0)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }

                tank = tanks.ElementAt(0);
            }

            if (tank.Id > 0 && tank.Id <= defaultIdMax)
            {
                return RedirectToAction("Detail", new { id = tank.Id, liststage = liststage, page = page, curindex = curindex, fromEdit = true });
            }
            ViewBag.CurrentStage = liststage;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentIndex = curindex;
            return View(tank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tank tank, int? liststage, int page, int curindex)
        {
            if (ModelState.IsValid)
            {
                if (saveImageFile(tank))
                {
                    if (tank.Id == -1)
                    {
                        tank = repository.Add(tank);
                        return RedirectToAction("Index", "Home", new { stage = liststage, page = page, curindex = curindex, tankId = tank.Id });
                    }
                    else
                    {
                        repository.Update(tank);
                        return RedirectToAction("Index", "Home", new { stage = liststage, page = page, curindex = curindex });
                    }
                }
            }

            ViewBag.CurrentStage = liststage;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentIndex = curindex;
            return View(tank);
        }

        private bool saveImageFile(Tank tank)
        {
            foreach (string upload in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[upload];
                if (!(file != null && file.ContentLength > 0)) continue;

                // the acceptable max size of the image file: 3MB
                int MaxCLength = 3;
                int MaxContentLength = 1024 * 1024 * MaxCLength; //3 MB
                if (file.ContentLength > MaxContentLength)
                {
                    ModelState.AddModelError("File", "Please upload a file with size no larger than " + MaxCLength + " MB");
                    return false;
                }

                // the acceptable image types
                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };

                if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ModelState.AddModelError("File", "Please upload a file of type: " + string.Join(", ", AllowedFileExtensions));
                    return false;
                }
                else
                {
                    //var filePath = tank.ImagePath; //fixed path right now, value should be '/Content/data/'
                    var fileName = tank.ImageFile;
                    var path = Path.Combine(Server.MapPath("~/Content/data"), fileName);
                    file.SaveAs(path);
                    ModelState.Clear();
                    return true;
                }
            }

            // not required property
            return true;
        }

        [HttpGet]
        public ActionResult Delete(int? id, int? liststage, int page, int curindex)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Tank tank = repository.Tanks.First(t => t.Id == id);
            if (tank == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentStage = liststage;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentIndex = curindex;
            ViewBag.DefaultIdMax = defaultIdMax;
            return View(tank);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? liststage, int page, int curindex)
        {
            repository.Delete(id);

            return RedirectToAction("Index", "Home", new { stage = liststage, page = page, curindex = curindex });
        }

        public ActionResult About()
        {
            ViewBag.Message = "TANK LIBRARY is a simple application created with C#, ASP.NET, MVC5, HTML5, CSS3 and JAVASCRIPT.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Frank Li";

            return View();
        }

        public ActionResult ResetData()
        {
            int delCount = repository.ResetData(defaultIdMax);
            return RedirectToAction("Index", new { showReset = delCount });
        }
    }
}