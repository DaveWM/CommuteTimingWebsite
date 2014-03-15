angular.module('myServices', [])
    .factory('googleMapsService', function($q) {
        return function() {
            return {
                PromiseLatLngFromAddress: function(address) {
                    var deferred = $q.defer();
                    geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ 'address': address, 'region': 'uk' },
                        function(results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                deferred.resolve(results);
                            } else deferred.resolve([]);
                        });
                    return deferred.promise;
                }
            };
        };
    })
    .factory('journeyService', function ($q, $window) {
        return function() {
            return {
                StartRecord: function(route) {
                    var deferred = $q.defer();
                    $.post($window.AddJourneyUrl, { routeID: route.ID }, function(newJourney) {
                        route.Journeys.push(newJourney);
                        deferred.resolve(newJourney);
                    });
                    return deferred.promise;
                },
                EndRecord: function(route) {
                    var deferred = $q.defer();
                    var current = this.CurrentJourney(route);
                    $.post($window.EndJourneyUrl, { journeyID: current.ID }, function(endDate) {
                        current.EndDate = endDate;
                        deferred.resolve(endDate);
                    });
                    return deferred.promise;
                },
                CurrentJourney: function(route) {
                    return Enumerable.From(route.Journeys).Where(function(j) {
                        return !j.EndDate;
                    }).FirstOrDefault();
                },
                GetJourneyTime: function(journey) {
                    if (!journey)
                        return moment.duration(0);
                    var startMoment = moment(journey.StartDate);
                    var endMoment = !journey.EndDate ? moment() : moment(journey.EndDate);
                    return moment.duration(endMoment.diff(startMoment));
                },
                FormatDuration: function(duration) {
                    var hours = $window.Math.floor(duration.asHours());
                    var mins = duration.minutes();
                    var secs = duration.seconds();
                    return String("00" + hours).slice(-2) + ':' + String("00" + mins).slice(-2) + ':' + String("00" + secs).slice(-2);
                },

                TimeAvg: function (route) {
                    var whatThisShouldBe = this;
                    var avgMillis = Enumerable.From(route.Journeys).Average(function (j) {
                        return whatThisShouldBe.GetJourneyTime(j).asMilliseconds();
                    });
                    return this.FormatDuration(moment.duration(avgMillis));
                },
                TimeSD: function (route) {
                    var whatThisShouldBe = this;
                    var sdMillis = Enumerable.From(route.Journeys).StandardDeviation(function (j) {
                        return whatThisShouldBe.GetJourneyTime(j).asMilliseconds();
                    });
                    return this.FormatDuration(moment.duration(sdMillis));
                }
            };
        };
    });