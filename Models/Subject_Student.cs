//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Challenge.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subject_Student
    {
        public int id_subject_student { get; set; }
        public int id_subject { get; set; }
        public int id_student { get; set; }
    
        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
    }
}