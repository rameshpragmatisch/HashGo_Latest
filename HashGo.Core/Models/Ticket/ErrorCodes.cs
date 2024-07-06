namespace DinePlan.Common.Model.Point
{
    public class ErrorCodes
    {
        //Ticket Errors - TIXXX
        public static FlyError TI001_NO_VALID_TICKET = new FlyError("TI001", "Not a Valid Ticket");

        public static FlyError TI002_NO_ID_IN_TICKET = new FlyError("TI002", "No ID is available");

        //Common Errors - COXXX
        public static FlyError CO001_CHECK_LOGS = new FlyError("CO001", "Contact Administrator Check Logs");

        public static FlyError CO002_NO_VALID_REQUEST = new FlyError("CO002", "Not a Valid Request");

        //Entity Errors - ENXXX
        public static FlyError EN001_NO_ID = new FlyError("EN001", "Not a Valid Entity ID");

        public static FlyError EN002_NO_SEARCHSTRING = new FlyError("EN001", "Not a Valid Search String");

        //Setting Errors = SNXXX
        public static FlyError SN001_NO_OBJECT = new FlyError("SNOO1", "Not a Valid Setting Object");

        public static FlyError FILE_NOT_FOUND = new FlyError("FO001", "File Not Found ");
    }
}