﻿using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Kahla.SDK.Data;
using Kahla.SDK.Events;
using Kahla.SDK.Models;
using Kahla.SDK.Models.ApiViewModels;
using Kahla.SDK.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace Kahla.SDK.Abstract
{
    public abstract class BotBase
    {
        public AES AES;
        public BotLogger BotLogger;
        public GroupsService GroupsService;
        public ConversationService ConversationService;
        public FriendshipService FriendshipService;
        public AuthService AuthService;
        public HomeService HomeService;
        public KahlaLocation KahlaLocation;
        public VersionService VersionService;
        public SettingsService SettingsService;
        public EventSyncer EventSyncer;
        public BotCommander BotCommander;

        public ManualResetEvent ExitEvent;
        public SemaphoreSlim ConnectingLock = new SemaphoreSlim(1);

        public KahlaUser Profile { get; set; }

        public virtual Task OnBotInit() => Task.CompletedTask;

        public virtual Task OnFriendRequest(NewFriendRequestEvent arg) => Task.CompletedTask;

        public virtual Task OnFriendsChangedEvent(FriendsChangedEvent arg) => Task.CompletedTask;

        public virtual Task OnGroupConnected(SearchedGroup group) => Task.CompletedTask;

        public virtual Task OnMessage(string inputMessage, NewMessageEvent eventContext) => Task.CompletedTask;

        public virtual Task OnGroupInvitation(int groupId, NewMessageEvent eventContext) => Task.CompletedTask;

        public virtual Task OnWasDeleted(FriendDeletedEvent typedEvent) => Task.CompletedTask;

        public virtual Task OnGroupDissolve(DissolveEvent typedEvent) => Task.CompletedTask;

        public virtual Task OnMemoryChanged() => Task.CompletedTask;

        public async Task Start(bool enableCommander)
        {
            if (enableCommander)
            {
                var _ = Connect().ConfigureAwait(false);
                await BotCommander.Command();
            }
            else
            {
                await Connect();
            }
        }

        public async Task Connect()
        {
            await ConnectingLock.WaitAsync();
            BotLogger.LogWarning("Establishing the connection to Kahla...");
            ExitEvent?.Set();
            ExitEvent = null;
            var server = AskServerAddress();
            SettingsService["ServerAddress"] = server;
            KahlaLocation.UseKahlaServer(server);
            if (!await TestKahlaLive(server))
            {
                return;
            }
            if (!await SignedIn())
            {
                await OpenSignIn();
                var code = await AskCode();
                await SignIn(code);
            }
            else
            {
                BotLogger.LogSuccess($"\nYou are already signed in! Welcome!");
            }
            await RefreshUserProfile();
            await OnBotInit();
            var websocketAddress = await GetWSAddress();
            // Trigger on request.
            var requests = (await FriendshipService.MyRequestsAsync())
                .Items
                .Where(t => !t.Completed);
            foreach (var request in requests)
            {
                await OnFriendRequest(new NewFriendRequestEvent
                {
                    Request = request
                });
            }
            // Trigger group connected.
            var friends = (await FriendshipService.MineAsync());
            foreach (var group in friends.Groups)
            {
                await OnGroupConnected(group);
            }
            ConnectingLock.Release();
            await MonitorEvents(websocketAddress);
            return;
        }


        public string AskServerAddress()
        {
            var cached = SettingsService["ServerAddress"] as string;
            if (!string.IsNullOrWhiteSpace(cached))
            {
                return cached;
            }
            BotLogger.LogInfo("Welcome! Please enter the server address of Kahla.");
            var result = BotLogger.ReadLine("\r\nEnter 1 for production\r\nEnter 2 for staging\r\nFor other server, enter like: https://server.kahla.app\r\n");
            if (result.Trim() == 1.ToString())
            {
                return "https://server.kahla.app";
            }
            else if (result.Trim() == 2.ToString())
            {
                return "https://staging.server.kahla.app";
            }
            else
            {
                return result;
            }
        }

        public async Task<bool> TestKahlaLive(string server)
        {
            try
            {
                BotLogger.LogInfo($"Using Kahla Server: {KahlaLocation}");
                BotLogger.LogInfo("Testing Kahla server connection...");
                var index = await HomeService.IndexAsync(server);
                BotLogger.AppendResult(true, 5);
                //BotLogger.LogSuccess("Success! Your bot is successfully connected with Kahla!\r\n");
                BotLogger.LogInfo($"Server time: \t{index.UTCTime}\tServer version: \t{index.APIVersion}");
                BotLogger.LogInfo($"Local time: \t{DateTime.UtcNow}\tLocal version: \t\t{VersionService.GetSDKVersion()}");
                if (index.APIVersion != VersionService.GetSDKVersion())
                {
                    BotLogger.AppendResult(false, 1);
                    BotLogger.LogDanger("API version don't match! Kahla bot may crash! We strongly suggest checking the API version first!");
                }
                else
                {
                    BotLogger.AppendResult(true, 1);
                }
                return true;
            }
            catch (Exception e)
            {
                BotLogger.LogDanger(e.Message);
                return false;
            }
        }

        public async Task<bool> SignedIn()
        {
            var status = await AuthService.SignInStatusAsync();
            return status.Value;
        }

        public async Task OpenSignIn()
        {
            BotLogger.LogInfo($"Signing in to Kahla...");
            var address = await AuthService.OAuthAsync();
            BotLogger.LogWarning($"Please open your browser to view this address: ");
            address = address.Split('&')[0] + "&redirect_uri=https%3A%2F%2Flocalhost%3A5000";
            BotLogger.LogWarning(address);
            //410969371
        }

        public async Task<int> AskCode()
        {
            int code;
            while (true)
            {
                await Task.Delay(10);
                var codeString = BotLogger.ReadLine($"Please enther the `code` in the address bar(after signing in):").Trim();
                if (!int.TryParse(codeString, out code))
                {
                    BotLogger.LogDanger($"Invalid code! Code is a number! You can find it in the address bar after you sign in.");
                    continue;
                }
                break;
            }
            return code;
        }

        public async Task SignIn(int code)
        {
            while (true)
            {
                try
                {
                    BotLogger.LogInfo($"Calling sign in API...");
                    var response = await AuthService.SignIn(code);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        BotLogger.AppendResult(true, 7);
                        break;
                    }
                }
                catch (WebException)
                {
                    BotLogger.AppendResult(false, 7);
                    BotLogger.LogDanger($"Invalid code!");
                    code = await AskCode();
                }
            }
        }

        public async Task RefreshUserProfile()
        {
            try
            {
                BotLogger.LogInfo($"Getting account profile...");
                var profile = await AuthService.MeAsync();
                BotLogger.AppendResult(true, 6);
                Profile = profile.Value;
            }
            catch (AiurUnexceptedResponse e)
            {
                BotLogger.AppendResult(false, 6);
                BotLogger.LogDanger(e.Message);
            }
        }

        public async Task<string> GetWSAddress()
        {
            BotLogger.LogInfo($"Getting websocket channel...");
            var address = await AuthService.InitPusherAsync();
            BotLogger.AppendResult(true, 6);
            return address.ServerPath;
        }

        public async Task MonitorEvents(string websocketAddress)
        {
            bool okToStop = false;
            if (ExitEvent != null)
            {
                BotLogger.LogDanger("Bot is trying to establish a new connection while there is already a connection.");
                return;
            }
            ExitEvent = new ManualResetEvent(false);
            var url = new Uri(websocketAddress);
            var client = new WebsocketClient(url)
            {
                ReconnectTimeout = TimeSpan.FromDays(1)
            };
            client.ReconnectionHappened.Subscribe(type => BotLogger.LogVerbose($"WebSocket: {type.Type}"));
            client.DisconnectionHappened.Subscribe(t =>
            {
                if (!okToStop)
                {
                    okToStop = true;
                    BotLogger.LogDanger("Websocket connection dropped! Auto retry...");
                    var _ = Connect().ConfigureAwait(false);
                }
            });
            await EventSyncer.Init(client, this);
            await client.Start();
            BotLogger.LogInfo($"Listening to your account channel.");
            BotLogger.LogVerbose(websocketAddress + "\n");
            BotLogger.AppendResult(true, 9);
            ExitEvent.WaitOne();
            okToStop = true;
            await client.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);
            BotLogger.LogVerbose("Websocket connection disconnected.");
        }

        public Task CompleteRequest(int requestId, bool accept)
        {
            var text = accept ? "accepted" : "rejected";
            BotLogger.LogWarning($"Friend request with id '{requestId}' was {text}.");
            return FriendshipService.CompleteRequestAsync(requestId, accept);
        }

        public Task MuteGroup(string groupName, bool mute)
        {
            var text = mute ? "muted" : "unmuted";
            BotLogger.LogWarning($"Group with name '{groupName}' was {text}.");
            return GroupsService.SetGroupMutedAsync(groupName, mute);
        }

        public async Task SendMessage(string message, int conversationId, string aesKey)
        {
            var encrypted = AES.OpenSSLEncrypt(message, aesKey);
            await ConversationService.SendMessageAsync(encrypted, conversationId);
        }

        public async Task JoinGroup(string groupName, string password)
        {
            try
            {
                var result = await GroupsService.JoinGroupAsync(groupName, password);
                var group = await GroupsService.GroupSummaryAsync(result.Value);
                await OnGroupConnected(group.Value);
            }
            catch (AiurUnexceptedResponse e) when (e.Code == ErrorType.HasDoneAlready)
            {
                // do nothing.
            }
        }

        protected string RemoveMentionMe(string sourceMessage)
        {
            sourceMessage = sourceMessage.Replace($"@{Profile.NickName.Replace(" ", "")}", "");
            return sourceMessage;
        }

        protected string Mention(KahlaUser target)
        {
            return $" @{target.NickName.Replace(" ", "")}";
        }

        public async Task LogOff()
        {
            ExitEvent?.Set();
            ExitEvent = null;
            await AuthService.LogoffAsync();
        }
    }
}
