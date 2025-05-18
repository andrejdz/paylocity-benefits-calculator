using System.Net.Mime;
using Api.Shared;
using Api.UseCases.Employees;
using Api.UseCases.Employees.Queries.GetEmployeeById;
using Api.UseCases.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EmployeesController : ControllerBase
{
    // Retuning API response.
    // Usually, when Web API that follows REST API principals is being implemented,
    // HTTP status codes are used to signal whether operation is successful or not.
    // Not sure how ApiResponse is used by a client, but if, for example, always 200 HTTP status code is returned,
    // and client is pushed to check Success property of ApiResponse.
    // This might cause an issue, because client might implement some retry mechanism based on HTTP status codes.
    // In that case, if 200 HTTP status code is returned, client might think that operation was successful,
    // despite the fact that transient error occurred that can be retried.

    private readonly ISender _sender;

    public EmployeesController(ISender sender)
    {
        _sender = sender;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<EmployeeResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<EmployeeResponse>))]
    public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Get(int id)
    {
        try
        {
            GetEmployeeByIdQuery query = new()
            {
                Id = id,
            };
            EmployeeResponse? response = await _sender.Send(query);
            if (response is null)
            {
                return NotFound(
                    new ApiResponse<EmployeeResponse>
                    {
                        Success = false,
                        Error = $"Employee with id {id} was not found.",
                    });
            }

            return new ApiResponse<EmployeeResponse> 
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
                new ApiResponse<EmployeeResponse>
                {
                    Success = false,
                    Error = ex.Message, // Instead of using message from an exception, some generic error message can be used.
                });
        }
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<EmployeeResponse>>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<EmployeeResponse>>))]
    public async Task<ActionResult<ApiResponse<List<EmployeeResponse>>>> GetAll()
    {
        try
        {
            // task: use a more realistic production approach
            // I suppose the number of employees is expected to be big, so as an enhancments pagination can be added.
            // So, GetEmployeesQuery can be modified to hold pagination parameters that are mapped from HTTP query parameters.
            //
            // EmployeeResponse is reused here, but sometimes it is required to return reduced amount of data, for example,
            // dropping dependents from an employee.
            // In this case, new response type can be created that does not contain collection of dependents.
            List<EmployeeResponse> employees = await _sender.Send(new GetEmployeesQuery());

            return new ApiResponse<List<EmployeeResponse>>
            {
                Data = employees,
                Success = true
            };
        }
        catch (Exception ex)
        {
            // Log exception here.

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<List<EmployeeResponse>>
                {
                    Success = false,
                    Error = ex.Message, // Instead of using message from an exception, some generic error message can be used.
                });
        }
    }
}
