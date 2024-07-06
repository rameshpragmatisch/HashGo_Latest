namespace DinePlan.Common.Model.Point
{
    public class FlyError
    {
        public FlyError(string erroCode, string desc)
        {
            ErrorCode = erroCode;
            ErrorDescription = desc;
        }

        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}