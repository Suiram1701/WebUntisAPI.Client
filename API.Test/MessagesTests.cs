using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;

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
    public async Task GetTeacherRecipientsAsync()
    {
        Dictionary<string, IEnumerable<MessagePerson>> persons = await SetUp.Client.GetTeacherRecipientsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(persons.Select(kv => kv.Key), Is.Unique.And.Not.Empty);
            Assert.That(persons.SelectMany(kv => kv.Value.Select(mp => mp.Id)), Is.Unique);
        });
    }

    [Test]
    public async Task GetStaffRecipientsFiltersAsync()
    {
        Dictionary<string, IEnumerable<FilterItem>> filters = await SetUp.Client.GetStaffRecipientsSearchFiltersAsync();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Select(kv => kv.Key), Is.Unique.And.Not.Empty);
            foreach (IEnumerable<FilterItem> items in filters.Values)
                Assert.That(items, Is.Unique);
        });
    }

    [Test]
    public async Task GetStaffRecipientsAsync()
    {
        IEnumerable<MessagePerson> people = await SetUp.Client.GetStaffRecipientsAsync(null, null);

        Assert.Multiple(() =>
        {
            Assert.That(people.Count(), Is.GreaterThan(0));
            Assert.That(people.Select(p => p.Id), Is.Unique);
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
    public async Task GetMessageInboxAsync()
    {
        IEnumerable<InboxMessagePreview> messages = await SetUp.Client.GetMessageInboxAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no messages in the inbox available.");

        Assert.Multiple(() =>
        {
            Assert.That(messages.Select(m => m.Id), Is.Unique);
            Assert.That(messages.Select(m => m.SentDateTime), Is.Ordered.Descending);
        });
    }

    [Test]
    public async Task GetSentMessagesAsync()
    {
        IEnumerable<SentMessagePreview> messages = await SetUp.Client.GetSentMessagesAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no sent messages available.");

        Assert.Multiple(() =>
        {
            Assert.That(messages.Select(m => m.Id), Is.Unique);
            Assert.That(messages.Select(m => m.SentDateTime), Is.Ordered.Descending);
        });
    }

    [Test]
    public async Task GetDraftMessagesAsync()
    {
        IEnumerable<DraftMessagePreview> messages = await SetUp.Client.GetSavedDraftsAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no drafts available.");

        Assert.That(messages.Select(m => m.Id), Is.Unique);
    }

    [Test]
    public async Task GetFullInboxMessageAsync()
    {
        IEnumerable<InboxMessagePreview> messages = await SetUp.Client.GetMessageInboxAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no messages in the inbox available.");

        InboxMessagePreview preview = messages.First();
        InboxMessage message = await SetUp.Client.GetFullMessageAsync(preview);
        AssertEqual(preview, message);
    }

    [Test]
    public async Task GetFullSentMessageAsync()
    {
        IEnumerable<SentMessagePreview> messages = await SetUp.Client.GetSentMessagesAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no sent messages available.");

        SentMessagePreview preview = messages.First();
        SentMessage message = await SetUp.Client.GetFullMessageAsync(preview);
        AssertEqual(preview, message);
    }

    [Test]
    public async Task GetFullDraftMessageAsync()
    {
        IEnumerable<DraftMessagePreview> messages = await SetUp.Client.GetSavedDraftsAsync();

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no drafts available.");

        DraftMessagePreview preview = messages.First();
        DraftMessage message = await SetUp.Client.GetFullMessageAsync(preview);
        AssertEqual(preview, message);
    }

    private static void AssertEqual(IMessagePreview preview, IMessage message)
    {
        Assert.Multiple(() =>
        {
            Assert.That(preview, Is.Not.Null);
            Assert.That(message, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(preview.Id, Is.EqualTo(message.Id));
            Assert.That(preview.Subject, Is.EqualTo(message.Subject));
            Assert.That(preview.SentDateTime, Is.EqualTo(message.SentDateTime));
            Assert.That(preview.HasAttachments, Is.EqualTo(message.Attachments.Any()));
            Assert.That(preview.AllowDeletion, Is.EqualTo(message.AllowDeletion));
        });
    }

    [Test]
    public async Task GetReplyFormAsync()
    {
        IEnumerable<InboxMessagePreview> messages = (await SetUp.Client.GetMessageInboxAsync()).Where(m => m.IsReplyAllowed);

        if (!messages.Any())
            Assert.Ignore("Could not run test because there no messages to reply available.");

        InboxMessagePreview preview = messages.First();
        MessageReplyForm replyForm = await SetUp.Client.GetReplyFormAsync(preview);

        Assert.Multiple(() =>
        {
            Assert.That(replyForm, Is.Not.Null);
            Assert.That(preview.Id, Is.EqualTo(replyForm.Id));
            Assert.That(preview.Subject, Is.EqualTo(replyForm.Subject));
            Assert.That(preview.Sender.DisplayName, Is.EqualTo(replyForm.Recipient.DisplayName));
        });

        ReplyMessage? replyMessage = replyForm.ReplyHistory.FirstOrDefault(r => r.Id.Equals(preview.Id));
        Assert.That(replyMessage, Is.Not.Null);
    }
}
