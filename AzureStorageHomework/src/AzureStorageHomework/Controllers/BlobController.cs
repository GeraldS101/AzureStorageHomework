using AzureStorageHomework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureStorageHomework.Controllers
{
    public class BlobController : Controller
    {
        IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Index()
        {
            var blobList = await _blobService.GetBlobList();
            return View(blobList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file1)
        {
            if (file1 != null)
            {
                if (file1.Length > 5242880)
                {
                    ModelState.AddModelError("", "File cannot be greater than 5MB.");
                }
                else
                {
                    var success = await _blobService.CreateBlob(file1);
                    if (!success)
                        ModelState.AddModelError("", "Error in creating blob.");
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit(string name)
        {
            ViewBag.name = name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile file1, string name)
        {
            if (file1 != null)
            {
                var origFileExt = System.IO.Path.GetExtension(name);
                var newFileExt = System.IO.Path.GetExtension(file1.FileName);
                if (file1.Length > 5242880)
                {
                    ModelState.AddModelError("", "File cannot be greater than 5MB.");
                }
                else if (newFileExt.ToLower() != origFileExt.ToLower())
                {
                    ModelState.AddModelError("", origFileExt + " to " + newFileExt + ". File extension does not match with the original. ");
                }
                else
                {
                    var success = await _blobService.UpdateBlob(file1, name);
                    if (!success)
                        ModelState.AddModelError("", "Error in updating blob.");
                }

                return RedirectToAction("Index");
            }

            ViewBag.name = name;
            return View();
        }

        [HttpGet]
        public ViewResult Delete(string name)
        {
            ViewBag.name = name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlob(string name)
        {
            var success = await _blobService.DeleteBlob(name);
            return RedirectToAction("Index");
        }
    }
}
