using TestHumanEmulator;

namespace XHE
{
    public class FavbetProcessor : ProcessorBase
    {
        #region XHEFavbetConstants
        private const string MainPage = "https://www.favbet.ua/";
        private const string XHENumberSeparator = "<@@@b@r@@@>";
        private const string DepositHrefId = "f8ccacf7-55fa-400b-bd21-728b147cd840";
        private const string LoginHref = "login/?from=header-desktop";
        private const string LoginInnerHtml = "<span class=\"Box_box__3oOkB Bu";
        private const string BalanceOuterHtml = "<span class=\"Text_base__3OOOi";
        private const string CurrencyOuterHtml = "<span class=\"Text_base__3OOOi";
        private const string FullTimeFirstTeamWinClass = "OutcomeButton_coef__290P9";
        private const string PlaceBetInputClass = "Input_input__FzRkn BetSumInput_input__3xT0O BetSumInput_withoutValue__3p5OP";
        private const string PlaceBetButtonClass = "Button_button__WYSdr Button_primary__1OqiW Button_l__3KQSE Button_color1__3fuRd Button_disabled__2w937 Button_fullWidth__1vToY";
        #endregion

        public FavbetProcessor(string server, string password = "") : base(server, password)
        {

        }

        protected override bool Login(string login, string password)
        {
            browser.navigate(MainPage);

            browser.wait_for(15, 3);

            if (anchor.is_exist_by_id(DepositHrefId))
            {
                return true;
            }
            else
            {
                anchor.click_by_href(LoginHref, NotExect);

                browser.wait_for(15, 3);

                Console.WriteLine($"click email: {input.get_by_id("email").meta_click()}");

                Console.WriteLine($"input email text: {MyKeyboard.Input(login)}");

                Console.WriteLine($"click password: {input.get_by_id("password").meta_click()}");

                Console.WriteLine($"input password text: {MyKeyboard.Input(password)}");
            }

            return btn.click_by_inner_html(LoginInnerHtml, NotExect);
        }

        protected override bool CheckBalance(double betAmount)
        {
            var balanceString = span.get_by_outer_html(BalanceOuterHtml, NotExect).get_inner_text().Split()[0];

            //var currencyString = span.get_by_outer_html(CurrencyOuterHtml, NotExect).get_inner_text().Split()[1];

            double balance = double.Parse(balanceString, System.Globalization.CultureInfo.InvariantCulture);

            return balance >= betAmount;
        }

        protected override bool PlaceBet(string betUrl, double betAmount)
        {
            if (!browser.navigate(betUrl))
            {
                return false;
            }

            span.wait_element_exist_by_attribute(FullTimeFirstTeamWinClass, FullTimeFirstTeamWinClass, NotExect);
            browser.wait_for();
            Thread.Sleep(2000);

            Console.WriteLine("betClick: " + span.get_by_attribute(ClassAttribute, FullTimeFirstTeamWinClass, NotExect).meta_click());

            input.get_by_attribute(FullTimeFirstTeamWinClass, PlaceBetInputClass).meta_click();

            MyKeyboard.Input($"{betAmount}");

            if(IsErrorsExist())
            {
                return false;
            }

            button.click_by_attribute(FullTimeFirstTeamWinClass, PlaceBetButtonClass, NotExect);

            return true;
        }

        private bool IsErrorsExist()
        {
            bool result = false;
            List<string> errors = new List<string>();

            var errorNumbers = div.get_all_numbers_by_attribute(FullTimeFirstTeamWinClass, "error", 0);

            if (errorNumbers?.Length > 0)
            {
                result = true;

                errorNumbers = errorNumbers[0].Split(XHENumberSeparator);

                foreach (var number in errorNumbers)
                {
                    var error = div.get_inner_text_by_number(Int32.Parse(number));

                    errors.Add(error);
                }
            }

            return result;
        }
    }
}
