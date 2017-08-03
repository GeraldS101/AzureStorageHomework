using AzureStorageHomework.Models;
using AzureStorageHomework.Models.Entities;
using AzureStorageHomework.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Controllers
{
    public class TableController : Controller
    {
        ITableService _tableStorageService;

        public TableController(ITableService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _tableStorageService.GetEmployeeList();
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeModel employee)
        {
            try {
                var success = await _tableStorageService.CreateEntry(employee);

                return RedirectToAction("Index");
            }
            catch
            {

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var employee = await _tableStorageService.GetEmployeeDetails(id);
                return View(employee);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeModel employee)
        {
            try {
                var success = await _tableStorageService.UpdateEntry(employee);

                return RedirectToAction("Index");
            }
            catch
            {

            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var employee = await _tableStorageService.GetEmployeeDetails(id);
                return View(employee);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEntry(string id)
        {
            var success = await _tableStorageService.DeleteEntry(id);
            return RedirectToAction("Index");
        }
    }
}
