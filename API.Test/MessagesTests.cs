using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;
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

    [Test]
    public void GetMessagePermissions()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<MessagePermissions> permissions = Client.GetMessagePermissionsAsync();
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

        Task<MessagePreview[]> messages = Client.GetMessageInboxAsync();
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

        Task<MessagePreview[]> messages = Client.GetMessageInboxAsync();
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

        Task<Dictionary<string, MessagePerson[]>> people = Client.GetMessagePeopleAsync();
        people.Wait();
        if (people.Result.Count > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetDrafts()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<DraftPreview[]> drafts = Client.GetSavedDraftsAsync();
        drafts.Wait();
        if (drafts.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
