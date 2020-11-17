﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerService.Core.Exceptions
{
    public class DevelopmentExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public DevelopmentExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCode = "error";
            var errorMessage = "Something gone wrong...";
            string stackTrace = null;
            HttpStatusCode internalStatusCode;
            var exceptionType = exception.GetType();

            switch (exception)
            {
                case UnauthorizedAccessException e when exceptionType == typeof(UnauthorizedAccessException):
                    internalStatusCode = HttpStatusCode.Unauthorized;
                    errorMessage = e.Message;
                    break;

                case InvalidOperationException e when exceptionType == typeof(InvalidOperationException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorMessage = e.Message;
                    break;

                case ControllerException e when exceptionType == typeof(ControllerException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    errorMessage = e.Message;
                    stackTrace = e.StackTrace;
                    break;

                case ServiceException e when exceptionType == typeof(ServiceException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    errorMessage = e.Message;
                    stackTrace = e.StackTrace;
                    break;

                case HelperException e when exceptionType == typeof(HelperException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    errorMessage = e.Message;
                    stackTrace = e.StackTrace;
                    break;

                case ValidatorException e when exceptionType == typeof(ValidatorException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    errorMessage = e.Message;
                    stackTrace = e.StackTrace;
                    break;

                case RepositoryException e when exceptionType == typeof(RepositoryException):
                    internalStatusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    errorMessage = e.Message;
                    stackTrace = e.StackTrace;
                    break;

                default:
                    internalStatusCode = HttpStatusCode.InternalServerError;
                    errorCode = "internal_error";
                    errorMessage = exception.Message;
                    stackTrace = exception.StackTrace;
                    break;
            }

            stackTrace = Regex.Replace(stackTrace, "\r\n", string.Empty);
            var response = new {statusCode = (int)internalStatusCode, code = errorCode, message = errorMessage, stackTrace = stackTrace };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            return context.Response.WriteAsync(payload);
        }
    }
}