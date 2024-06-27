window.mapInterop = {
    initMap: function (mapId, lat,long) {
        var map = L.map(mapId).setView([lat, long], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);
        return map;
    },
    addMarker: function (map, latitude, longitude) {
        L.marker([latitude, longitude]).addTo(map);
    },
    getMarkers: function (map) {
        var markers = [];
        map.eachLayer(function (layer) {
            if (layer instanceof L.Marker) {
                markers.push(layer.getLatLng());
            }
        });
        return markers;
    }
};