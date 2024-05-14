using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;
using TuyaApp.Infrastructure.Consts;
using TuyaApp.Infrastructure.Extensions;
using TuyaApp.Infrastructure.Helpers;

namespace TuyaApp.Infrastructure.Concretes.Services
{
    public class TuyaCloudService : ITuyaCloudService
    {
        private readonly NotifyIcon _notifyIcon;
        private string token;

        public TuyaCloudService(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        // This method logs into the Tuya Cloud API and returns an access token
        private async Task<string> LoginAsync(Device device)
        {
            // Generate a timestamp and hash for the request
            string t = DateTime.Now.MillisecondsTimestamp();
            string hash = HashData.Hash(device.TuyaAccount.ClientId + t, device.TuyaAccount.Secret);

            // Create a REST client and request using the generated hash
            var options = new RestClientOptions(TuyaCloudEndpoint.TuyaCloudToken);
            options.MaxTimeout = -1;
            var client = new RestClient(options);
            //client.Timeout = -1;
            var request = RestRequestBuilder.GetRestRequest(device.TuyaAccount.ClientId, hash, t);

            // Execute the request and return the access token from the response
            var response = await client.ExecuteAsync(request);

            return response.TokenResponse();
        }

        // This method sends a request to the Tuya Cloud API to turn a device on or off
        public async Task<bool> ExecuteRequestAsync(Device device, string function, string action)
        {
            // If no access token exists, log in to Tuya Cloud and get a new one
            if (token is null)
                token = await LoginAsync(device);

            // Generate a timestamp and hash for the request using the access token
            string t = DateTime.Now.MillisecondsTimestamp();
            string hash = HashData.Hash(device.TuyaAccount.ClientId + token + t, device.TuyaAccount.Secret);

            // Create a REST client and request using the generated hash and access token
            var options = new RestClientOptions(TuyaCloudEndpoint.TuyaCloudDevice + device.DeviceTuyaId + "/commands");
            options.MaxTimeout = -1;
            var client = new RestClient(options);
            //client.Timeout = -1;
            var request = RestRequestBuilder.PostRestRequest(device.TuyaAccount.ClientId, hash, t, token);

            // Create a JSON payload with the switch code and value
            string jsonPayload = "{\n\t\"commands\":[\n\t\t{\n\t\t\t\"code\": \"" + function + "\",\n\t\t\t\"value\":" + action.ToLower() + "\n\t\t}\n\t]\n}";

            // Add the JSON payload to the request object
            request.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);

            // Execute the request asynchronously
            await client.ExecuteAsync(request);

            // Return true to indicate that the request was successful
            return true;
        }

        // This method sets the icon of the system tray based on the last state of the device
        public void SetIcon(bool lastState)
        {
            if (!lastState)
                _notifyIcon.Icon = new System.Drawing.Icon(@".\Resources\bulb_on.ico");
            else
            {
                _notifyIcon.Icon = new System.Drawing.Icon(@".\Resources\bulb_off.ico");
            }
        }

        // This method checks the last state of the device using the Tuya Cloud API
        public async Task<List<RestResultDto>> CheckLastStateAsync(Device device, string sw_number)
        {
            // Logs in if token is null
            if (token is null)
                token = await LoginAsync(device);

            // Generates a hash for the request
            string t = DateTime.Now.MillisecondsTimestamp();
            string hash = HashData.Hash(device.TuyaAccount.ClientId + token + t, device.TuyaAccount.Secret);

            // Creates a REST client and request
            var options = new RestClientOptions(TuyaCloudEndpoint.TuyaCloudDevice + device.DeviceTuyaId + "/status");
            options.MaxTimeout = -1;
            var client = new RestClient(options);
            //client.Timeout = -1;

            var request = RestRequestBuilder.GetRestRequest(device.TuyaAccount.ClientId, hash, t, token);

            // Executes the request and gets the response
            var response = await client.ExecuteAsync(request);

            // Deserializes the response
            var myDeserializedClass = response.DeserializeResponse();

            return myDeserializedClass.Result;

        }
    }
}
