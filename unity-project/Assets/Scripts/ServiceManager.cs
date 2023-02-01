using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceManager : MonoBehaviour
{
    private LocationPlugin plugin;
    [SerializeField]
    private Text latText;
    [SerializeField]
    private Text lngText;
    [SerializeField]
    private Text altText;
    [SerializeField]
    private Text accuracyText;
    [SerializeField]
    private Text bearingText;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private Text providerText;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private Text locationAvailabilityText;
    [SerializeField]
    private Text timeText;

    private List<LocationAndTime> _locations = new List<LocationAndTime>();
    private LocationData _lastLocation = new LocationData();

    public struct LocationAndTime
    {
        public DateTime Time;
        public LocationData Location;
    }

    private void Awake()
    {
        plugin = GetComponentInChildren<LocationPlugin>();
    }

    private void Start()
    {
        plugin.OnLocation += OnLocationReceived;
        plugin.OnAvailability += OnLocationAvailability;
        plugin.OnDistanceChanged += OnDistanceChanged;

        Destination destination = new Destination();
        destination.destinationName = "Post";
        destination.latitude = 47.984641;
        destination.longitude = 8.815055;
        destination.triggerRadius = 40;
        plugin.setDestination(destination);
    }

    public void OnStartLocationServiceBtn()
    {
        plugin.StartLocationService(5000, 3000, 10);
    }

    private void OnLocationReceived(LocationData _location)
    {
        Debug.Log($"Lat: {_location.latitude} Lng: {_location.longitude} Alt: {_location.altitude}");
        if (_lastLocation.latitude != _location.latitude &&
            _lastLocation.longitude != _location.longitude)
        {
            // On it didnt run background 
            //Debug.Log($"NEW LOCATION: {_location.latitude},{_location.longitude}");
            _lastLocation = _location;
        }
        var newLoc = new LocationAndTime();
        newLoc.Location = _location;
        newLoc.Time = DateTime.Now;
        _locations.Add(newLoc);
        WriteLocationToUI(_location);
    }

    private void OnLocationAvailability(bool _isAvailable)
    {
        locationAvailabilityText.text = _isAvailable.ToString();
    }

    private void OnDistanceChanged(double _distance)
    {
        distanceText.text = $"Distance to Dest: {_distance} m";
    }

    private void OnApplicationPause(bool _isPaused)
    {
        if (!_isPaused)
        {
            WriteLocationToUI(plugin.LastLocation);
        }
    }

    private void WriteLocationToUI(LocationData _location)
    {
        latText.text = "Latitude: " + _location.latitude.ToString();
        lngText.text = "Longitude: " + _location.longitude.ToString();
        altText.text = "Altitude: " + (plugin.HasAltitude() ? _location.altitude.ToString() : "-");
        accuracyText.text = "Accuracy: " + (plugin.HasAccuracy() ? _location.accuracy.ToString() : "-");
        bearingText.text = "Bearing: " + (plugin.HasBearing() ? _location.bearing.ToString() : "-");
        speedText.text = "Speed: " + (plugin.HasSpeed() ? _location.speed.ToString() : "-");
        providerText.text = "Provider: " + _location.provider.ToString();
        timeText.text = DateTime.Now.ToString();
    }

    public void GetLocations()
    {
        if (_locations.Count > 0)
        {
            _locations.ForEach((x) =>
            {
                Debug.Log($"{x.Time} {x.Location.latitude}, {x.Location.longitude}");
            });
        }
    }
}
