using System.Globalization;
using TestHumanEmulator;
using XHE.XHE_DOM;

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
                anchor.click_by_href(LoginHref, NotExact);

                browser.wait_for(15, 3);

                Console.WriteLine($"click email: {input.get_by_id("email").meta_click()}");

                Console.WriteLine($"input email text: {MyKeyboard.Input(login)}");

                Console.WriteLine($"click password: {input.get_by_id("password").meta_click()}");

                Console.WriteLine($"input password text: {MyKeyboard.Input(password)}");
            }

            return btn.click_by_inner_html(LoginInnerHtml, NotExact);
        }

        protected override bool CheckBalance(double betAmount)
        {
            var balanceString = span.get_by_outer_html(BalanceOuterHtml, NotExact).get_inner_text().Split()[0];

            //var currency = span.get_by_outer_html(CurrencyOuterHtml, NotExect).get_inner_text().Split()[1];

            double balance = double.Parse(balanceString, System.Globalization.CultureInfo.InvariantCulture);

            return balance >= betAmount;
        }

        protected override bool PlaceBet(string betUrl, double betAmount, double koef)
        {
            if (!browser.navigate(betUrl))
            {
                return false;
            }

            browser.wait_for();
            Thread.Sleep(2000);

            var divsByBetType = div.get_all_by_inner_text("Handicap", Exact).get_next();

            var periodUpperDiv = divsByBetType[0].get_all_child_by_inner_text("2nd Half", NotExact, true);
            var periodDiv = GetLowestDivWithValue(periodUpperDiv, "2nd Half")?.get_parent()[0].get_next();

            if (periodUpperDiv.get_number().Count > 0)
            {

                var upperDivWithNecessaryParam = periodDiv.get_all_child_by_inner_text("1.5", NotExact, true);

                var targetDiv = GetLowestDivWithValue(upperDivWithNecessaryParam, koef.ToString("0.00", CultureInfo.GetCultureInfo("en-US")));

                Thread.Sleep(2000);

                targetDiv.meta_click();

                Thread.Sleep(2000);

                input.get_by_attribute(ClassAttribute, PlaceBetInputClass).click();

                MyKeyboard.DeleteAllTextFromInput();

                MyKeyboard.Input($"{betAmount}");

                if (IsErrorsExist())
                {
                    return false;
                }

                button.click_by_attribute(ClassAttribute, PlaceBetButtonClass, NotExact);
            }
            else
            {
                Console.WriteLine($"Bet: Handicap\nParam: 2nd\nKoef{koef} \nNot Found");

                return false;
            }

            return true;
        }

        private bool IsErrorsExist()
        {
            bool result = false;
            List<string> errors = new List<string>();

            var errorNumbers = div.get_all_numbers_by_attribute(ClassAttribute, "error", 0);

            if (errorNumbers?.Length > 0)
            {
                result = true;

                errorNumbers = errorNumbers[0].Split(XHENumberSeparator);

                foreach (var number in errorNumbers)
                {
                    var error = div.get_inner_text_by_number(Int32.Parse(number));

                    Console.WriteLine(error);

                    errors.Add(error);
                }
            }

            return result;
        }

        private XHEInterfaces GetLowestDivWithValue(XHEInterfaces? betDivs, string innerText)
        {
            var result = betDivs.get_all_child_by_inner_text(innerText, NotExact, true);

            if (result.Count == 0)
            {
                return null;
            }

            foreach (var div in result)
            {
                if (result.Count > 2)
                {
                    var res = GetLowestDivWithValue(div, innerText);

                    if (res != null)
                    {
                        return res;
                    }
                }
                else if (div.get_inner_text().Contains(innerText) && result.Count == 2)
                {
                    return div;
                }
            }

            return null;
        }

        private bool CheckKoef(string spanKoefString, double koef)
        {
            double spanKoefValue;

            return Double.TryParse(spanKoefString, NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out spanKoefValue) && spanKoefValue == koef;
        }
    }
}
