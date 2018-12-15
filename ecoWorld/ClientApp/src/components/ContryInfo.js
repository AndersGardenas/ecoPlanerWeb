import React, { Component } from 'react';


export default class ContryInfo extends React.Component {
    constructor(props) {
        super(props);
        this.getSelectedContry = this.getSelectedContry.bind(this);
    }
    const url = `api/SampleData/WeatherForecasts?startDateIndex=${startDateIndex}`;

    getSelectedContry() {

    }


    render() {

        return (
            <h1>Hello, {this.props.contry}</h1>
        );
    }
}