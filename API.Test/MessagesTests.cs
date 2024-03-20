using NUnit.Framework;
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
        Task<int> messages = SetUp.Client.GetUnreadMessagesCountAsync();
        messages.Wait();
        if (messages.Result == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetMessagePermissions()
    {
        Task<MessagePermissions> permissions = SetUp.Client.GetMessagePermissionsAsync();
        permissions.Wait();
        if (permissions.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetMessageInbox()
    {
        Task<(MessagePreview[], MessagePreview[])> messages = SetUp.Client.GetMessageInboxAsync();
        messages.Wait();
        if (messages.Result.Item1.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetSentMessages()
    {
        Task<MessagePreview[]> messages = SetUp.Client.GetSentMessagesAsync();
        messages.Wait();
        if (messages.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetFullMessage()
    {
        Task<(MessagePreview[], MessagePreview[])> messages = SetUp.Client.GetMessageInboxAsync();
        messages.Wait();
        Task<Message> msg = messages.Result.Item1.First(msg => msg.Subject == "Test").GetFullMessageAsync(SetUp.Client);
        msg.Wait();
        _ = msg.Result;
        return;
    }

    [Test]
    public void GetReceptionPeople()
    {
        Task<Dictionary<string, MessagePerson[]>> people = SetUp.Client.GetMessagePeopleAsync();
        people.Wait();
        if (people.Result.Count > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetStaffFilters()
    {
        Task<Dictionary<string, FilterItem[]>> filters = SetUp.Client.GetStaffSearchFiltersAsync();
        filters.Wait();

        if (filters.Result.Count > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetSearchedStaffPeople()
    {
        Task<MessagePerson[]> filters = SetUp.Client.GetStaffFilterSearchResultAsync("", new());
        filters.Wait();

        if (filters.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetDrafts()
    {
        Task<DraftPreview[]> drafts = SetUp.Client.GetSavedDraftsAsync();
        drafts.Wait();
        if (drafts.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetReplyForm()
    {
        Task<(MessagePreview[], MessagePreview[])> messages = SetUp.Client.GetMessageInboxAsync();
        messages.Wait();

        Task<Message> drafts = SetUp.Client.GetReplyFormAsync(messages.Result.Item1[0]);
        drafts.Wait();
        if (drafts.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
