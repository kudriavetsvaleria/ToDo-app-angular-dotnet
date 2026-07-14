using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Domain.Dtos;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Authorize] // every endpoint here requires a valid JWT
[Route("api/[controller]")] // -> /api/tasks
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // The authenticated user's id, read from the JWT (the NameIdentifier claim we put in the token).
    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /api/tasks?page=1&pageSize=10&search=milk&categoryId=2
    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null)
    {
        var tasks = await _taskService.GetTasksAsync(CurrentUserId, page, pageSize, search, categoryId);
        return Ok(tasks);
    }

    // GET /api/tasks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var task = await _taskService.GetTaskAsync(id, CurrentUserId);
        return task is null ? NotFound() : Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var created = await _taskService.CreateTaskAsync(CurrentUserId, request);
        // 201 Created + Location header pointing at the new resource.
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/tasks/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskRequest request)
    {
        var updated = await _taskService.UpdateTaskAsync(id, CurrentUserId, request);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/tasks/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id, CurrentUserId);
        return deleted ? NoContent() : NotFound();
    }
}
