
angular.module('maps',['myServices']).directive('googleMap',function () {
    return {
        restrict: 'E',
        scope: {
            waypoints: '=',
            height: '@',
            onDoubleClick: '&'
        },
        template: '<div ng-style="{height: height}" class="googleMap"><\div>',
        link: function ($scope, element, attrs, controller) {
                google.maps.visualRefresh = true;

                var mapOptions = {
                    center: new google.maps.LatLng(-34.397, 150.644),
                    zoom: 1,
                    disableDoubleClickZoom: true
                };
                var map = new google.maps.Map(element[0], mapOptions);
                
                function SetMapCentre() {
                    if ($scope.waypoints.length) {
                        var minLat = Enumerable.From($scope.waypoints).Min(function (w) { return w.Latitude; });
                        var maxLat = Enumerable.From($scope.waypoints).Max(function (w) { return w.Latitude; });
                        var minLng = Enumerable.From($scope.waypoints).Min(function (w) { return w.Longitude; });
                        var maxLng = Enumerable.From($scope.waypoints).Max(function (w) { return w.Longitude; });
                        map.fitBounds(new google.maps.LatLngBounds(new google.maps.LatLng(minLat, minLng), new google.maps.LatLng(maxLat, maxLng)));
                    } else if (navigator.geolocation) {
                        navigator.geolocation.getCurrentPosition(function(pos) {
                            map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
                            map.setZoom(14);
                        });
                    }
                }

            SetMapCentre();

            google.maps.event.addListener(map, 'dblclick', function(event) {
                    event.stop();
                    $scope.onDoubleClick()(event.latLng.lat(), event.latLng.lng());
                    $scope.$apply();
                });
                var markers = [];
                var journeyLine = null;
                $scope.$watch('waypoints', function (newWPs, oldWPs) {
                    SetMapCentre();
                    $.each(newWPs, function(index, waypoint) {
                        if (!Enumerable.From(markers).Any(function(m) {
                            return m.waypointID == waypoint.ID;
                        })) {
                            var marker = new google.maps.Marker({
                                position: new google.maps.LatLng(waypoint.Latitude, waypoint.Longitude),
                                title: waypoint.Name,
                                map: map,
                                draggable: true,
                                animation: google.maps.Animation.DROP
                            });
                            marker.waypointID = waypoint.ID;
                            google.maps.event.addListener(marker, 'dragend', function(event) {
                                var wp = Enumerable.From(newWPs).Where(function(w) {
                                    return w.ID == marker.waypointID;
                                }).FirstOrDefault();
                                wp.Latitude = event.latLng.lat();
                                wp.Longitude = event.latLng.lng();
                                $scope.$apply();
                            });
                            markers.push(marker);
                        } else {
                            var marker = Enumerable.From(markers).Where(function(m) { return m.waypointID == waypoint.ID; }).FirstOrDefault();
                            marker.setTitle(waypoint.Name);
                            marker.setPosition(new google.maps.LatLng(waypoint.Latitude, waypoint.Longitude));
                        }
                    });
                    $.each(oldWPs, function(index, wp) {
                        if (!Enumerable.From(newWPs).Any(function(w) { return w.ID == wp.ID })) {
                            var marker = Enumerable.From(markers).Where(function(m) { return m.waypointID == wp.ID; }).FirstOrDefault();
                            if (marker != null) {
                                marker.setMap(null);
                                marker = null;
                            }
                        }
                    });
                    var coords = Enumerable.From(newWPs).OrderBy(function(w) { return w.Index; }).Select(function(w) { return new google.maps.LatLng(w.Latitude, w.Longitude); }).ToArray();
                    if (!journeyLine) {
                        journeyLine = new google.maps.Polyline({
                            path: coords,
                            geodesic: true,
                            strokeColor: '#FF0000',
                            strokeOpacity: 1.0,
                            strokeWeight: 2
                        });

                        journeyLine.setMap(map);
                    } else {
                        var lineCoords = journeyLine.getPath();
                        var length = lineCoords.length;
                        for (var i = 0; i < length; i++) {
                            lineCoords.removeAt(0);
                        }
                        $.each(coords, function(index, item) {
                            lineCoords.push(item);
                        });
                    }

                }, true);
        }
        ,replace:true
    };
}).directive('geocoder',function (googleMapsService,$timeout) {
    return {
        restrict: 'E',
        scope: {
            address: '=ngModel',
            onResultSelected : '&',
            typingTimeout : '@'
        },
        template: '<div class="list-group"><a class="list-group-item" ng-repeat="result in GeocoderResults" ng-click="onResultSelected()(result)" tooltip-placement="right" tooltip="Click to Add to Waypoints" tooltip-trigger="mouseenter" tooltip-append-to-body="true"><i class="fa fa-chevron-right"></i> {{result.formatted_address}}</a><\div>',
        replace:true,
        link: function($scope, element, attrs, controller) {

            var timeoutPromise=null;

            $scope.Results = [];
            $scope.$watch('address', function(address, oldVal) {
                if (address && address != oldVal) {
                    if (timeoutPromise) {
                        $timeout.cancel(timeoutPromise);
                    }
                    timeoutPromise = $timeout(function () {
                        googleMapsService().PromiseLatLngFromAddress(address).then(function (results) {
                            $scope.GeocoderResults = results;
                        });
                    }, $scope.typingTimeout);
                }
                else if (!address)
                    $scope.GeocoderResults = null;
            });
        }
    };
})