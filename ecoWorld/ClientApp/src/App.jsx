import React, { createRef, Component } from 'react';
import { render } from 'react-dom';
import { Map, TileLayer, GeoJSON } from 'react-leaflet';
import Control from 'react-leaflet-control';
import geojson from 'json-loader!./custom.geojson';
const mapCenter = [39.9528, -75.1638];
const zoomLevel = 2;
const minZoomLevel = 2;
const maxZoomLevel = 2;

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = { currentZoomLevel: zoomLevel };
        this.handleUpPanClick = this.handleUpPanClick.bind(this);
        this.handleRightPanClick = this.handleRightPanClick.bind(this);
        this.handleLeftPanClick = this.handleLeftPanClick.bind(this);
        this.handleDownPanClick = this.handleDownPanClick.bind(this);
        this.log = this.log.bind(this);
    }


    componentDidMount() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.on('zoomend', () => {
            const updatedZoomLevel = leafletMap.getZoom();
            this.handleZoomLevelChange(updatedZoomLevel);
        });
    }

    handleZoomLevelChange(newZoomLevel) {
        this.setState({ currentZoomLevel: newZoomLevel });
    }

    handleUpPanClick() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.panBy([0, -100]);
        window.console.log('Panning up');
    }

    handleRightPanClick() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.panBy([100, 0]);
        window.console.log('Panning right');
    }

    handleLeftPanClick() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.panBy([-100, 0]);
        window.console.log('Panning left');
    }

    handleDownPanClick() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.panBy([0, 100]);
        window.console.log('Panning down');
    }




    log(e) {
        var point = [e.latlng.lng, e.latlng.lat];
        var features = geojson.features.length;
        window.console.log(geojson);
        for (var i = 0; i < features; i++) {
            var feature = geojson.features[i];

            if (inside(point, feature.geometry.coordinates) == true) {
                window.console.log('Found it ' + feature.properties.admin);
                break;
            }
        }
        window.console.log("Lat, Lon : " + e.latlng);
    }




    render() {
        window.console.log('this.state.currentZoomLevel ->', this.state.currentZoomLevel);
        return (
            <div>
                <Map
                    ref={m => { this.leafletMap = m; }}
                    center={mapCenter}
                    zoom={zoomLevel}
                    onLocationfound={this.handleLocationFound}
                    onClick={this.log}

                >
                    <TileLayer
                        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                        url='http://{s}.tile.osm.org/{z}/{x}/{y}.png'
                    />
                    <GeoJSON data={geojson}
                    />
                </Map>

            </div>
        );
    }
}

function inside(point, area) {
    var numA = area.length;
    for (var iter = 0; iter < numA; iter++) {
         var vs; 
        if (numA > 1) {
           vs = area[iter][0];
        } else {
           vs = area[0];
        }
        // ray-casting algorithm based on
        // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html

        var x = point[0], y = point[1];
        var inside = false;
        for (var i = 0, j = vs.length - 1; i < vs.length; j = i++) {
            var xi = vs[i][0], yi = vs[i][1];
            var xj = vs[j][0], yj = vs[j][1];
            var intersect = ((yi > y) != (yj > y))
                && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
            if (intersect) {
                inside = !inside;
            }
        }
        if (inside) {
            return true;
        }
    }
    return false;
};

render(
    <App />,
    document.getElementById('mount')
);
