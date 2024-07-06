#region using

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyResponse
    {
        public bool IsError { get; set; }
        public object Response { get; set; }
        public FlyError Error { get; set; }

        public static FlyResponse BuildResponse(bool _IsError, object _Response, FlyError _Error)
        {
            var returnResponseObject = new FlyResponse
            {
                IsError = _IsError,
                Response = _Response,
                Error = _Error
            };
            return returnResponseObject;
        }
    }
}