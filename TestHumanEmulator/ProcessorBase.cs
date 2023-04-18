using TestHumanEmulator;

namespace XHE
{
    public abstract class ProcessorBase : XHEScript
    {
        #region XHEConstant
        protected const string ClassAttribute = "class";
        protected const string IdAttribute = "id";
        protected const string NameAttribute = "name";
        protected const int NotExact = 0;
        protected const int Exact = 1;
        #endregion

        protected MyKeyboardImplementation MyKeyboard;

        public ProcessorBase(string server, string password = "") : base(server, password)
        {
            server = "127.0.0.1:7011";
            InitXHE();
            MyKeyboard = new MyKeyboardImplementation(server, password, new XHEScript());
        }

        //ToDo: replace the params to UserBetJobDto for our main app
        public void ProcessBet(string login, string password, double betAmount, string betUrl, double koef)
        {
            //SetBrowserSettings();

            if (Login(login, password))
            {
                if (CheckBalance(betAmount))
                {
                    PlaceBet(betUrl, betAmount, koef);
                }
            }
        }

        protected abstract bool Login(string login, string password);
        protected abstract bool CheckBalance(double betAmaunt);
        protected abstract bool PlaceBet(string betUrl, double betAmaunt, double koef);

        public bool SetBrowserSettings()
        {
            browser.clear_address_bar_history();
            browser.clear_cache();
            browser.clear_history();
            browser.clear_cookies("");
            browser.set_do_not_track();

            return true;
        }
    }
}
