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
    [AuthorizeStudent()]
    public class StudentController : Controller
    {
        
        public ActionResult ListSubjects()
        {
            // Muestra la lista de materias para que el estudiante se inscriba
            try
            {
                List<ListSubjectViewModelStudent> lst_subjects;
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    int id_student = ((Student)Session["User"]).id_student;

                    // Obtengo la lista de las materias a las que está inscripto el estudiante
                    List<int> lst_student_subjects = (from sub_stu in db.Subjects_Students
                                                      where sub_stu.id_student == id_student
                                                      select sub_stu.id_subject).ToList();


                    // Obtengo la lista de todas las materias en las que el estudiante no esté inscripto,
                    // filtrando las que sean de un profesor no activo y ordenándolas alfabéticamente
                    lst_subjects = (from subject in db.Subjects
                                    join teacher in db.Teachers
                                    on subject.id_teacher equals teacher.id_teacher
                                    where teacher.active == true &&
                                    !lst_student_subjects.Contains(subject.id_subject)
                                    orderby subject.name
                                    select new ListSubjectViewModelStudent
                                    {
                                        id_subject = subject.id_subject,
                                        name = subject.name,
                                        time_from = subject.time_from,
                                        time_to = subject.time_to,
                                        capacity = subject.capacity,
                                        teacher_name = teacher.last_name + ", " + teacher.first_name
                                    }).ToList();

                    // Ahora que tengo la lista de todas las materias a mostrar en el view,
                    // relleno el campo remaining_places a cada una
                    foreach(var subject in lst_subjects)
                    {
                        subject.remaining_places = subject.capacity - GetInscriptionsCount(subject.id_subject);
                    }

                    // Ahora que cada materia tiene su cupo calculado, filtro y saco las 
                    // materias que no tengan cupo
                    lst_subjects = (from item in lst_subjects
                                    where item.remaining_places != 0
                                    select item).ToList();
                }
                return View(lst_subjects);
            }
            catch
            {
                TempData["Alert"] = "An error occurred, try again later";
                return RedirectToAction("Index", "Home");
            }
        }

        private int GetInscriptionsCount(int id_subject)
        {
            // Obtiene la cantidad de inscriptos en una materia
            try
            {
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    return (from sub_stu in db.Subjects_Students
                            where sub_stu.id_subject == id_subject
                            select sub_stu).Count();
                }
            }
            catch
            {
                throw new Exception("Couldn't retrieve number of inscriptions");
            }
        }

        private bool StartsInMiddle(Schedule schedule1, Schedule schedule2)
        {
            // Revisa si el horario schedule1 empieza entre medio del schedule2
            return (schedule1.time_from >= schedule2.time_from &&
                    schedule1.time_from < schedule2.time_to);
        }

        public ActionResult Subject(int id)
        {
            // Muestra la información de la materia de acuerdo al id
            try
            {
                ListSubjectViewModelStudent subject_view_model;
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    int id_student = ((Student)Session["User"]).id_student;

                    // Veo si el alumno ya está inscripto en la materia revisando los registros
                    // de la tabla Subjects_Students
                    var course = (from sub_stu in db.Subjects_Students
                                   where sub_stu.id_student == id_student && sub_stu.id_subject == id
                                   select sub_stu).FirstOrDefault();

                    if(course != null)
                    {
                        // El estudiante ya está inscripto, mandar a la lista de materias con un mensaje
                        TempData["Alert"] = "You're already enrolled in this subject";
                        return RedirectToAction("ListSubjects", "Student");
                    }

                    // Obtengo información de la materia para después chequear que
                    // sea una materia de un profesor activo y que cuente con cupo
                    var info_subject = (from teacher in db.Teachers
                                        join subject in db.Subjects
                                        on teacher.id_teacher equals subject.id_teacher
                                        where subject.id_subject == id
                                        select new { teacher.active, subject.capacity }).FirstOrDefault();
                    
                    if(info_subject == null)
                    {
                        TempData["Alert"] = "An error occurred, try again later";
                        return RedirectToAction("ListSubjects", "Student");
                    }

                    // Reviso que la materia pertenezca a un profesor activo
                    if(!info_subject.active)
                    {
                        TempData["Alert"] = "The course is not available in this moment, try again later";
                        return RedirectToAction("ListSubjects", "Student");
                    }

                    // Reviso que la materia tenga cupo disponible
                    int inscriptions_count = GetInscriptionsCount(id);
                    if (inscriptions_count >= info_subject.capacity)
                    {
                        // No hay cupo
                        TempData["Alert"] = "The course is full, try again later";
                        return RedirectToAction("ListSubjects", "Student");
                    }

                    // Si paso todos los filtros, mostrar la información de la materia 
                    // con el botón para inscribirse
                    subject_view_model = (from subject in db.Subjects
                                          join teacher in db.Teachers
                                          on subject.id_teacher equals teacher.id_teacher
                                          where subject.id_subject == id
                                          select new ListSubjectViewModelStudent
                                          {
                                              id_subject = subject.id_subject,
                                              name = subject.name,
                                              description = subject.desc,
                                              time_from = subject.time_from,
                                              time_to = subject.time_to,
                                              capacity = subject.capacity,
                                              remaining_places = subject.capacity - inscriptions_count,
                                              teacher_name = teacher.last_name + ", " + teacher.first_name
                                          }).FirstOrDefault();
                }

                if (subject_view_model == null)
                {
                    TempData["Alert"] = "An error occurred, try again later";
                    return RedirectToAction("Index", "Home");
                }

                return View(subject_view_model);
            }
            catch
            {
                TempData["Alert"] = "An error occurred, try again later";
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Enroll()
        {
            ViewData["EnrollMessage"] = "There's no subject to enroll";
            ViewData["TypeMessage"] = "alert-info";
            return View();
        }

        [HttpPost]
        public ActionResult Enroll(int id_subject)
        {
            // Recibe por post el id de la materia y trata de inscribirte a la misma
            try
            {
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    int id_student = ((Student)Session["User"]).id_student;

                    // Esto es lo mismo de un metodo anterior que esta arriba... hacer alguna funcion para no repetir el codigo
                    // Veo si el alumno ya está inscripto en la materia revisando los registros
                    // de la tabla Subjects_Students
                    var course = (from sub_stu in db.Subjects_Students
                                    where sub_stu.id_student == id_student && sub_stu.id_subject == id_subject
                                    select sub_stu).FirstOrDefault();

                    if (course != null)
                    {
                        // El estudiante ya está inscripto, mandar a la lista de materias con un mensaje
                        ViewData["EnrollMessage"] = "You're already enrolled in this subject";
                        ViewData["TypeMessage"] = "alert-warning";
                        return View();
                    }

                    // Obtengo la lista de los horarios de todas las materias en las que el 
                    // estudiante se inscribió
                    var lst_schedules = (from subject in db.Subjects
                                         join sub_stu in db.Subjects_Students
                                         on subject.id_subject equals sub_stu.id_subject
                                         where sub_stu.id_student == id_student
                                         select new Schedule
                                         {
                                             time_from = subject.time_from,
                                             time_to = subject.time_to
                                         }).ToList();

                    // Obtengo el horario de la materia a la cual se desea inscribir
                    var subject_schedule = (from subject in db.Subjects
                                            where subject.id_subject == id_subject
                                            select new Schedule
                                            {
                                                time_from = subject.time_from,
                                                time_to = subject.time_to
                                            }).FirstOrDefault();

                    if (subject_schedule == null)
                    {
                        ViewData["EnrollMessage"] = "An error occurred, try again later";
                        ViewData["TypeMessage"] = "alert-danger";
                        return View();
                    }

                    // Recorro la lista de horarios de las inscripciones del estudiante
                    // buscando si hay algun solapamiento de horarios
                    foreach (var schedule in lst_schedules)
                    {
                        if (StartsInMiddle(schedule, subject_schedule) ||
                           StartsInMiddle(subject_schedule, schedule))
                        {
                            ViewData["EnrollMessage"] = "This course overlaps with any other subject that you already enrolled. You can't enroll in this course";
                            ViewData["TypeMessage"] = "alert-info";
                            return View();
                        }
                    }

                    // Si llegó hasta acá significa que no hay horarios solapados...
                    // Inscribirlo en la materia

                    Subject_Student new_subject_student = new Subject_Student();
                    new_subject_student.id_student = ((Student)Session["User"]).id_student;
                    new_subject_student.id_subject = id_subject;

                    db.Subjects_Students.Add(new_subject_student);
                    db.SaveChanges();
                }
                ViewData["EnrollMessage"] = "The enrollment has been successful";
                ViewData["TypeMessage"] = "alert-success";
                return View();
            }
            catch
            {
                ViewData["EnrollMessage"] = "An error occurred, try again later";
                ViewData["TypeMessage"] = "alert-danger";
                return View();
            }
        }
    }
}