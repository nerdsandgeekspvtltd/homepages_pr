using Microsoft.JSInterop;

namespace Sports.Events.WA.Services
{
    /// <summary>
    /// Interface for interacting with Azure Maps JavaScript API.
    /// </summary>
    public interface IAzureMapJs
    {
        /// <summary>
        /// Sets the .NET object reference for communication with JavaScript.
        /// </summary>
        /// <param name="objRef">The DotNetObjectReference representing the .NET object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetDotNetObj(Object obj);

        /// <summary>
        /// Initializes the map in the specified HTML container.
        /// </summary>
        /// <param name="containerId">The ID of the HTML container for the map.</param>
        /// <returns>Task representing the asynchronous operation. Returns an object representing the initialized map.</returns>
        Task<object> InitMap(string containerId);

        /// <summary>
        /// Clears all popups on the map.
        /// </summary>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task ClearPopups();

        /// <summary>
        /// Sets a popup at the specified coordinates on the map.
        /// </summary>
        /// <param name="content">The content of the popup.</param>
        /// <param name="longitude">The longitude of the popup location.</param>
        /// <param name="latitude">The latitude of the popup location.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SetPopUp(string content, double longitude, double latitude);

        /// <summary>
        /// Adds a marker at the specified coordinates on the map.
        /// </summary>
        /// <param name="longitude">The longitude of the marker location.</param>
        /// <param name="latitude">The latitude of the marker location.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task AddMarker(double longitude, double latitude);
    }
}
