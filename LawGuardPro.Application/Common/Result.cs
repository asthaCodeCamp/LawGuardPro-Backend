using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Common;

public interface IResult
{
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; }
}

public class Result : IResult
{
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; }

    protected Result(int statusCode, List<Error> errors)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public bool IsSuccess() { 
        if(StatusCode >= 200 || StatusCode <= 300){
            return true;
        }
        return false;
    }
    public static Result Success(int statusCode) { 
        return new Result(statusCode, new List<Error>());
    }
    public static Result Failure(int statusCode,  List<Error> errors) { 
           return new Result(statusCode, errors); 
    } 
    
   
}

public interface IResult<T> 
{
    public T Data { get; }
    public int StatusCode { get; }
    public List<Error> Errors { get; }
}

public class Result<T> : IResult<T>
{
    public T Data { get; }
    public int StatusCode { get; }
    public List<Error> Errors { get; }
    protected Result(int statusCode, T data, List<Error> errors) {
        Data = data;
        StatusCode = statusCode;
        Errors = errors;
    }
    public bool IsSuccess()
    {
        if (StatusCode >= 200 || StatusCode <= 300)
        {
            return true;
        }
        return false;
    }
    public static Result<T> Success(T data) {
        
        return new Result<T> (StatusCodes.Status200OK, data, new List<Error>());
    }
    public new static Result<T> Failure(List<Error> errors)
    {
        return new Result<T>(StatusCodes.Status400BadRequest, default, errors);
    }
}

