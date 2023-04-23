using Microsoft.AspNetCore.Mvc;
using Paginacao.Data;
using Paginacao.Models;
using Microsoft.EntityFrameworkCore;

namespace Paginacao.Controllers;

[ApiController]
[Route("v1/todos")]
public class TodoController : ControllerBase
{
    [HttpGet("load")]
    public async Task<IActionResult> LoadAsync(
        [FromServices] AppDbContext context)
    {
        for (int i = 0; i < 1348; i++)
        {
            var todo = new Todo()
            {
                Id = i + 1,
                Done = false,
                CreatedAt = DateTime.Now,
                Title = $"Tarefa {i}"
            };
            await context.Todos.AddAsync(todo);
            await context.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpGet("page/{page:int}/take/{take:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int page = 1, 
        [FromRoute] int take = 25)
    {
        var total = await context.Todos.CountAsync();
        var todos = await context
            .Todos
            .AsNoTracking()
            .Skip((page-1)*take)
            .Take(take)
            .ToListAsync();

        return Ok(new 
        {
            total,
            page,
            take,
            data = todos
        });
    }
}