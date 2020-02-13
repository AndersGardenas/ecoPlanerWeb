import React from 'react';
import axios from 'axios';
const host = 'api/Map/';

export default class HelloMessage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { contryPop: 0 };
        this.state = { money: 0 };
        this.state = { countrySate: null };
        this.state = { map: "" };
        this.state = { arrowMap: "" };
        this.requestGetContry = this.requestGetContry.bind(this);
        this.requestGetAllContries = this.requestGetAllContries.bind(this);
        this.renderMap = this.renderMap.bind(this);
        this.popMapButton = this.popMapButton.bind(this);
        this.gdpMapButton = this.gdpMapButton.bind(this);
        this.requestGetTradingPartner = this.requestGetTradingPartner.bind(this);
    }


    componentDidMount() {
        this.interval = setInterval(() => this.requestGetContry(), 500);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.contry !== this.props.contry) {
            this.requestGetContry();
            this.requestGetTradingPartner();
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
        const url = host + `GetContry?name=${this.props.contry}`;
        axios(url).then(
            response => {
                if (tmpContry === this.props.contry) {

                    this.setState({
                        countrySate: response.data
                    });
                }
            }
        );
    }

    async requestGetTradingPartner() {
        if (this.props.contry === null || this.props.contry === undefined) {
            return;
        }
        var tmpContry = this.props.contry;
        const url = host + `GetTradingPartner?name=${this.props.contry}`;
        axios(url).then(
            response => {
                console.log("data: " + response.data)
                if (response.data === "") {
                    return;
                }
                if (tmpContry === this.props.contry) {

                    var home = response.data['home'];
                    console.log("home " + home)

                    var arrowList = [];
                    for (var prop in response.data) {
                        if (prop !== 'home') {
                            var data = response.data[prop];
                            arrowList.push([home.split('|')[1], home.split('|')[0]], [data.split('|')[1], data.split('|')[0]])
                        }
                    }
                    this.setState({
                        arrowMap: arrowList
                    });
                    this.props.parentArrowMapCallback(arrowList);
                }
            }
        );
    }

    async requestGetAllContries() {
        const url = host + `GetAllContry`;
        axios(url).then(
            response => {
                this.props.parentMapCallback(response.data);
            }
        );
    }

    async requestGetAllContriesGDP() {
        const url = host + `GetGDPContry`;
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
                <div><pre>{JSON.stringify(this.state.countrySate, null, 2)}</pre></div>;
                <p>Arrow map is: {this.state.arrowMap.toString()}</p>
            </div>
        );
    }
}


function pareJson(data) {

    this.setState({
        contryPop: data['contryPop'],
        money: data['money'],
        price: data.split("|")[2], price2: data.split("|")[3]
    });
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

