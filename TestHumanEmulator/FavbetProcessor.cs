using System.Globalization;
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

            //var currencyString = span.get_by_outer_html(CurrencyOuterHtml, NotExect).get_inner_text().Split()[1];

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
            var divs = div.get_all_by_inner_html("<div class=\"Box_box__3oOkB OutcomeButton_outcome__S21im OutcomeButton_table__2FxD6 OutcomeButton_desktopStyles__2SqAq Box_justify_between__YVIct Box_align_center__1nUqG\"", NotExact);
            divs.meta_click();

            var divsByBetType = div.get_all_by_inner_text("Handicap", Exect).get_next();

            //var scnd = div.get_all_by_number($"{divsByBetType.get_number()}").get_all_by_inner_text("2nd", NotExact).get_next()[0];
            var secondHaldDiv = divsByBetType[0].get_all_child_by_inner_text("2nd Half", NotExact, true).get_number().Count;

            if (secondHaldDiv > 0)
            {
                //var test1 = divsByBetType.get_all_child_by_inner_text("2.5", NotExact, true).get_number();
                //var test2 = divsByBetType.get_all_child_by_inner_text("1.16", NotExact, true).get_number();
                //var test1last = test1.Last();
                //var test2last = test2.Last();

                //if (test1last == test2last + 1 || test1last == test2last - 1)
                //{
                //    var test3 = 
                //    test3.meta_click();
                //}

                var betDivs = divsByBetType[0].get_child_by_inner_html("<div class=\"Box_box__3oOkB OutcomeButton_outcome__S21im OutcomeButton_table__2FxD6 OutcomeButton_desktopStyles__2SqAq Box_justify_between__YVIct Box_align_center__1nUqG\" style=\"padding: 0px 8px;\"", 0, true);

                betDivs.meta_click();
            }
            //secondHaldDiv.set_attribute("value", "1231232");


            //var divsByPeriod = div.get_all_by_number($"{divsByBetType[0].get_number()}").get_all_by_inner_text("2nd", NotExact).get_next();

            //var divsByBetParam1 = divsByPeriod[0].get_child_by_inner_text("2.5", NotExact);
            //var divsByBetParam2 = divsByPeriod[0].get_child_by_inner_text("1.16", NotExact);

            //divsByBetParam2.meta_click();

            //if (divsByBetParam1 == divsByBetParam2)
            //{
            //    divsByBetParam2.meta_click();
            //}

            //var divsByBetParam = div.get_all_by_number($"{divsByPeriod[0].get_number()}").get_all_by_inner_text("2.5", NotExact);

            //divsByBetParam.click();
            //divsByBetParam.meta_click();

            //divsByBetParam.meta_click();

            //var targetDiv = divsByBetParam.get_all_by_inner_text("1.29", Exect);
            //Period
            //var divsByPeriod = divsByBetType[0].get_all_by_inner_text("2nd", NotExact);

            //BetParam
            //var divsByBetParam = divsByPeriod.get_all_by_inner_text("3.5", NotExact);




            span.wait_element_exist_by_attribute(ClassAttribute, FullTimeFirstTeamWinClass, NotExact);
            browser.wait_for();
            Thread.Sleep(2000);

            var betSpan = span.get_by_attribute(ClassAttribute, FullTimeFirstTeamWinClass, NotExact);

            if (CheckKoef(betSpan.get_inner_text(), koef))
            {
                Console.WriteLine("betClick: " + betSpan.meta_click());

                input.get_by_attribute(ClassAttribute, PlaceBetInputClass).meta_click();

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
                Console.WriteLine("Koefs don't match");

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

        private bool CheckKoef(string spanKoefString, double koef)
        {
            double spanKoefValue;

            return Double.TryParse(spanKoefString, NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out spanKoefValue) && spanKoefValue == koef;
        }
    }
}
