﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Cronofy.Test.CronofyAccountClientTests
{
    [TestFixture]
    public sealed class UpsertEvent
    {
        private const string clientId = "abcdef123456";
        private const string clientSecret = "s3cr3t1v3";
        private const string accessToken = "zyxvut987654";

        private const string calendarId = "cal_123456_abcdef";

        private CronofyAccountClient client;
        private StubHttpClient http;

        [SetUp]
        public void SetUp()
        {
            this.client = new CronofyAccountClient(accessToken);
            this.http = new StubHttpClient();

            client.HttpClient = http;
        }

        [Test]
        public void CanUpsertEvent()
        {
            const string eventId = "qTtZdczOccgaPncGJaCiLg";
            const string summary = "Board meeting";
            const string description = "Discuss plans for the next quarter";
            const string startTimeString = "2014-08-05 15:30:00Z";
            const string endTimeString = "2014-08-05 17:00:00Z";
            const string locationDescription = "Board room";

            http.Stub(
                HttpPost
                .Url("https://api.cronofy.com/v1/calendars/" + calendarId + "/events")
                .RequestHeader("Authorization", "Bearer " + accessToken)
                .RequestHeader("Content-Type", "application/json; charset=utf-8")
                .RequestBodyFormat(
                    "{{\"event_id\":\"{0}\"," +
                    "\"summary\":\"{1}\"," +
                    "\"description\":\"{2}\"," +
                    "\"start\":{{\"time\":\"{3}\",\"tzid\":\"Etc/UTC\"}}," +
                    "\"end\":{{\"time\":\"{4}\",\"tzid\":\"Etc/UTC\"}}," +
                    "\"location\":{{\"description\":\"{5}\"}}" +
                    "}}",
                    eventId,
                    summary,
                    description,
                    startTimeString,
                    endTimeString,
                    locationDescription)
                .ResponseCode(202)
            );

            var builder = new UpsertEventRequestBuilder()
                .EventId(eventId)
                .Summary(summary)
                .Description(description)
                .Start(new DateTime(2014, 8, 5, 15, 30, 0, DateTimeKind.Utc))
                .End(new DateTime(2014, 8, 5, 17, 0, 0, DateTimeKind.Utc))
                .Location(locationDescription);

            client.UpsertEvent(calendarId, builder);
        }

        [Test]
        public void CanUpsertEventWithoutLocation()
        {
            const string eventId = "qTtZdczOccgaPncGJaCiLg";
            const string summary = "Board meeting";
            const string description = "Discuss plans for the next quarter";
            const string startTimeString = "2014-08-05 15:30:00Z";
            const string endTimeString = "2014-08-05 17:00:00Z";

            http.Stub(
                HttpPost
                .Url("https://api.cronofy.com/v1/calendars/" + calendarId + "/events")
                .RequestHeader("Authorization", "Bearer " + accessToken)
                .RequestHeader("Content-Type", "application/json; charset=utf-8")
                .RequestBodyFormat(
                    "{{\"event_id\":\"{0}\"," +
                    "\"summary\":\"{1}\"," +
                    "\"description\":\"{2}\"," +
                    "\"start\":{{\"time\":\"{3}\",\"tzid\":\"Etc/UTC\"}}," +
                    "\"end\":{{\"time\":\"{4}\",\"tzid\":\"Etc/UTC\"}}" +
                    "}}",
                    eventId,
                    summary,
                    description,
                    startTimeString,
                    endTimeString)
                .ResponseCode(202)
            );

            var builder = new UpsertEventRequestBuilder()
                .EventId(eventId)
                .Summary(summary)
                .Description(description)
                .Start(new DateTime(2014, 8, 5, 15, 30, 0, DateTimeKind.Utc))
                .End(new DateTime(2014, 8, 5, 17, 0, 0, DateTimeKind.Utc));

            client.UpsertEvent(calendarId, builder);
        }

        [Test]
        public void CanUpsertEventWithTimeZoneId()
        {
            const string eventId = "qTtZdczOccgaPncGJaCiLg";
            const string summary = "Board meeting";
            const string description = "Discuss plans for the next quarter";
            const string startTimeString = "2014-08-05 15:30:00Z";
            const string endTimeString = "2014-08-05 17:00:00Z";
            const string timeZoneId = "Europe/London";

            http.Stub(
                HttpPost
                .Url("https://api.cronofy.com/v1/calendars/" + calendarId + "/events")
                .RequestHeader("Authorization", "Bearer " + accessToken)
                .RequestHeader("Content-Type", "application/json; charset=utf-8")
                .RequestBodyFormat(
                    "{{\"event_id\":\"{0}\"," +
                    "\"summary\":\"{1}\"," +
                    "\"description\":\"{2}\"," +
                    "\"start\":{{\"time\":\"{3}\",\"tzid\":\"{5}\"}}," +
                    "\"end\":{{\"time\":\"{4}\",\"tzid\":\"{5}\"}}" +
                    "}}",
                    eventId,
                    summary,
                    description,
                    startTimeString,
                    endTimeString,
                    timeZoneId)
                .ResponseCode(202)
            );

            var builder = new UpsertEventRequestBuilder()
                .EventId(eventId)
                .Summary(summary)
                .Description(description)
                .Start(new DateTime(2014, 8, 5, 15, 30, 0, DateTimeKind.Utc))
                .End(new DateTime(2014, 8, 5, 17, 0, 0, DateTimeKind.Utc))
                .TimeZoneId(timeZoneId);

            client.UpsertEvent(calendarId, builder);
        }

        [Test]
        public void CanUpsertEventWithSeparateTimeZoneIds()
        {
            const string eventId = "qTtZdczOccgaPncGJaCiLg";
            const string summary = "Board meeting";
            const string description = "Discuss plans for the next quarter";
            const string startTimeString = "2014-08-05 15:30:00Z";
            const string endTimeString = "2014-08-05 17:00:00Z";
            const string startTimeZoneId = "Europe/London";
            const string endTimeZoneId = "America/Chicago";

            http.Stub(
                HttpPost
                .Url("https://api.cronofy.com/v1/calendars/" + calendarId + "/events")
                .RequestHeader("Authorization", "Bearer " + accessToken)
                .RequestHeader("Content-Type", "application/json; charset=utf-8")
                .RequestBodyFormat(
                    "{{\"event_id\":\"{0}\"," +
                    "\"summary\":\"{1}\"," +
                    "\"description\":\"{2}\"," +
                    "\"start\":{{\"time\":\"{3}\",\"tzid\":\"{4}\"}}," +
                    "\"end\":{{\"time\":\"{5}\",\"tzid\":\"{6}\"}}" +
                    "}}",
                    eventId,
                    summary,
                    description,
                    startTimeString,
                    startTimeZoneId,
                    endTimeString,
                    endTimeZoneId)
                .ResponseCode(202)
            );

            var builder = new UpsertEventRequestBuilder()
                .EventId(eventId)
                .Summary(summary)
                .Description(description)
                .Start(new DateTime(2014, 8, 5, 15, 30, 0, DateTimeKind.Utc))
                .StartTimeZoneId(startTimeZoneId)
                .End(new DateTime(2014, 8, 5, 17, 0, 0, DateTimeKind.Utc))
                .EndTimeZoneId(endTimeZoneId);

            client.UpsertEvent(calendarId, builder);
        }
    }
}
