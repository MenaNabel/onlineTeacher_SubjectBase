import React, { Component } from "react";
import GetUserToken from "./getToken";
import axios from "axios";
import { baseUrl } from "../assets/baseUrl";

export default class TelegramLink extends Component {
  state = {
    level: {},
  };

  componentDidMount() {
    const token = GetUserToken();
    if (token != null) {
      axios
        .get(`${baseUrl}api/Students/me`, {
          headers: { Authorization: `Bearer ${token}` },
        })
        .then((res) => {
          this.setState({
            level: res.data.level,
          });
        });
    }
  }

  render() {
    const { level } = this.state;
    const token = GetUserToken();
    if (!token) {
      return null;
    }

    return (
      <div className="telegramLink">
        <a href={level.telegeramLink} target="_blank">
          <i className="fab fa-telegram"></i>
        </a>
      </div>
    );
  }
}
