using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utils
{
    public static class JsonUtil
    {
        public enum StatusResponse
        {
            Success = 1,
            Error = 0
        }

        public static JsonResult Success(object data = null, string message = null, int? dataCount = null)
        {
            object obj;

            if (dataCount.HasValue)
                obj = new { isSuccess = StatusResponse.Success, message = message, dataCount = dataCount, data = data };
            else
                obj = new { isSuccess = StatusResponse.Success, message = message, data = data };

            return new JsonResult(obj);
        }

        public static JsonResult Success(ResponseModel model)
        {
            object obj;

            if (model.DataCount.HasValue)
                obj = new { isSuccess = StatusResponse.Success, message = model.Message, dataCount = model.DataCount, data = model.Data };
            else
                obj = new { isSuccess = StatusResponse.Success, message = model.Message, data = model.Data };

            return new JsonResult(obj);
        }

        public static JsonResult Error(ResponseModel model)
        {
            return new JsonResult( new { isSuccess = StatusResponse.Error, message = model.Message, data = model.Data });
        }

        public static JsonResult Error(ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        public static JsonResult Error(string message, object data)
        {
            return new JsonResult(new { isSuccess = StatusResponse.Error, message = message, data = data });
        }

        public static JsonResult Error(string message)
        {
            return Error(message, null);
        }

        public static JsonResult Create(ResponseModel model)
        {
            if (model.IsSuccess)
                return Success(model);
            else
                return Error(model);
        }

    }
}
