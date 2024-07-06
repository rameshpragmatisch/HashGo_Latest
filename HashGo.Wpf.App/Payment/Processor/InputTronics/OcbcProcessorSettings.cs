namespace DinePlan.Modules.PaymentModule.PaymentProcessors.InputTronics
{
    public class OcbcProcessorSettings
    {
        public string Port { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public bool? Network { get; set; }
        public string IpAddress{ get; set; }
        public bool? AutoSendToTerminal{ get; set; }
        public string TerminalAddress{ get; set; }


    }
}