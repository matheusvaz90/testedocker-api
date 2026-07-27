using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db)
    {
        _db = db;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        return await _db.Tasks.Where(t => t.UserId == CurrentUserId).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var task = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem task)
    {
        task.UserId = CurrentUserId;
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> Update(int id, TodoItem input)
    {
        var task = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        if (task is null) return NotFound();

        task.Title = input.Title;
        task.Description = input.Description;
        task.Done = input.Done;
        await _db.SaveChangesAsync();
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        if (task is null) return NotFound();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
