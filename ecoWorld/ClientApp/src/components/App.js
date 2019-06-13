import React, { Component } from 'react';
import { Map, TileLayer, GeoJSON } from 'react-leaflet';
import SideBar from './SideBar';

const mapCenter = [39.9528, -75.1638];
const zoomLevel = 3;
const geojson = require('../custom.geo.json');
const maxScrol = 8;
const minScrol = 2;

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = { currentZoomLevel: zoomLevel };
        this.state = { contry: null };
        this.log = this.log.bind(this);
        this.renderCountries = this.renderCountries.bind(this);
        this.setColor = this.setColor.bind(this);
    }

    componentDidMount() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.on('zoomend', () => {
            const updatedZoomLevel = leafletMap.getZoom();
            this.handleZoomLevelChange(updatedZoomLevel);
        });
    }

    handleZoomLevelChange(newZoomLevel) {
        window.console.log(newZoomLevel);
        this.setState({ currentZoomLevel: newZoomLevel });
    }

    log(e) {
        var point = [e.latlng.lng, e.latlng.lat];
        var features = geojson.features.length;
        for (var i = 0; i < features; i++) {
            var feature = geojson.features[i];

            if (inside(point, feature.geometry.coordinates) === true) {
                this.setState({ contry: feature.properties.admin });
                break;
            }
        }
    }

    renderCountries(countryGeoJson) {
        var features = countryGeoJson.features.length;
        var contries = [];
        for (var i = 0; i < features; i++) {
            var feature = countryGeoJson.features[i];
            if (this.state.contry === feature.properties.admin) {
                this.setColor('#00008B', contries, i, feature);
            } else {
                this.setColor('#1a1d62', contries, i, feature);
            }

        }
        return contries;
    }

    setColor(colorValue, contries, i, feature) {
        let style3 = () => ({
            color: colorValue, weight: 0.5
        });
        contries[i] = <GeoJSON data={feature} style={style3} key={'setColor' + i} />;
    }

    render() {
        return (
            <div className="row">
                <div className="col-2">
                    <SideBar contry={this.state.contry}  />
                </div>
                <div className="col-10">
                    <Map
                        ref={m => { this.leafletMap = m; }}
                        center={mapCenter}
                        zoom={zoomLevel}
                        maxZoom={maxScrol}
                        minZoom={minScrol}
                        onClick={this.log}
                        maxBounds={[[-90, -260], [90, 260]]}
                    >    
                        <TileLayer attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                            url='http://{s}.tile.osm.org/{z}/{x}/{y}.png'
                            noWrap='false'
                        />

                        {this.renderCountries(geojson)}
                    </Map>
                </div>
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
            var yiGreater = yi > y;
            var yjGreater = yj > y;
            var intersect = yiGreater !== yjGreater
                && x < (xj - xi) * (y - yi) / (yj - yi) + xi;
            if (intersect) {
                inside = !inside;
            }
        }
        if (inside) {
            return true;
        }
    }
    return false;
}