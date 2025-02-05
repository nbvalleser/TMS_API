using API_TMS.Data;
using API_TMS.DTO;
using API_TMS.ModMain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using static API_TMS.Controllers.TaskController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_TMS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly TMS_DBContext dbContext;

        public TaskController(TMS_DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

		// CREATE NEW TASK
		[HttpPost]
		public IActionResult CreateTask([FromBody] AddTaskDto newTask)
		{

			var allowedStatuses = new[] { "PENDING", "IN PROCESS", "COMPLETED" };

			if (!allowedStatuses.Contains(newTask.Status.ToUpper()))
			{
				return BadRequest(new { message = "Invalid status value." });
			}

			var mytaskMainModel = new MyTask()
			{
				Title = newTask.Title,
				Description = newTask.Description,
				Status = newTask.Status,
				CreatedAt = newTask.CreatedAt
			};

			// Use Main Model to create new task
			dbContext.MyTasks.Add(mytaskMainModel);
			dbContext.SaveChanges();

			// Map from Main Model to DTO
			var taskDTO = new TaskDto()
			{
				Id = mytaskMainModel.Id,
				Title = mytaskMainModel.Title,
				Description = mytaskMainModel.Description,
				Status = mytaskMainModel.Status.ToUpper(),
				CreatedAt = mytaskMainModel.CreatedAt
			};
			return CreatedAtAction(nameof(GetTaskById), new { id = taskDTO.Id }, taskDTO);

		}


		// RETRIEVE ALL TASK		
		[HttpGet]
		public IActionResult GetTaskAll()
		{
            // Get Data from DB
			var myalltaskMain = dbContext.MyTasks.ToList();

			// Map from Main Model to DTO
			var tasksDto = new List<TaskDto>();
			foreach (var task in myalltaskMain)
			{
				tasksDto.Add(new TaskDto()
				{
					Id = task.Id,
					Title = task.Title,
					Description = task.Description,
					Status = task.Status,
					CreatedAt = task.CreatedAt
				});
			}
			// Return to DTO
			return Ok(tasksDto);            
		}

		[HttpGet]
		[Route("{id:int}")]
		public IActionResult GetTaskById([FromRoute] int id)
		{
			try
			{
				var myalltaskMain = dbContext.MyTasks.FirstOrDefault(obj => obj.Id == id);
				if (myalltaskMain == null)
				{
					return NotFound();
				}

				// Map from Main Model to DTO
				var taskDTO = new TaskDto
				{
					Id = myalltaskMain.Id,
					Title = myalltaskMain.Title,
					Description = myalltaskMain.Description,
					Status = myalltaskMain.Status,
					CreatedAt = myalltaskMain.CreatedAt
				};

				// Return from DTO to client
				return Ok(taskDTO);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateTaskById(int id, [FromBody] UpdateTaskStatusDto updateFrombody)
		{			
			try
			{
				MyTask to_update_Task = new MyTask();

				var allowedStatuses = new[] { "PENDING", "IN PROCESS", "COMPLETED" };

				if (!allowedStatuses.Contains(updateFrombody.Status.ToUpper()))
				{
					return BadRequest(new { message = "Invalid status value." });
				}

				using (var _dbContext = dbContext)
				{
					using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
					{
						try
						{
							to_update_Task = dbContext.MyTasks.Where(h => h.Id == id).FirstOrDefault()!;

							if (to_update_Task != null)
							{
								to_update_Task.Status = updateFrombody.Status.ToUpper();
								dbContext.SaveChanges();
							} 
							else
							{
								return NotFound();
							}
							transaction.Commit();
						}
						catch (Exception)
						{
							transaction.Rollback();
							throw;
						}						
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteTaskById(int id)
		{
			try
			{
				var product = dbContext.MyTasks.FirstOrDefault(p => p.Id == id);
				if (product == null)
				{
					return NotFound(new { message = "Task not found" });
				}

				dbContext.MyTasks.Remove(product);
				dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return NoContent(); 
		}		
	}
}
