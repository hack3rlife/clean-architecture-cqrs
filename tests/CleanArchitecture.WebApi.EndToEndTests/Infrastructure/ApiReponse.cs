using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public T Result { get; set; }

        public ProblemDetails ProblemDetails { get; set; }
    }
}
