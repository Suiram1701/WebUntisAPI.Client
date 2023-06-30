using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using static API.Test.AuthentificationTests;

namespace API.Test;

internal class MessagesTests
{
    [Test]
    public void GetUnreadMessages()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<int> messages = Client.MessageClient.GetUnreadMessagesCountAsync();
        messages.Wait();
        if (messages.Result == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetMessagePermissions()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<MessagePermissions> permissions = Client.MessageClient.GetMessagePermissionsAsync();
        permissions.Wait();
        if (permissions.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetMessageInbox()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<MessagePreview[]> messages = Client.MessageClient.GetMessageInboxAsync();
        messages.Wait();
        if (messages.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetFullMessage()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<MessagePreview[]> messages = Client.MessageClient.GetMessageInboxAsync();
        messages.Wait();
        Task<Message> msg = messages.Result.First(msg => msg.Subject == "Test").GetFullMessageAsync(Client);
        msg.Wait();
        _ = msg.Result;
        return;
    }

    [Test]
    public void GetReceptionPeople()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<KeyValuePair<string, MessagePerson[]>[]> people = Client.MessageClient.GetMessagePeopleAsync();
        people.Wait();
        if (people.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
