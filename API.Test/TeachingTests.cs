using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;

namespace API.Test;

[TestFixture]
internal class TeachingTests
{
    [Test]
    public async Task GetUnreadNewsAsync()
    {
        int unreadNewsCount = await SetUp.Client.GetUnreadNewsCountAsync();
        Assert.That(unreadNewsCount, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task GetNewsFeedAsync()
    {
        NewsWidget news = await SetUp.Client.GetNewsFeedAsync();
        Assert.That(news.MessagesOfTheDay.Count(), Is.GreaterThanOrEqualTo(0));
    }
}
