﻿/*
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Microsoft.Extensions.Configuration;

namespace SendPushSample {
    class Program {
        private const string FcmSampleNotificationContent = "{\"data\":{\"message\":\"Notification Hub test notification from SDK sample\"}}";
        private const string FcmSampleSilentNotificationContent = "{ \"message\":{\"data\":{ \"Nick\": \"Mario\", \"body\": \"great match!\", \"Room\": \"PortugalVSDenmark\" } }}";
        private const string AppleSampleNotificationContent = "{\"aps\":{\"alert\":\"Notification Hub test notification from SDK sample\"}}";
        private const string AppleSampleSilentNotificationContent = "{\"aps\":{\"content-available\":1}, \"foo\": 2 }";
        private const string WnsSampleNotification = "<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">Notification Hub test notification from SDK sample</text></binding></visual></toast>";

        static async Task Main(string[] args) {
            // Getting connection key from the new resource
            var config = LoadConfiguration(args);
            var nhClient = NotificationHubClient.CreateClientFromConnectionString(config.PrimaryConnectionString, config.HubName);

            // Register some fake devices
            var fcmDeviceId = Guid.NewGuid().ToString();
            var fcmInstallation = new Installation {
                InstallationId = "fake-fcm-install-id",
                Platform = NotificationPlatform.Fcm,
                PushChannel = fcmDeviceId,
                PushChannelExpired = false,
                Tags = new[] { "fcm" }
            };
            await nhClient.CreateOrUpdateInstallationAsync(fcmInstallation);

            var appleDeviceId = "00fc13adff785122b4ad28809a3420982341241421348097878e577c991de8f0";
            var apnsInstallation = new Installation {
                InstallationId = "fake-apns-install-id",
                Platform = NotificationPlatform.Apns,
                PushChannel = appleDeviceId,
                PushChannelExpired = false,
                Tags = new[] { "apns" }
            };
            await nhClient.CreateOrUpdateInstallationAsync(apnsInstallation);

            switch ((SampleConfiguration.Operation)Enum.Parse(typeof(SampleConfiguration.Operation), config.SendType)) {
                case SampleConfiguration.Operation.Broadcast:
                    // Notification groups should be created on client side
                    var outcomeFcm = await nhClient.SendFcmNativeNotificationAsync(FcmSampleNotificationContent);
                    var outcomeSilentFcm = await nhClient.SendFcmNativeNotificationAsync(FcmSampleSilentNotificationContent);
                    var fcmOutcomeDetails = await WaitForThePushStatusAsync("FCM", nhClient, outcomeFcm);
                    var fcmSilentOutcomeDetails = await WaitForThePushStatusAsync("FCM", nhClient, outcomeSilentFcm);
                    PrintPushOutcome("FCM", fcmOutcomeDetails, fcmOutcomeDetails.FcmOutcomeCounts);
                    PrintPushOutcome("FCM Silent ", fcmSilentOutcomeDetails, fcmSilentOutcomeDetails.FcmOutcomeCounts);

                    // Send groupable notifications to iOS
                    var notification = new AppleNotification(AppleSampleNotificationContent);
                    if (!string.IsNullOrEmpty(config.AppleGroupId)) {
                        notification.Headers.Add("apns-collapse-id", config.AppleGroupId);
                    }

                    var outcomeApns = await nhClient.SendNotificationAsync(notification);
                    var outcomeSilentApns = await nhClient.SendAppleNativeNotificationAsync(AppleSampleSilentNotificationContent);
                    var apnsOutcomeDetails = await WaitForThePushStatusAsync("APNS", nhClient, outcomeApns);
                    var apnsSilentOutcomeDetails = await WaitForThePushStatusAsync("APNS", nhClient, outcomeSilentApns);
                    PrintPushOutcome("APNS", apnsOutcomeDetails, apnsOutcomeDetails.ApnsOutcomeCounts);
                    PrintPushOutcome("APNS Silent", apnsSilentOutcomeDetails, apnsSilentOutcomeDetails.ApnsOutcomeCounts);

                    var outcomeWns = await nhClient.SendWindowsNativeNotificationAsync(WnsSampleNotification);
                    var wnsOutcomeDetails = await WaitForThePushStatusAsync("WNS", nhClient, outcomeWns);
                    PrintPushOutcome("WNS", wnsOutcomeDetails, wnsOutcomeDetails.WnsOutcomeCounts);

                    break;
                case SampleConfiguration.Operation.SendByTag:
                    // Send notifications by tag
                    var outcomeFcmByTag = await nhClient.SendFcmNativeNotificationAsync(FcmSampleNotificationContent, config.Tag ?? "fcm");
                    var fcmTagOutcomeDetails = await WaitForThePushStatusAsync("FCM Tags", nhClient, outcomeFcmByTag);
                    PrintPushOutcome("FCM Tags", fcmTagOutcomeDetails, fcmTagOutcomeDetails.FcmOutcomeCounts);

                    var outcomeApnsByTag = await nhClient.SendAppleNativeNotificationAsync(AppleSampleNotificationContent, config.Tag ?? "apns");
                    var apnsTagOutcomeDetails = await WaitForThePushStatusAsync("APNS Tags", nhClient, outcomeApnsByTag);
                    PrintPushOutcome("APNS Tags", apnsTagOutcomeDetails, apnsTagOutcomeDetails.ApnsOutcomeCounts);

                    break;
                case SampleConfiguration.Operation.SendByDevice:
                    // Send notifications by deviceId
                    var outcomeFcmByDeviceId = await nhClient.SendDirectNotificationAsync(CreateFcmNotification(), config.FcmDeviceId ?? fcmDeviceId);
                    var fcmDirectSendOutcomeDetails = await WaitForThePushStatusAsync("FCM direct", nhClient, outcomeFcmByDeviceId);
                    PrintPushOutcome("FCM Direct", fcmDirectSendOutcomeDetails, fcmDirectSendOutcomeDetails.ApnsOutcomeCounts);

                    var outcomeApnsByDeviceId = await nhClient.SendDirectNotificationAsync(CreateApnsNotification(), config.AppleDeviceId ?? appleDeviceId);
                    var apnsDirectSendOutcomeDetails = await WaitForThePushStatusAsync("APNS direct", nhClient, outcomeApnsByDeviceId);
                    PrintPushOutcome("APNS Direct", apnsDirectSendOutcomeDetails, apnsDirectSendOutcomeDetails.ApnsOutcomeCounts);

                    break;
                default:
                    Console.WriteLine("Invalid Sendtype");
                    break;
            }
        }

        private static Notification CreateFcmNotification() {
            return new FcmNotification(FcmSampleNotificationContent);
        }

        private static Notification CreateApnsNotification() {
            return new AppleNotification(AppleSampleNotificationContent);
        }

        private static async Task<NotificationDetails> WaitForThePushStatusAsync(string pnsType, NotificationHubClient nhClient, NotificationOutcome notificationOutcome) {
            var notificationId = notificationOutcome.NotificationId;
            var state = NotificationOutcomeState.Enqueued;
            var count = 0;
            NotificationDetails outcomeDetails = null;
            while ((state == NotificationOutcomeState.Enqueued || state == NotificationOutcomeState.Processing) && ++count < 10) {
                try {
                    Console.WriteLine($"{pnsType} status: {state}");
                    outcomeDetails = await nhClient.GetNotificationOutcomeDetailsAsync(notificationId);
                    state = outcomeDetails.State;
                }
                catch (MessagingEntityNotFoundException) {
                    // It's possible for the notification to not yet be enqueued, so we may have to swallow an exception
                    // until it's ready to give us a new state.
                }
                Thread.Sleep(1000);
            }
            return outcomeDetails;
        }

        private static void PrintPushOutcome(string pnsType, NotificationDetails details, NotificationOutcomeCollection collection) {
            if (collection != null) {
                Console.WriteLine($"{pnsType} outcome: " + string.Join(",", collection.Select(kv => $"{kv.Key}:{kv.Value}")));
            } else {
                Console.WriteLine($"{pnsType} no outcomes.");
            }
            Console.WriteLine($"{pnsType} error details URL: {details.PnsErrorDetailsUri}");
        }

        private static SampleConfiguration LoadConfiguration(string[] args) {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("config.json", true);
            configurationBuilder.AddCommandLine(args);
            var configRoot = configurationBuilder.Build();
            var sampleConfig = new SampleConfiguration();
            configRoot.Bind(sampleConfig);
            return sampleConfig;
        }
    }
}
//*/