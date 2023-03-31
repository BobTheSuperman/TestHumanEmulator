using TestHumanEmulator;

namespace XHE
{
    public class FavbetProcessor : ProcessorBase
    {
        private const string MainPage = "https://www.favbet.ua/";

        public FavbetProcessor(string server, string password = "") : base(server, password)
        {

        }

        protected override bool Login(string login, string password)
        {
            browser.navigate(MainPage);

            browser.wait_for(15, 3);

            if (anchor.is_exist_by_id("f8ccacf7-55fa-400b-bd21-728b147cd840"))
            {
                return true;
            }
            else
            {
                anchor.click_by_href("login/?from=header-desktop", 0);

                browser.wait_for(15, 3);

                Console.WriteLine($"click email: {input.get_by_id("email").meta_click()}");

                Console.WriteLine($"input email text: {MyKeyboard.Input(login)}");

                Console.WriteLine($"click password: {input.get_by_id("password").meta_click()}");

                Console.WriteLine($"input password text: {MyKeyboard.Input(password)}");
            }

            return btn.click_by_inner_html("<span class=\"Box_box__3oOkB Bu", 0);
        }

        protected override bool CheckBalance(double betAmount)
        {
            var balanceString = span.get_by_outer_html("<span class=\"Text_base__3OOOi ", 0).get_inner_text().Split()[0];
            //var currencyString = span.get_by_outer_html("<span class=\"Text_base__3OOOi ", 0).get_inner_text().Split()[1];

            double balance = double.Parse(balanceString, System.Globalization.CultureInfo.InvariantCulture);

            return balance >= betAmount;
        }

        protected override bool PlaceBet(string betUrl)
        {
            if (!browser.navigate(betUrl))
                return false;

            browser.wait_for(15, 3);

            span.click_by_attribute("class", "OutcomeButton_coef__290P9", 0);

            return true;
        }
    }
}
