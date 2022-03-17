using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? DataCount { get; set; }
        public dynamic Data { get; set; }
        public Exception Exception { get; set; }

        public ResponseModel()
        {
        }

        public ResponseModel(bool isSuccess, string message, int? dataCount, object data, Exception exception)
        {
            IsSuccess = isSuccess;
            Message = message;
            DataCount = dataCount;
            Data = data;
            Exception = exception;
        }

        public static ResponseModel CreateSuccess(object data = null, string message = null, int? dataCount = null)
        {
            return new ResponseModel(true, message, dataCount, data, null);
        }

        public static ResponseModel CreateError(string message = null)
        {
            return new ResponseModel(false, message, null, null, null);
        }

        public static ResponseModel CreateErrorObject(object data)
        {
            return new ResponseModel(false, null, null, data, null);
        }

        public static ResponseModel CreateErrorObject(Exception ex)
        {
            return new ResponseModel(false, null, null, null, ex);
        }
    }
}
