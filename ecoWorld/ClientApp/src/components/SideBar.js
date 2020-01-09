import React from 'react';
import axios from 'axios';

export default class HelloMessage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { contryPop: 0 };
        this.state = { money: 0 };
        this.state = { price: 0 };
        this.state = { price2: 0 };
        this.state = { map: "" };
        this.requestGetContry = this.requestGetContry.bind(this);
        this.requestGetAllContries = this.requestGetAllContries.bind(this);
        this.renderMap = this.renderMap.bind(this);
        this.popMapButton = this.popMapButton.bind(this);
        this.gdpMapButton = this.gdpMapButton.bind(this);
    }

    componentDidMount() {
        this.interval = setInterval(() => this.requestGetContry(), 500);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.contry !== this.props.contry) {
            this.requestGetContry();
        }
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }


    async requestGetContry() {
        if (this.props.contry === null || this.props.contry === undefined) {
            return;
        }
        var tmpContry = this.props.contry;
        const url = `api/SampleData/GetContry?name=${this.props.contry}`;
        axios(url).then(
            response => {
                if (tmpContry === this.props.contry) {
                    this.setState({
                        contryPop: response.data.split("|")[0], money: response.data.split("|")[1],
                        price: response.data.split("|")[2], price2: response.data.split("|")[3]
                    });
                }
            }
        );
    }

    async requestGetAllContries() {
        const url = `api/SampleData/GetAllContry`;
        axios(url).then(
            response => {
                this.props.parentMapCallback(response.data);
            }
        );
    }

    async requestGetAllContriesGDP() {
        const url = `api/SampleData/GetGDPContry`;
        axios(url).then(
            response => {
                this.props.parentMapCallback(response.data);
            }
        );
    }

    async renderMap(mapType) {
        if (this.state.map === mapType) {
            this.setState({ map: "" });
            this.props.parentMapCallback(null);
            return;
        }
        this.setState({ map: mapType });

        if (mapType === 'population') {
            this.requestGetAllContries();
        } else if (mapType === 'GDP') {
            this.requestGetAllContriesGDP();
        }
    }

    async popMapButton() {
        this.renderMap("population");
    }

    async gdpMapButton() {
        this.renderMap("GDP");
    }

    render() {
        return (
            <div>
                <button onClick={this.popMapButton} type="button">PopMap</button><p/>
                <button onClick={this.gdpMapButton} type="button">gdpMapButton</button>
                <p>Map mode: {this.state.map} </p>
                <p>Hello: {this.props.contry} </p>
                <p>Populations is: {nFormatter(this.state.contryPop, 2)}</p>
                <p>Money is: {nFormatter(this.state.money, 2)}</p>
                <p>{this.state.price}</p>
                <p>{this.state.price2}</p>
            </div>
        );
    }
}


function nFormatter(num, digits) {
    var si = [
        { value: 1, symbol: "" },
        { value: 1E3, symbol: "k" },
        { value: 1E6, symbol: "M" },
        { value: 1E9, symbol: "G" },
        { value: 1E12, symbol: "T" },
        { value: 1E15, symbol: "P" },
        { value: 1E18, symbol: "E" }
    ];
    var rx = /\.0+$|(\.[0-9]*[1-9])0+$/;
    var i;
    for (i = si.length - 1; i > 0; i--) {
        if (num >= si[i].value) {
            break;
        }
    }
    return (num / si[i].value).toFixed(digits).replace(rx, "$1") + si[i].symbol;
}
