import { Container } from "@mui/material";
import React, { Component } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import MenueDropDown from "./MenueDropDown";
import GetUserToken from "./getToken";
import { baseUrl } from "../assets/baseUrl";

// material ui

export default class Navbar extends Component {
  state = {
    levels: [],
    tokenexist: false,
  };

  componentDidMount() {
    const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
    axios.get(`${baseUrl}api/Levels`).then((res) => {
      this.setState({
        levels: res.data,
      });
    });
    const newtokenexist = GetUserToken();
    if (newtokenexist != null) {
      this.setState({
        tokenexist: true,
      });
    }
  }

  render() {
    const { openMobSideMenu } = this.props;
    const { tokenexist } = this.state;

    const { levels } = this.state;
    return (
      <div>
        <div className="mynavbr">
          <Container>
            <div className="mynavbar-parent">
              <div className="logo">
                <img src="./images/logo.svg" alt="" />
              </div>
              <ul className="links">
                <li className="navItem">
                  <Link to="/" className="navLink">
                    الرئيسية
                  </Link>
                </li>
                <li className="navItem">
                  <Link to="/free" className="navLink">
                    المحاضرات المجانية
                  </Link>
                </li>
                <li className="navItem">
                  <Link to="/levels" className="navLink">
                    المراحل
                  </Link>
                </li>
                {/* <li className="navItem">
                  <Link to="/mylectures" className="navLink">
                    محاضراتي
                  </Link>
                </li> */}
                {/* <li className="navItem">
                  <a className="navLink">
                    المحاضرات المباشره
                  </a>
                </li> */}
                <li className="navItem">
                  <Link to="/who-us" className="navLink">
                    من نحن
                  </Link>
                </li>
              </ul>
              <div className="login">
                {tokenexist ? (
                  <MenueDropDown />
                ) : (
                  <>
                    <Link to="/sign-up" className="navLink">
                      تسجيل حساب جديد
                    </Link>
                    <Link to="/login" className="navLink">
                      تسجيل الدخول
                    </Link>
                  </>
                )}
              </div>
              <div className="sidemenue-icon" onClick={openMobSideMenu}>
                <i className="fas fa-bars"></i>
              </div>
            </div>
          </Container>
        </div>
      </div>
    );
  }
}
