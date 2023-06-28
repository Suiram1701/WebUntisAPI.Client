using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthentificationTests;

namespace API.Test;

internal class MessagesTests
{
    [Test]
    public void GetUnreadMessages()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<int> messages = Client.GetUnreadMessagesCountAsync();
        messages.Wait();
        if (messages.Result == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
