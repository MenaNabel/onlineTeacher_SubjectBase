import React, { Component } from "react";

import { Container } from "@mui/material";
import axios from "axios";
import { Link } from "react-router-dom";
import MenueDropDown from "./MenueDropDown";
import GetUserToken from "./getToken";
import { baseUrl } from "../assets/baseUrl";

export default class Sidebar extends Component {
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
        const { closeSideMenue } = this.props;
        const { tokenexist } = this.state;

        return (
            <div>
                <div>
                    <div className="Mysidebar">
                        <button className="closeSideMenue" onClick={closeSideMenue}>
                            <i className="fas fa-times"></i>
                        </button>
                        <div className="logo">
                            <a href="/">
                                <img src="./images/logo.svg" alt="" />
                            </a>
                        </div>
                        <ul className="list-sidebar">
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
                            <li className="navItem">
                                <Link to="#" className="navLink">
                                    تواصل معنا
                                </Link>
                            </li>
                        </ul>
                        <div className="login">
                            {tokenexist ? null : (
                                <Link to="/sign-up" className="navLink">
                                    تسجيل حساب جديد
                                </Link>
                            )}

                            {tokenexist ? (
                                <MenueDropDown />
                            ) : (
                                <Link to="/login" className="navLink">
                                    تسجيل الدخول
                                </Link>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
