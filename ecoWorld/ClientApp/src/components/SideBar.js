import React from 'react';
import axios from 'axios';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faUserFriends, faDollarSign, faAppleAlt, faTshirt, } from '@fortawesome/free-solid-svg-icons'


const host = 'api/Map/';

const TRADINGPARTNERS = 'TradePartners'
const GDP = 'GDP'
const POPULATION = 'population'

export default class HelloMessage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            contryPop: 0,
            money: 0,
            countrySate: [],
            map: "",
            arrowMap: ""
        };
        this.requestGetContry = this.requestGetContry.bind(this);
        this.requestGetAllContries = this.requestGetAllContries.bind(this);
        this.requestGetContryTradePartners = this.requestGetContryTradePartners.bind(this);
        this.renderMap = this.renderMap.bind(this);
        this.popMapButton = this.popMapButton.bind(this);
        this.gdpMapButton = this.gdpMapButton.bind(this);
        this.tradePartnersButton = this.tradePartnersButton.bind(this);
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
                    this.props.parentArrowMapCallback([]);
                    return;
                }
                if (tmpContry === this.props.contry) {

                    var home = response.data['home'];
                    console.log("home " + home)

                    var arrowList = [];
                    for (var prop in response.data) {
                        if (prop !== 'home') {
                            var data = response.data[prop];
                            arrowList.push([[home.split('|')[1], home.split('|')[0]], [data.split('|')[1], data.split('|')[0]]])
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
            });
    }

    async requestGetAllContriesGDP() {
        const url = host + `GetGDPContry`;
        axios(url).then(
            response => {
                this.props.parentMapCallback(response.data);
            });
    }

    async requestGetContryTradePartners() {
        const url = host + `GetContryTradePartners`;
        axios(url).then(
            response => {
                this.props.parentMapCallback(response.data);
            });
    }

    async renderMap(mapType) {
        if (this.state.map === mapType) {
            this.setState({ map: "" });
            this.props.parentMapCallback(null);
            return;
        }
        this.setState({ map: mapType });

        if (mapType === POPULATION) {
            this.requestGetAllContries();
        } else if (mapType === GDP) {
            this.requestGetAllContriesGDP();
        } else if (mapType === TRADINGPARTNERS) {
            this.requestGetContryTradePartners();
        }
    }

    async popMapButton() {
        this.renderMap(POPULATION);
    }

    async gdpMapButton() {
        this.renderMap(GDP);
    }

    async tradePartnersButton() {
        this.renderMap(TRADINGPARTNERS);
    }

    render() {
        return (
            <div>
                <p>
                    <FontAwesomeIcon icon={faDollarSign} />
                    {nFormatter(this.state.countrySate.money, 3)}
                    &nbsp;
                    <FontAwesomeIcon icon={faUserFriends} />
                    {nFormatter(this.state.countrySate.populations, 3)}
                    <br />
                    <FontAwesomeIcon icon={faAppleAlt} />
                    {nFormatter(this.state.countrySate.Fruit, 3)}
                    &nbsp;
                    <FontAwesomeIcon icon={faTshirt} />
                    {nFormatter(this.state.countrySate.Cloth, 3)}
                    <br />
                    <FontAwesomeIcon icon={faDollarSign} />
                    <FontAwesomeIcon icon={faAppleAlt} />
                    {nFormatter(this.state.countrySate.Cost_of_Fruit, 3)}
                    &nbsp;
                    <FontAwesomeIcon icon={faDollarSign} />
                    <FontAwesomeIcon icon={faTshirt} />
                    {nFormatter(this.state.countrySate.Cost_of_Cloth, 3)}
                </p>

                <button onClick={this.popMapButton} type="button">PopMap</button><p />
                <button onClick={this.gdpMapButton} type="button">GdpMap</button><p />
                <button onClick={this.tradePartnersButton} type="button">TradePartnersMap</button>
                <p>Map mode: {this.state.map} </p>

                <div> Contry info: <pre>{JSON.stringify(this.state.countrySate, null, 2)}</pre></div>

                <div> Arrow map is: <pre>{JSON.stringify(this.state.arrowMap, null, 2)}</pre></div>
            </div>
        );
    }
}

//<div> Contry info: <pre>{JSON.stringify(this.state.countrySate, null, 2)}</pre></div>

function nFormatter(input, digits) {
    if (input < 100) {
        return parseFloat(input).toFixed(2);
    }
    var num = parseInt(input);
    num = num || 0;
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
};

