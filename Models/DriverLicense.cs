using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("driver_license")]
    public class DriverLicense : Model
    {
        [PrimaryKey]
        [Key]
        [AutoIncrement]
        public int Id {  get; set; }
        public DateTime DueDate { get; set; }
        public string PersonRegister {  get; set; }

        public string IdentificationNumber { get; set; }

        public string MotherName { get; set; }

        public string FatherName {  get; set; }

        [Column("category_id")]
        public Category Category { get; set; }
    }
}
