using Microsoft.JSInterop;
using Sports.Events.WA.Models;
using System;
using System.Threading.Tasks;

namespace Sports.Events.WA.Services
{
    /// <summary>
    /// Service for interacting with Azure Maps JavaScript functions.
    /// </summary>
    public class AzureMapJs : IAzureMapJs
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureMapJs"/> class.
        /// </summary>
        /// <param name="jsRuntime">The JavaScript runtime.</param>
        public AzureMapJs(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Sets the reference to the .NET object in JavaScript.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetDotNetObj(Object obj)
        {
            await _jsRuntime.InvokeVoidAsync("myBlazorInterop.setDotNetObj", obj);
        }

        /// <summary>
        /// Initializes the map.
        /// </summary>
        /// <param name="containerId">The ID of the container element.</param>
        /// <returns>An object representing the initialized map.</returns>
        public async Task<object> InitMap(string containerId)
        {
            object map;
            try
            {
                map = await _jsRuntime.InvokeAsync<object>("myBlazorInterop.initMap", Utlities.subscriptionKey, containerId);
            }
            catch (Exception ex)
            {
                map = new();
            }
            return map;
        }

        /// <summary>
        /// Clears all popups from the map.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ClearPopups()
        {
            await _jsRuntime.InvokeVoidAsync("myBlazorInterop.clearPoups");
        }

        /// <summary>
        /// Sets a popup on the map at the specified coordinates.
        /// </summary>
        /// <param name="content">The content of the popup.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetPopUp(string content, double longitude, double latitude)
        {
            await _jsRuntime.InvokeVoidAsync("myBlazorInterop.setPopup", content, longitude, latitude);
        }

        /// <summary>
        /// Adds a marker to the map at the specified coordinates.
        /// </summary>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddMarker(double longitude, double latitude)
        {
            await _jsRuntime.InvokeVoidAsync("myBlazorInterop.addMarker", longitude, latitude);
        }
    }
}
