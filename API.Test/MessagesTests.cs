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
        Assert.DoesNotThrowAsync(async delegate
        {
            _ = await SetUp.Client.GetUnreadMessagesCountAsync();
        });
    }

    [Test]
    public async Task GetMessagePermissionsAsync()
    {
        MessagePermissions permissions = await SetUp.Client.GetMessagePermissionsAsync();
        Assert.Multiple(() =>
        {
            Assert.That(permissions.ShowSentTab);
            Assert.That(permissions.ShowDraftsTab);
            Assert.That(permissions.MaxFileSize, Is.GreaterThan(0L));
        });
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
    public async Task GetTeacherRecipientsAsync()
    {
        Dictionary<string, IEnumerable<MessagePerson>> persons = await SetUp.Client.GetTeacherRecipientsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(persons.Select(kv => kv.Key), Is.Unique.And.Not.Empty);
            Assert.That(persons.Select(kv => kv.Value.Select(mp => mp.Id)), Is.All.Unique);
        });
    }

    [Test]
    public async Task GetStaffRecipientsFiltersAsync()
    {
        Dictionary<string, IEnumerable<FilterItem>> filters = await SetUp.Client.GetStaffRecipientsSearchFiltersAsync();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Select(kv => kv.Key), Is.Unique.And.Not.Empty);
            Assert.That(filters.Select(kv => kv.Value.Select(f => f.ReferenceId)), Is.All.Unique);
        });
    }

    [Test]
    public async Task GetStaffRecipientsAsync()
    {
        IEnumerable<MessagePerson> people = await SetUp.Client.GetStaffRecipientsAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(people.Count(), Is.GreaterThan(0));
            Assert.That(people.Select(p => p.Id), Is.Unique);
        });
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
