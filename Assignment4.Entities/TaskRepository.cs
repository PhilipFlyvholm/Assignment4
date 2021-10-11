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
                StateChanged = DateTime.Now,
                Created = DateTime.Now
            };
            _context.Tasks.Add(entity);
            _context.SaveChanges();
            return (Response.Created, entity.id);
        }

        public Response Delete(int taskId)
        {
            var task = Read(taskId);
            if (task == null) return Response.NotFound;
            if (task.State.Equals(State.Active))
            {
                Update(new TaskUpdateDTO
                {
                    Id = taskId,
                    State = State.Removed
                });
                return Response.Deleted;
            }
            else
            {
                return Response.Conflict;
            }
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

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            return new ReadOnlyCollection<TaskDTO>(_context.Tasks.Select(t => new TaskDTO(
                t.id,
                t.Title,
                t.AssignedTo.Name,
                new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
                t.State
            )).ToList());
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            return new ReadOnlyCollection<TaskDTO>(_context.Tasks
            .Where(t => t.State.Equals(state))
            .Select(t => new TaskDTO(
            t.id,
            t.Title,
            t.AssignedTo.Name,
            new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
            t.State
            )).ToList());
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            return new ReadOnlyCollection<TaskDTO>(_context.Tasks
            .Where(t => t.Tags.Where(tag => tag.Name.Equals(tag)).Count() > 0)
            .Select(t => new TaskDTO(
            t.id,
            t.Title,
            t.AssignedTo.Name,
            new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
            t.State
            )).ToList());
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            return new ReadOnlyCollection<TaskDTO>(_context.Tasks
            .Where(t => t.AssignedTo.id.Equals(userId))
            .Select(t => new TaskDTO(
            t.id,
            t.Title,
            t.AssignedTo.Name,
            new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
            t.State
            )).ToList());
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            return new ReadOnlyCollection<TaskDTO>(_context.Tasks
            .Where(t => t.State.Equals(State.Removed))
            .Select(t => new TaskDTO(
            t.id,
            t.Title,
            t.AssignedTo.Name,
            new ReadOnlyCollection<String>(t.Tags.Select(t => t.Name).ToList()),
            t.State
            )).ToList());
            throw new NotImplementedException();
        }

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Tasks.Find(task.Id);
            entity.State = task.State;
            entity.StateChanged = DateTime.Now;
            _context.SaveChanges();
            return Response.Updated;
        }

        private User GetUser(int? id) =>
            _context.Users.FirstOrDefault(u => u.id == id);
    }
}

