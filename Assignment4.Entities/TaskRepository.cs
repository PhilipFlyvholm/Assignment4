using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            var entity = new Task
            {
                Title = task.Title,
                AssignedTo = GetUser(task.AssignedToId),
                Description = task.Description,
                State = State.New,
            };
            throw new NotImplementedException();
        }

        public Response Delete(int taskId)
        {
            throw new NotImplementedException();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            var tasks = _context.Tasks.Where(t => t.id == taskId).Select(t => new TaskDetailsDTO(
               t.id,
               t.Title,
               t.Description,
               t.Created,
               t.AssignedTo.Name,
               new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
               t.State,
               t.StateChanged
           ));
            return tasks.FirstOrDefault();
        }

        private IReadOnlyCollection<string> GetTags(int v, int taskId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public Response Update(TaskUpdateDTO task)
        {
            throw new NotImplementedException();
        }

        private User GetUser(int? id) =>
            _context.Users.FirstOrDefault(u => u.id == id);
    }
}

