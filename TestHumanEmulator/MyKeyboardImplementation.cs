using System.Reflection;
using XHE;
using XHE.XHE_System;

namespace TestHumanEmulator
{
    public class MyKeyboardImplementation : XHEKeyboard
    {
        public MyKeyboardImplementation(string server, string password, XHEScriptBase script) : base(server, password, script)
        {
        }

        public bool my_send_input(string string_, string timeout = "0:2", bool inFlash = false, bool auto_change = true)
        {
            string[,] aParams = new string[4, 2]
            {
                { "string", string_ },
                { "timeout", timeout },
                {
                    "inFlash",
                    inFlash.ToString()
                },
                {
                    "auto_change",
                    auto_change.ToString()
                }
            };
            bool result = call_boolean(MethodBase.GetCurrentMethod()!.Name, aParams);

            return result;
        }

        public bool Input(string string_, string timeout = "10:30", bool inFlash = false, bool auto_change = true)
        {
            string[,] aParams = new string[4, 2]
            {
                { "string", string_ },
                { "timeout", timeout },
                {
                    "inFlash",
                    inFlash.ToString()
                },
                {
                    "auto_change",
                    auto_change.ToString()
                }
            };
            bool result = call_boolean("input", aParams);
            Thread.Sleep(1000);

            return result;
        }

        /// <summary>
        /// Ctrl+A+Delete
        /// </summary>
        public void DeleteAllTextFromInput()
        {
            send_key("65", false, true);
            send_key("46", false, false);
        }
    }
}
