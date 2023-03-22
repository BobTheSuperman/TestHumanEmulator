using TestHumanEmulator;

namespace XHE
{
    public abstract class ProcessorBase : XHEScript
    {
        protected MyKeyboardImplementation MyKeyboard;
        public ProcessorBase(string server, string password = "") : base(server, password)
        {
            server = "127.0.0.1:7011";
            InitXHE();
            MyKeyboard = new MyKeyboardImplementation(server, password, new XHEScript());
        }

        public void ProcessBet()
        { 
            Login();

            if(CheckBalance())
            {
                PlaceBet();
            }
        }

        protected abstract bool Login();
        protected abstract bool CheckBalance();
        protected abstract void PlaceBet();
    }
}
