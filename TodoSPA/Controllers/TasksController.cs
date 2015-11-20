using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using TodoSPA.DAL;

namespace TodoSPA.Controllers
{

    [Authorize]
    public class TasksController : ApiController
    {
        private TodoListServiceContext db = new TodoListServiceContext();

        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Todo> currentUserToDos = db.Todoes.Where(a => a.Owner == owner);
            return currentUserToDos;
        }

        [HttpGet]
        public Todo Get(int id)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo todo = db.Todoes.FirstOrDefault(a => a.ID == id);

            if (todo == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (todo.Owner != owner)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
                         
            return todo;
        }

        [HttpPost]
        public Todo Post(Todo todo)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;            
            todo.Owner = owner;

            if (string.IsNullOrEmpty(todo.Description))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            db.Todoes.Add(todo);
            db.SaveChanges();
            return todo;            
        }

        [HttpPut]
        public Todo Put(int id, Todo todo)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo xtodo = db.Todoes.FirstOrDefault(a => a.ID == id);

            if (xtodo == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (xtodo.Owner != owner)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            if (string.IsNullOrEmpty(todo.Description))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            xtodo.Description = todo.Description;
            db.SaveChanges();
            return xtodo;
        }

        [HttpDelete]
        public void Delete(int id)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo todo = db.Todoes.First(a => a.ID == id);

            if (todo == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (todo.Owner != owner)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            db.Todoes.Remove(todo);
            db.SaveChanges();
        }        
    }
}
