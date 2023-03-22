using TestHumanEmulator;

namespace XHE
{
    public class FavbetProcessor : ProcessorBase
    {
        private const string LoginPage = "https://www.favbet.ua/uk/login/?from=header-desktop";

        public FavbetProcessor(string server, string password = "") : base(server, password)
        {

        }

        protected override bool Login()
        {
            if(!browser.navigate(LoginPage))
                return false;

            if (anchor.wait_element_exist_by_attribute("href", "https://www.favbet.ua/uk/personal-office/balance/deposit/"))
                return true;

            Console.WriteLine($"wait input email: {input.wait_element_exist_by_name("email")}");

            Console.WriteLine($"click email: {input.get_by_id("email").meta_click()}");

            Console.WriteLine($"input email text: {MyKeyboard.Input("sergebaben@gmail.com")}");

            Console.WriteLine($"click password: {input.get_by_id("password").meta_click()}");

            Console.WriteLine($"input password text: {MyKeyboard.Input("_BjBPjxtb_WMi3")}");

            return btn.click_by_inner_html("<span class=\"Box_box__3oOkB Bu", 0);
        }

        protected override bool CheckBalance(double betAmount)
        {
            var res1 = span.get_by_xpath("/html/body/div[1]/div/div/div[1]/div[1]/div/div[3]/div[2]/div[1]/div/span/span[2]/span/span[1]").get_inner_html();

            var res2 = span.get_by_xpath("/html/body/div[1]/div/div/div[1]/div[1]/div/div[3]/div[2]/div[1]/div/span/span[2]/span/span[1]").get_inner_html();

            //var result1 = res1.);
            //var result2 = res2.get_value();

            var res3 = span.get_by_outer_html("<span class=\"Text_base__3OOOi ", 0).get_inner_text().Split()[0];
            double result = double.Parse(res3, System.Globalization.CultureInfo.InvariantCulture);
            double result2 = double.Parse("120.67", System.Globalization.CultureInfo.InvariantCulture);

            return true;
        }

        protected override void PlaceBet()
        {
            throw new NotImplementedException();
        }
    }
}
