using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using rmr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rmr
{
    class FirebaseReaderWriter
    {
        private readonly string ChildName = "Persons";
        private readonly string employeesName = "Employees";
        private readonly string predmetiName = "Predmeti";
        readonly FirebaseClient firebase = new FirebaseClient("https://feriapp-6d517.firebaseio.com/");
        public async Task<List<Person>> GetAllPersons()
        {
            return (await firebase
            .Child(ChildName)
            .OnceAsync<Person>()).Select(item => new Person
            {
                Email = item.Object.Email,
                Password = item.Object.Password,
                PersonId = item.Object.PersonId
            }).ToList();
        }
        public async Task AddPerson(string email, string password)
        {
            await firebase
            .Child(ChildName)
            .PostAsync(new Person() { PersonId = Guid.NewGuid(), Email = email, Password = password });
        }
        public async Task<Person> GetPerson(Guid personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
            .Child(ChildName)
            .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.PersonId == personId);
        }
        public async Task<Person> GetPerson(string email)
        {
            var allPersons = await GetAllPersons();
            // await firebase
            // .Child(ChildName)
            // .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.Email == email);
        }
        public async Task UpdatePerson(Guid personId, string email, string password)
        {
            var toUpdatePerson = (await firebase
            .Child(ChildName)
            .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);
            await firebase
            .Child(ChildName)
            .Child(toUpdatePerson.Key)
            .PutAsync(new Person() { PersonId = personId, Email = email, Password = password });
        }
        public async Task DeletePerson(Guid personId)
        {
            var toDeletePerson = (await firebase
            .Child(ChildName)
            .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);
            await firebase.Child(ChildName).Child(toDeletePerson.Key).DeleteAsync();
        }


        public async Task<List<Predmet>> GetAllSubjects()
        {
            return (await firebase
            .Child(predmetiName)
            .OnceAsync<Predmet>()).Select(item => new Predmet
            {
                Naziv = item.Object.Naziv,
                Ects = item.Object.Ects,
                Semester = item.Object.Semester
                }).ToList();
        }

        public async Task<Predmet> GetSubject(Guid subjectId)
        {
            var allSubjects = await GetAllSubjects();
            await firebase
            .Child(predmetiName)
            .OnceAsync<Predmet>();
            return allSubjects.FirstOrDefault(x => x.PredmetId == subjectId);
        }

        public async Task<Predmet> GetSubject(string naziv)
        {
            var allSubjects = await GetAllSubjects();
            await firebase
            .Child(predmetiName)
            .OnceAsync<Predmet>();
            return allSubjects.FirstOrDefault(a => a.Naziv == naziv);
        }

        public async Task UpdateSubject(Guid subjectId, string naziv, string ects, string semester)
        {
            var toUpdateubject = (await firebase
            .Child(predmetiName)
            .OnceAsync<Predmet>()).FirstOrDefault(a => a.Object.PredmetId == subjectId);
            await firebase
            .Child(predmetiName)
            .Child(toUpdateubject.Key)
            .PutAsync(new Predmet() { PredmetId = subjectId, Naziv = naziv, Ects = ects, Semester = semester });
        }
        public async Task DeleteSubject(Guid subjectId)
        {
            var toDeleteSubject = (await firebase
            .Child(predmetiName)
            .OnceAsync<Predmet>()).FirstOrDefault(a => a.Object.PredmetId == subjectId);
            await firebase.Child(predmetiName).Child(toDeleteSubject.Key).DeleteAsync();
        }

        public async Task AddSubject(Guid id, string naziv, string ects, string semester)
        {
            await firebase
            .Child("Predmeti")
            .PostAsync(new Predmet() { PredmetId = Guid.NewGuid(), Naziv = naziv, Ects = ects, Semester = semester });
        }

        public async Task AddEmployee(Guid id, string name, string age, string salary, string image)
        {
            await firebase
            .Child(employeesName)
            .PostAsync(new Employee() { ID = Guid.NewGuid(), Name = name, Age = age, Salary = salary });
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return (await firebase
            .Child(employeesName)
            .OnceAsync<Employee>()).Select(item => new Employee
            {
                ID = (Guid)item.Object.ID,
                Name = item.Object.Name,
                Age = item.Object.Age,
                Salary = item.Object.Salary,

            }).Take(20).ToList();
        }

        public async Task<Employee> GetEmployee(Guid employeeId)
        {
            var allSubjects = await GetAllEmployees();
            await firebase
            .Child(employeesName)
            .OnceAsync<Employee>();
            return allSubjects.FirstOrDefault(x => x.ID == employeeId);
        }

        public async Task UpdateEmployee(Guid employeeId, string name, string age, string salary, string image)
        {
            var toUpdateubject = (await firebase
            .Child(employeesName)
            .OnceAsync<Employee>()).FirstOrDefault(a => a.Object.ID == employeeId);
            await firebase
            .Child(employeesName)
            .Child(toUpdateubject.Key)
            .PutAsync(new Employee() { ID = employeeId, Name = name, Age = age, Salary = salary });
        }
        public async Task DeleteEmployee(Guid employeeId)
        {
            var toDeleteSubject = (await firebase
            .Child(employeesName)
            .OnceAsync<Employee>()).FirstOrDefault(a => a.Object.ID == employeeId);
            await firebase.Child(employeesName).Child(toDeleteSubject.Key).DeleteAsync();
        }

        public async Task<List<Predmet>> DobiPredmete()
        {
            return (await firebase
         .Child("Predmet")
         .OnceAsync<Predmet>()).Select(item => new Predmet
         {
             Naziv = item.Object.Naziv,
             Ects = item.Object.Ects
         }).ToList();
        }
    }
}
