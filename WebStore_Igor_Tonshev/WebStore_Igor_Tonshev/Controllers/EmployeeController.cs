﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;
using WebStore.DomainNew.Models;
using WebStore.DomainNew.Model;

namespace WebStore_Igor_Tonshev.Controllers
{
    [Route("users")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesData _employeesData;

        public EmployeeController(IEmployeesData employeesData)
        {
            _employeesData = employeesData;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_employeesData.GetAll());
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            var employee = _employeesData.GetById(id);

            if (ReferenceEquals(employee, null))
                return NotFound();//возвращаем результат 404 Not Found

            return View(employee);
        }

        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            EmployeeView model;
            if (id.HasValue)
            {
                model = _employeesData.GetById(id.Value);
                if (ReferenceEquals(model, null))
                    return NotFound();// возвращаем результат 404 Not Found
            }
            else
            {
                model = new EmployeeView();
            }
            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public IActionResult Edit(EmployeeView model)
        {
            if (model.Age < 18 && model.Age > 75)
            {
                ModelState.AddModelError("Age", "Ошибка возраста!");
            }

            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    _employeesData.UpdateEmployee(model.Id, model);
                }
                else
                {
                    _employeesData.AddNew(model);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("delete/{id}")]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public IActionResult Delete(int id)
        {
            _employeesData.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}