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

        //ToDo: replace the params to UserBetJobDto for our main app
        public void ProcessBet(string login, string password, double betAmount, string betUrl)
        { 
            if(Login(login, password))
            {
                if (CheckBalance(betAmount))
                {
                    PlaceBet(betUrl);
                }
            }
        }

        protected abstract bool Login(string login, string password);
        protected abstract bool CheckBalance(double betAmaumt);
        protected abstract bool PlaceBet(string betUrl);
    }
}
