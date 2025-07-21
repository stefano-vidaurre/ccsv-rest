using CCSV.Domain.Dtos;
using CCSV.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CCSV.Rest.Controllers;

public abstract class RestController<TRead, TCreate, TUpdate, TQuery, TFilter> : RestController<TRead, TCreate, TQuery, TFilter>
    where TRead : EntityReadDto
    where TCreate : EntityCreateDto
    where TUpdate : EntityUpdateDto
    where TQuery : EntityQueryDto
    where TFilter : EntityFilterDto
{
    private readonly IEntityAppService<TRead, TCreate, TUpdate, TQuery, TFilter> _service;

    protected RestController(IEntityAppService<TRead, TCreate, TUpdate, TQuery, TFilter> service) : base(service)
    {
        _service = service;
    }

    [HttpPut("{id}")]
    public Task Update(Guid id, [FromBody] TUpdate data)
    {
        return _service.Update(id, data);
    }
}

public abstract class RestController<TRead, TCreate, TQuery, TFilter> : RestController<TRead, TQuery, TFilter>
    where TRead : EntityReadDto
    where TCreate : EntityCreateDto
    where TQuery : EntityQueryDto
    where TFilter : EntityFilterDto
{
    private readonly IEntityAppService<TRead, TCreate, TQuery, TFilter> _service;

    protected RestController(IEntityAppService<TRead, TCreate, TQuery, TFilter> service) : base(service)
    {
        _service = service;
    }

    [HttpPost]
    public Task Create([FromBody] TCreate data)
    {
        return _service.Create(data);
    }
}

[ApiController]
public abstract class RestController<TRead, TQuery, TFilter> : ControllerBase
    where TRead : EntityReadDto
    where TQuery : EntityQueryDto
    where TFilter : EntityFilterDto
{
    private readonly IEntityAppService<TRead, TQuery, TFilter> _service;

    protected RestController(IEntityAppService<TRead, TQuery, TFilter> service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<IEnumerable<TQuery>> GetAll([FromQuery] TFilter filter)
    {
        return _service.GetAll(filter);
    }

    [HttpGet("{id}")]
    public Task<TRead> GetById(Guid id)
    {
        return _service.GetById(id);
    }

    [HttpGet("Length")]
    public Task<int> GetLength([FromQuery] TFilter filter)
    {
        return _service.GetLength(filter);
    }

    [HttpDelete("{id}")]
    public Task Delete(Guid id)
    {
        return _service.Delete(id);
    }

    [HttpPut("{id}/Enabled")]
    public Task Enable(Guid id)
    {
        return _service.Enable(id);
    }

    [HttpPut("{id}/Disabled")]
    public Task Disable(Guid id)
    {
        return _service.Disable(id);
    }
}
