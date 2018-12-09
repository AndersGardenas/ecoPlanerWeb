import React from 'react';
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom';
import registerServiceWorker from './registerServiceWorker';
import { Map, Marker, Popup, TileLayer } from 'react-leaflet'

import App from './components/App.js';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);




//const position = [51.505, -0.09]
//const map = (
//  <Map center={position} zoom={13}>
//    <TileLayer
//      url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
//      attribution="&copy; <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
//    />
//    <Marker position={position}>
//      <Popup>A pretty CSS3 popup.<br />Easily customizable.</Popup>
//    </Marker>
//  </Map>
//);

//render(map, document.getElementById('map'));

//ReactDOM.renderComponent(<App />, document.getElementById('map'));

registerServiceWorker();

