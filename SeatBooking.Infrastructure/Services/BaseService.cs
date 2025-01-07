using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using SeatBooking.Application.Repositories;
using SeatBooking.Domain.Common;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DishAdvisor.Infrastructure.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork _unitOfWork;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        protected string GetUserIdFromJwt()
        {
            string id = _httpContextAccessor?.HttpContext?.User.FindFirst("id")!.Value!; 
            return id;
        }

        protected Result<TEntity> Success<TEntity>(TEntity entity) => new Result<TEntity>
        {
            Data = entity,
            StatusCode = HttpStatusCode.OK
        };

        protected Result<TEntity> Fail<TEntity>(string message) => new Result<TEntity>
        {
            Message = message,
            StatusCode = HttpStatusCode.InternalServerError
        };

        protected Result<TEntity> BadRequest<TEntity>(string message) => new Result<TEntity>
        {
            Message = message,
            StatusCode = HttpStatusCode.BadRequest
        };

        protected Result<TEntity> NotFound<TEntity>(string message) => new Result<TEntity>
        {
            Message = message,
            StatusCode = HttpStatusCode.NotFound
        };
    }
}
