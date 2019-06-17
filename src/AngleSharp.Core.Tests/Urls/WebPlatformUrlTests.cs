namespace AngleSharp.Core.Tests.Urls
{
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using System.Collections;

    public class UrlTestFixture
    {
        public string Input;
        public string Base;
        public string Href;
        public string Origin;
        public string Protocol;
        public string Username;
        public string Password;
        public string Host;
        public string Hostname;
        public string Port;
        public string Pathname;
        public string Search;
        public string Hash;
        public bool Failure;
        public string Description;
    }

    [TestFixtureSource(typeof(UrlFixtureData), "UrlFixtures")]
    public class WebPlatFormUrlTests
    {
        private UrlTestFixture _test;
        private string FailureMessage = "Input: {0}\nBase: {1}\nTarget: {2}";

        public WebPlatFormUrlTests(UrlTestFixture test)
        {
            _test = test;
            if (!string.IsNullOrEmpty(test.Description))
            {
                this.FailureMessage = string.Format("Description: {0}\n{1}", test.Description, FailureMessage);
            }
        }

        [Test]
        public void WebPlatFormUrlTest()
        {
            var baseurl = new Url(_test.Base);
            var url = new Url(baseurl, _test.Input);
            if (_test.Failure)
            {
                Assert.IsTrue(url.IsInvalid, string.Format(FailureMessage, _test.Input, _test.Base, "Failure"));
                Assert.Pass(string.Format("{0}\nResult: {1}", string.Format(FailureMessage, _test.Input, _test.Base, "Failure"), url.IsInvalid));
            }
            else
            {
                if (_test.Href != null) Assert.AreEqual(_test.Href, url.Href ?? "null", string.Format(FailureMessage, _test.Input, _test.Base, "Href"));
                if (_test.Origin != null) Assert.AreEqual(_test.Origin, url.Origin ?? "null", string.Format(FailureMessage, _test.Input, _test.Base, "Origin"));
                if (_test.Protocol != null) Assert.AreEqual(_test.Protocol, url.Scheme + ":", string.Format(FailureMessage, _test.Input, _test.Base, "Scheme"));
                if (_test.Username != null) Assert.AreEqual(_test.Username, url.UserName ?? string.Empty, string.Format(FailureMessage, _test.Input, _test.Base, "UserName"));
                if (_test.Password != null) Assert.AreEqual(_test.Password, url.Password ?? string.Empty, string.Format(FailureMessage, _test.Input, _test.Base, "Password"));
                if (_test.Host != null) Assert.AreEqual(_test.Host, url.Host, string.Format(FailureMessage, _test.Input, _test.Base, "Host"));
                if (_test.Hostname != null) Assert.AreEqual(_test.Hostname, url.HostName, string.Format(FailureMessage, _test.Input, _test.Base, "HostName"));
                if (_test.Port != null) Assert.AreEqual(_test.Port, url.Port ?? string.Empty, string.Format(FailureMessage, _test.Input, _test.Base, "Port"));
                if (_test.Pathname != null) Assert.AreEqual(_test.Pathname, string.IsNullOrEmpty(url.Path) ? "/" : url.Path, string.Format(FailureMessage, _test.Input, _test.Base, "Path"));
                if (_test.Search != null) Assert.AreEqual(_test.Search, url.Query ?? string.Empty, string.Format(FailureMessage, _test.Input, _test.Base, "Search"));
                if (_test.Hash != null) Assert.AreEqual(_test.Hash, url.Fragment ?? string.Empty, string.Format(FailureMessage, _test.Input, _test.Base, "Hash"));
                Assert.Pass(string.Format("{0}\nResult: {1}", string.Format(FailureMessage, _test.Input, _test.Base, "Href"), url.Href));
            }
        }
    }

    public class UrlFixtureData
    {
        public static IEnumerable UrlFixtures
        {
            get
            {
                JArray cases = JArray.Parse(Assets.GetManifestResourceString("Urls.urltestdata.json"));
                string nextDescription = string.Empty;

                foreach (JToken token in cases)
                {
                    if (token.HasValues)
                    {
                        var test = (token as JObject).ToObject<UrlTestFixture>();
                        if (!string.IsNullOrEmpty(nextDescription))
                        {
                            test.Description = nextDescription;
                            nextDescription = string.Empty;
                        }
                        yield return new TestFixtureData(test);
                    }
                    else
                    {
                        nextDescription = token.ToString();
                    }
                }
            }
        }
    }
}
