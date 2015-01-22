﻿using Jojatekok.MoneroAPI.ProcessManagers;
using Jojatekok.MoneroAPI.RpcManagers;
using Jojatekok.MoneroAPI.Settings;
using System;

namespace Jojatekok.MoneroAPI
{
    public class MoneroClient : IDisposable
    {
        private RpcWebClient RpcWebClient { get; set; }
        private IPathSettings PathSettings { get; set; }
        /// <summary>Represents the settings of data fetching timers.</summary>
        public ITimerSettings TimerSettings { get; private set; }

        /// <summary>Contains methods to interact with the daemon.</summary>
        public DaemonManager Daemon { get; private set; }
        /// <summary>Contains methods to interact with the account manager.</summary>
        public AccountManager AccountManager { get; private set; }

        /// <summary>Creates a new instance of Monero API .NET's client service.</summary>
        /// <param name="pathSettings">Path settings to use for specifying the location of several files and folders.</param>
        /// <param name="rpcSettings">IP-related settings to use when communicating through the Monero core assemblies' RPC protocol.</param>
        /// <param name="timerSettings">Timer settings which can be used to alter data fetching intervals.</param>
        public MoneroClient(IPathSettings pathSettings = null, IRpcSettings rpcSettings = null, ITimerSettings timerSettings = null)
        {
            if (pathSettings == null) pathSettings = new PathSettings();
            if (rpcSettings == null) rpcSettings = new RpcSettings();
            if (timerSettings == null) timerSettings = new TimerSettings();

            RpcWebClient = new RpcWebClient(rpcSettings);
            PathSettings = pathSettings;
            TimerSettings = timerSettings;

            Daemon = new DaemonManager(RpcWebClient, pathSettings, timerSettings);
            AccountManager = new AccountManager(RpcWebClient, pathSettings, timerSettings, Daemon);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) {
                if (AccountManager != null) {
                    AccountManager.Dispose();
                    AccountManager = null;
                }

                if (Daemon != null) {
                    Daemon.Dispose();
                    Daemon = null;
                }
            }
        }
    }
}
