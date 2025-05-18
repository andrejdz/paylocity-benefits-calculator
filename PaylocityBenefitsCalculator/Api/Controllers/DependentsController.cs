using Api.Shared;
using Api.UseCases;
using Api.UseCases.Dependents.Queries.GetDependents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Api.UseCases.Dependents.Queries.GetDependentById;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class DependentsController : ControllerBase
{
    private readonly ISender _sender;

    public DependentsController(ISender sender)
    {
        _sender = sender;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<DependentResponse>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<DependentResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<DependentResponse>))]
    public async Task<ActionResult<ApiResponse<DependentResponse>>> Get(int id)
    {
        try
        {
            GetDependentByIdQuery query = new()
            {
                Id = id,
            };
            DependentResponse? response = await _sender.Send(query);
            if (response is null)
            {
                return NotFound(
                    new ApiResponse<DependentResponse>
                    {
                        Success = false,
                        Error = $"Dependent with id {id} was not found.",
                    });
            }

            return new ApiResponse<DependentResponse>
            {
                Data = response,
                Success = true,
            };
        }
        // Usually, I use global exception handler to handle unhandled exceptions.
        catch (Exception ex)
        {
            // Log exception here.

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<DependentResponse>
                {
                    Success = false,
                    Error = ex.Message, // Instead of using message from an exception, some generic error message can be used.
                });
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<DependentResponse>>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<DependentResponse>>))]
    public async Task<ActionResult<ApiResponse<List<DependentResponse>>>> GetAll()
    {
        try
        {
            List<DependentResponse> dependents = await _sender.Send(new GetDependentsQuery());

            return new ApiResponse<List<DependentResponse>>
            {
                Data = dependents,
                Success = true
            };
        }
        catch (Exception ex)
        {
            // Log exception here.

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<List<DependentResponse>>
                {
                    Success = false,
                    Error = ex.Message, // Instead of using message from an exception, some generic error message can be used.
                });
        }
    }
}
