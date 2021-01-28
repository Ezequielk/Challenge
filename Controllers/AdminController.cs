using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Challenge.Filters;
using Challenge.Models;
using Challenge.Models.ViewModels;

namespace Challenge.Controllers
{
    [AuthorizeAdmin()]
    public class AdminController : Controller
    {
        public ActionResult ListTeachers()
        {
            // Lista todos los profesores de la base de datos
            try
            {
                List<ListTeacherViewModel> lst_teachers;
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    // Obtengo la lista de todos los profes
                    lst_teachers = (from teacher in db.Teachers
                                    select new ListTeacherViewModel
                                    {
                                        id_teacher = teacher.id_teacher,
                                        first_name = teacher.first_name,
                                        last_name = teacher.last_name,
                                        active = teacher.active
                                    }).ToList();
                }
                return View(lst_teachers);
            }
            catch
            {
                TempData["Alert"] = "An error occurred, try again later";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult NewTeacher()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewTeacher(ListTeacherViewModel model)
        {
            // Agrega un nuevo profesor al sistema
            try
            {
                if (ModelState.IsValid) // Checkea los DataAnnotations de ListTeacherViewModel, o sea los [Required]
                {
                    // Si es un modelo válido, agregar el nuevo profe a la base
                    using (Models.DBContainer db = new Models.DBContainer())
                    {
                        Teacher teacher = new Teacher();
                        teacher.first_name = model.first_name;
                        teacher.last_name = model.last_name;
                        teacher.active = model.active;

                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListTeachers", "Admin");
                }
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public ActionResult EditTeacher(int id)
        {
            // Muestra los datos del profe a editar
            try
            {
                ListTeacherViewModel model = new ListTeacherViewModel();
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    Teacher teacher = db.Teachers.Find(id);
                    if (teacher == null)
                    {
                        TempData["Alert"] = "An error occurred, try again later";
                        return RedirectToAction("ListTeachers");
                    }
                    model.id_teacher = teacher.id_teacher;
                    model.first_name = teacher.first_name;
                    model.last_name = teacher.last_name;
                    model.active = teacher.active;
                }
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        [HttpPost]
        public ActionResult EditTeacher(ListTeacherViewModel model)
        {
            // Edita al profe y lo persiste en la base de datos
            try
            {
                if (ModelState.IsValid)
                {
                    using (Models.DBContainer db = new Models.DBContainer())
                    {
                        Teacher teacher = db.Teachers.Find(model.id_teacher);
                        if (teacher == null)
                        {
                            TempData["Alert"] = "An error occurred, try again later";
                            return RedirectToAction("ListTeachers");
                        }
                        teacher.first_name = model.first_name;
                        teacher.last_name = model.last_name;
                        teacher.active = model.active;

                        db.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListTeachers", "Admin");
                }
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public ActionResult DeleteTeacher(int id)
        {
            // Elimina al profesor del sistema
            try
            {
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    Teacher teacher = db.Teachers.Find(id);
                    if (teacher == null)
                    {
                        TempData["Alert"] = "An error occurred, try again later";
                        return RedirectToAction("ListTeachers");
                    }
                    db.Teachers.Remove(teacher);
                    db.SaveChanges();
                }
                return RedirectToAction("ListTeachers", "Admin");
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        public ActionResult ListSubjects()
        {
            // Lista todas las materias del sistema
            try
            {
                List<ListSubjectViewModelAdmin> lst_subjects;
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    // Obtengo la lista de todas las materias
                    lst_subjects = (from subject in db.Subjects
                                    select new ListSubjectViewModelAdmin
                                    {
                                        id_subject = subject.id_subject,
                                        name = subject.name,
                                        description = subject.desc,
                                        time_from = subject.time_from,
                                        time_to = subject.time_to,
                                        capacity = subject.capacity,
                                        id_teacher = subject.id_teacher
                                    }).ToList();
                }
                return View(lst_subjects);
            }
            catch
            {
                TempData["Alert"] = "An error occurred, try again later";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult NewSubject()
        {
            PopulateDropDownList();
            return View();
        }

        private void PopulateDropDownList()
        {
            // Rellena el DropDownList con los nombres de los profesores
            try
            {
                List<DropDownListTeacherViewModel> lst_teachers;
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    lst_teachers = (from teacher in db.Teachers
                                    select new DropDownListTeacherViewModel
                                    {
                                        id_teacher = teacher.id_teacher,
                                        name = teacher.first_name + " " + teacher.last_name
                                    }).ToList();
                }
                List<SelectListItem> lst_items = lst_teachers.ConvertAll(i =>
                {
                    return new SelectListItem()
                    {
                        Text = i.name,
                        Value = i.id_teacher.ToString(),
                        Selected = false
                    };
                });
                ViewBag.items = lst_items;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        [HttpPost]
        public ActionResult NewSubject(ListSubjectViewModelAdmin model)
        {
            // Crea una nueva materia y la guarda en el sistema
            try
            {
                if (ModelState.IsValid)
                {
                    using (Models.DBContainer db = new Models.DBContainer())
                    {
                        Subject subject = new Subject();
                        subject.name = model.name;
                        subject.desc = model.description;
                        subject.capacity = model.capacity;
                        subject.time_from = model.time_from;
                        subject.time_to = model.time_to;
                        subject.id_teacher = model.id_teacher;

                        if(subject.time_to <= subject.time_from)
                        {
                            // Horario no válido
                            ViewData["Error"] = "Ending time must be strictly greater than starting time";
                            PopulateDropDownList();
                            return View(model);
                        }

                        db.Subjects.Add(subject);
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListSubjects", "Admin");
                }
                // Si el modelo no es válido, tengo que volver a la misma pantalla, con
                // lo cual hay que rellenar de nuevo el DropDownList
                PopulateDropDownList();
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        public ActionResult EditSubject(int id)
        {
            // Carga los datos de la materia a modificar
            try
            {
                ListSubjectViewModelAdmin model = new ListSubjectViewModelAdmin();
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    Subject subject = db.Subjects.Find(id);
                    if (subject == null)
                    {
                        TempData["Alert"] = "An error occurred, try again later";
                        return RedirectToAction("ListSubjects");
                    }
                    model.id_subject = subject.id_subject;
                    model.name = subject.name;
                    model.description = subject.desc;
                    model.capacity = subject.capacity;
                    model.time_from = subject.time_from;
                    model.time_to = subject.time_to;
                    model.id_teacher = subject.id_teacher;
                }
                PopulateDropDownList();
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        [HttpPost]
        public ActionResult EditSubject(ListSubjectViewModelAdmin model)
        {
            // Modifica la materia y la persiste en la base de datos
            try
            {
                if (ModelState.IsValid)
                {
                    using (Models.DBContainer db = new Models.DBContainer())
                    {
                        Subject subject = db.Subjects.Find(model.id_subject);
                        if (subject == null)
                        {
                            TempData["Alert"] = "An error occurred, try again later";
                            return RedirectToAction("ListSubjects");
                        }
                        subject.name = model.name;
                        subject.desc = model.description;
                        subject.capacity = model.capacity;
                        subject.time_from = model.time_from;
                        subject.time_to = model.time_to;
                        subject.id_teacher = model.id_teacher;

                        if (subject.time_to <= subject.time_from)
                        {
                            // Horario no válido
                            ViewData["Error"] = "Ending time must be strictly greater than starting time";
                            PopulateDropDownList();
                            return View(model);
                        }

                        db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListSubjects", "Admin");
                }
                // Si el modelo no es válido, tengo que volver a la misma pantalla, con
                // lo cual hay que rellenar de nuevo el DropDownList
                PopulateDropDownList();
                return View(model);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public ActionResult DeleteSubject(int id)
        {
            // Elimina la materia cuyo id_subject es id
            try
            {
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    Subject subject = db.Subjects.Find(id);
                    if (subject == null)
                    {
                        TempData["Alert"] = "An error occurred, try again later";
                        return RedirectToAction("ListSubjects");
                    }
                    db.Subjects.Remove(subject);
                    db.SaveChanges();
                }
                return RedirectToAction("ListSubjects", "Admin");
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
    }
}