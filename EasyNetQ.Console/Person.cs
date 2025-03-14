using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.Console
{
    class Person
    {
        public Person(string fullName, string documento, DateTime birthDate)
        {
            FullName = fullName;
            Documento = documento;
            BirthDate = birthDate;
        }
        public string FullName { get; set; }
        public string Documento { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
