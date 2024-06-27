// Initialize variables to store references to .NET object and map
var dotNetObjRef = null;
var map = null;

// Define an object to hold JavaScript functions for Blazor Interop
window.myBlazorInterop = {

    // Function to set the reference to the .NET object
    setDotNetObj: function (_dotNetObjRef) {
        dotNetObjRef = _dotNetObjRef;
    },

    // Function to initialize the map
    initMap: function (subscriptionKey, containerId) {

        try {


            // Create a new Azure Maps instance and assign it to the 'map' variable
            map = new atlas.Map(containerId, {
                center: [79.4192, 13.6288], // Initial map center
                zoom: 12, // Initial zoom level
                view: 'Auto', // Use auto view
                style: 'road', // Map style
                authOptions: {
                    authType: 'subscriptionKey',
                    subscriptionKey: subscriptionKey // Azure Maps subscription key
                }
            });

            // Add click event listener to the map
            map.events.add("click", function (e) {
                console.log(e);

                // Extract longitude and latitude from the event
                var long = e.position[0];
                var lat = e.position[1];

                // Call the 'OnMapLocation' method in the .NET object
                dotNetObjRef.invokeMethodAsync('OnMapLocation', long, lat);
            });

        } catch (e) {
            console.log(e);
        }

        // Return the map instance
        return map;
    },

    // Function to set a popup on the map
    setPopup: function (_content, _longitude, _latitude) {
        // Create a new popup with the provided content and position
        var popup = new atlas.Popup({
            content: _content, // Popup content
            position: [_longitude, _latitude], // Popup position
            fillColor: 'rgba(0,0,0,0.8)', // Popup fill color
            closeButton: false // Disable close button
        });

        // Open the popup on the map
        popup.open(map);
    },

    // Function to add a marker on the map
    addMarker: function (_longitude, _latitude) {
        // Create a new HTML marker at the provided position
        var marker = new atlas.HtmlMarker({
            color: 'DodgerBlue', // Marker color
            position: [_longitude, _latitude] // Marker position
        });

        // Add the marker to the map
        map.markers.add(marker);
    },


    // Function to clear all popups and markers from the map
    clearPoups: function () {
        map.popups.clear(); // Clear all popups
        map.markers.clear(); // Clear all markers
    }
};
