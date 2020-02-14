import React, { Component } from 'react';
import { Map, TileLayer, GeoJSON } from 'react-leaflet';
import SideBar from './SideBar';
import LineMap from './LineMap';
import "leaflet-polylinedecorator";
import L from "leaflet";

const mapCenter = [39.9528, -75.1638];
const zoomLevel = 3;
const geojson = require('../custom.geo.json');
const maxScrol = 8;
const minScrol = 2;


var arrow = [
    {
        offset: "100%",
        repeat: 0,
        symbol: L.Symbol.arrowHead({
            pixelSize: 15,
            polygon: false,
            pathOptions: { stroke: true }
        })
    }
];

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = { currentZoomLevel: zoomLevel };
        this.state = { contry: null };
        this.state = { mapData: null };
        this.state = { arrowMap: [[[57, -19], [60, -12]], [[30, -120], [30, 0]]] };
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
        if (this.state.mapData !== null && this.state.mapData !== undefined) {
            for (var i = 0; i < features; i++) {
                var feature = countryGeoJson.features[i];

                for (var c = 0; c < this.state.mapData.length - 1; c++) {
                    var split = this.state.mapData[c].split(':');
                    if (split[0] === feature.properties.admin) {
                        this.setColor('#00' + split[1] + '00', contries, i, feature, 1);
                        break;
                    }
                }
            }
        } else {
            for (i = 0; i < features; i++) {

                feature = countryGeoJson.features[i];
                if (this.state.contry === feature.properties.admin) {
                    this.setColor('#00008B', contries, i, feature, 0.5);
                } else {
                    this.setColor('#1a1d62', contries, i, feature, 0.5);
                }
            }

        }
        return contries;
    }

    setColor(colorValue, contries, i, feature, inputWeight) {
        let style3 = () => ({
            color: colorValue, weight: inputWeight
        });
        contries[i] = <GeoJSON data={feature} style={style3} key={'setColor' + i} />;
    }

    arrowMapCallBack = (childData) => {
        if (childData === null) {
            this.setState({
                arrowMap: []
            });
        } else {
            arrow.remove();
            arrow = [
                {
                    offset: "100%",
                    repeat: 0,
                    symbol: L.Symbol.arrowHead({
                        pixelSize: 15,
                        polygon: false,
                        pathOptions: { stroke: true }
                    })
                }
            ];
            this.setState({
                arrowMap: childData
            });
        }
    }

    mapCallBack = (childData) => {
        if (childData === null) {
            this.setState({
                mapData: null
            });
            return;
        }
        var contries = childData.split(";");
        var max = 0;
        var min = 100000000;
        var totalt = 0;

        for (var i = 0; i < contries.length - 1; i++) {
            var num = parseFloat(contries[i].split(":")[1].replace(',', '.'));
            if (num < min) {
                min = num;
            }
            if (num > max) {
                max = num;
            }
            totalt += num;
        }
        avg = (avg + max) / 2;
        var avg = totalt / contries.length;
        var avgAdj = avg / 128;
        var maxAdj = (max - avg) / 127;
        for (i = 0; i < contries.length - 1; i++) {
            var split = contries[i].split(':');

            var name = split[0];
            num = parseFloat(contries[i].split(":")[1].replace(',', '.'));
            var result;
            if (num < avg) {
                result = (num / avgAdj).toString(16).split(".")[0];
            } else {
                result = ((num - avg) / maxAdj + 128).toString(16).split(".")[0];
            }
            if (result.length === 1) {
                result = "0" + result;
            }

            contries[i] = name + ":" + result;
        }
        this.setState({
            mapData: contries
        });
    }

    render() {
        return (
            <div className="row">
                <div className="col-2">
                    <SideBar contry={this.state.contry} parentMapCallback={this.mapCallBack} parentArrowMapCallback={this.arrowMapCallBack} />
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
                        <LineMap patterns={arrow} positions={this.state.arrowMap} />
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
