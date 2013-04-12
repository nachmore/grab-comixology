using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatiN.Core;
using WatiN.Core.Exceptions;

namespace DownloadComiXology
{
    class ComiXology
    {
        public string Url { get; private set; }
        public string Password { get; private set; }

        public ComiXology(string url, string password)
        {
            Url = url;
            Password = password;
        }

        ~ComiXology() 
        {
            if (_browser != null)
                _browser.Dispose();
        }

        IE _browser;

        public void Run()
        {
            _browser = new IE(Url);

            Login();

            // iterate through all sections
            var i = 0;

            while (true)
            {
                i++;

                if (!PurchaseCompleteSection(i)) 
                {
                    // we're done!
                    break;
                }
            }
       }

        private bool PurchaseCompleteSection(int index)
        {
            // cowboy style!
            _browser.WaitForComplete();
            Thread.Sleep(1000);

            while (true)
            {
                // get the current section
                // we reget it every time due to the changing HTML
                Div section;

                try
                {
                    section = _browser.Divs.Where(elem => elem.Id == "ahd" + index + "_container").First();
                }
                catch (InvalidOperationException)
                {
                    // i.e. we couldn't find the element, so the Where returned 0 items, and then threw on First()
                    return false;
                }

                var rows = section.ChildrenOfType<List>();

                foreach (var row in rows)
                {
                    foreach (var item in row.ChildrenOfType<ListItem>())
                    {
                        var link = item.Links[0];

                        // we've already been here, we're at the end of the section, progress to the next one
                        if (link.Text.Contains("In Cart"))
                            return true;
                        
                        link.Click();
                    }
                }

                // click the "Next" button
                try
                {
                    section.Link(index + "_AdHocNext").Click();
                }
                catch (ElementNotFoundException)
                {
                    // nothing to click, no next page of comics for this section, move to the next section
                    return true;
                }

                _browser.WaitForComplete();
                Thread.Sleep(3000);
            }

        }

        private void GoToNextSectionPage(int id)
        {
            _browser.Link("19_AdHocNext").Click();
        }

        private void Login()
        {
            var passwordBox = _browser.TextField(Find.ByName("password"));
            passwordBox.TypeText(Password);
            passwordBox.KeyPress('\r');
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        }
    }
}
