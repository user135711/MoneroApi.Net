﻿using Jojatekok.MoneroAPI.RpcUtilities;

namespace Jojatekok.MoneroAPI.RpcManagers
{
    public abstract class BaseRpcManager
    {
        private RpcWebClient RpcWebClient { get; set; }

        private string RpcHost { get; set; }
        private ushort RpcPort { get; set; }

        internal BaseRpcManager(RpcWebClient rpcWebClient, bool isDaemon) {
            RpcWebClient = rpcWebClient;

            var rpcSettings = rpcWebClient.RpcSettings;

            if (isDaemon) {
                RpcHost = rpcSettings.UrlHostDaemon;
                RpcPort = rpcSettings.UrlPortDaemon;
            } else {
                RpcHost = rpcSettings.UrlHostAccountManager;
                RpcPort = rpcSettings.UrlPortAccountManager;
            }
        }
        protected T HttpPostData<T>(string command, JsonRpcRequest jsonRpcRequest = null) where T : HttpRpcResponse
        {
            var output = RpcWebClient.HttpPostData<T>(RpcHost, RpcPort, command, jsonRpcRequest);
            if (output != null && output.Status == RpcResponseStatus.Ok) {
                return output;
            }

            return null;
        }

        protected bool HttpPostData(string command, JsonRpcRequest jsonRpcRequest = null)
        {
            var output = HttpPostData<HttpRpcResponse>(command, jsonRpcRequest);
            if (output != null && output.Status == RpcResponseStatus.Ok) {
                return true;
            }

            return false;
        }

        protected JsonRpcResponse<T> JsonPostData<T>(JsonRpcRequest jsonRpcRequest) where T : class
        {
            return RpcWebClient.JsonPostData<T>(RpcHost, RpcPort, jsonRpcRequest);
        }

        protected bool JsonPostData(JsonRpcRequest jsonRpcRequest)
        {
            var output = JsonPostData<object>(jsonRpcRequest);
            return output != null && output.Error == null;
        }
    }
}
