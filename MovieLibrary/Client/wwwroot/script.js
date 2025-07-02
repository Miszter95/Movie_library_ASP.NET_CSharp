let map, infoWindow;

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 47.497913, lng: 19.040236 },
        zoom: 10,
    });
    infoWindow = new google.maps.InfoWindow();

    const locationButton = document.createElement("button");

    locationButton.textContent = "Pan to Current Location and see Nearby Movie Theater with Red Marker";
    locationButton.style.height = "30px";
    locationButton.classList.add("custom-map-control-button");
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(locationButton);
    locationButton.addEventListener("click", () => {

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };

                    var request = {
                        location: new google.maps.LatLng(pos),
                        radius: '2000',
                        type: ['movie_theater']
                    };

                    service = new google.maps.places.PlacesService(map);
                    service.nearbySearch(request, callback);


                    infoWindow.setPosition(pos);
                    infoWindow.setContent("You are here!");
                    infoWindow.open(map);
                    map.setZoom(14);
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }
    });
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service failed."
            : "Error: Your browser doesn't support geolocation."
    );
    infoWindow.open(map);
}

window.initMap = initMap;

function callback(results, status) {
    if (status == google.maps.places.PlacesServiceStatus.OK) {
        for (var i = 0; i < results.length; i++) {

            new google.maps.Marker({
                position: results[i].geometry.location,
                map,
                title: "Movie_theater",
            });

            console.log(results[i]);
        }
    }
    return results;
}
