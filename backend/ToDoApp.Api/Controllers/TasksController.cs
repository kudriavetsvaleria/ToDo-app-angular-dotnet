using Microsoft.AspNetCore.Mvc;
using ToDoApp.Domain.Dtos;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // -> /api/tasks
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // TODO (auth step): replace this hard-coded id with the user id from the JWT token.
    private const int TempUserId = 1;

    // GET /api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll()
    {
        var tasks = await _taskService.GetTasksAsync(TempUserId);
        return Ok(tasks);
    }

    // GET /api/tasks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var task = await _taskService.GetTaskAsync(id, TempUserId);
        return task is null ? NotFound() : Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var created = await _taskService.CreateTaskAsync(TempUserId, request);
        // 201 Created + Location header pointing at the new resource.
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/tasks/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskRequest request)
    {
        var updated = await _taskService.UpdateTaskAsync(id, TempUserId, request);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/tasks/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id, TempUserId);
        return deleted ? NoContent() : NotFound();
    }
}
