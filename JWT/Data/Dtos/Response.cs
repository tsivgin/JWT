using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Data.Dtos
{
    public class Response<T> where T : class
    {
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }
        public bool IsSuccessful { get; private set; }
        public ErrorDto? Error { get; private set; }
    
        public static Response<T> Success(T data,int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode,IsSuccessful=true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> {Data= default, StatusCode = statusCode,IsSuccessful=true };
        }
        public static Response<T> Fail(int statusCode,ErrorDto errorDto)
        {
            return new Response<T> { Data = default, StatusCode = statusCode ,Error = errorDto,IsSuccessful=false};
        }
        public static Response<T> Fail(int statusCode, string errorMessage,bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> { Data = default, StatusCode = statusCode, Error = errorDto ,IsSuccessful=false};
        }

    }
}
