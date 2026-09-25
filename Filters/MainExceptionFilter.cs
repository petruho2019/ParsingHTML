using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ParsingHTML.Exceptions;
using ParsingHTML.Models.DTO;

namespace ParsingHTML.Filters;


public class MainExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = new ResponseDto
        {
            IsError = 1,
            ElementsCount = 0,
            EmailCount = 0,
            Url = string.Empty,
            DecryptedPlainText = string.Empty,
            ElementsAttrList = [],
            EmailsList = []
        };

        var (errorCode, message) = context.Exception switch
        {
            ParseException validEx => ("Parse", validEx.Message),
            FormatException formatException => ("Decode", formatException.Message),
            ValidationException validationException => ("Validation", validationException.Message),
            NullReferenceException nullReferenceException => ("NullRefence", nullReferenceException.Message),
            _ => ("InternalServerError", "Внутренняя ошибка сервера")
        };

        response.ErrorCode = errorCode;
        response.ErrorMessage = message;

        context.Result = new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}