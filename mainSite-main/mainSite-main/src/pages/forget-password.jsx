import React, { Component } from "react";

import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

// material ui
import Grid from "@mui/material/Grid";
import ShowIcon from "@mui/icons-material/Visibility";
import ShowOffIcon from "@mui/icons-material/VisibilityOff";
import Container from "@mui/material/Container";
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";
import axios from "axios";
import { toast } from "react-toastify";
import jwt_decode from "jwt-decode";
import Loading from "../component/Loading";
import { baseUrl } from "../assets/baseUrl";
import { GoogleLogin } from "react-google-login";

class ForgetPassword extends Component {
  state = {
    email: "",

    token: "",
    loading: false,
  };

  componentDidMount() {}

  render() {
    const handelChange = (e) => {
      this.setState({
        [e.target.name]: e.target.value,
      });
    };

    const submitForm = (e) => {
      e.preventDefault();
      this.setState({
        loading: true,
      });
      const state = { ...this.state };
      // const PASE_URL = "http://hossam1234-001-site1.ftempurl.com/api/";
      const PASE_URL = "https://localhost:44334/api/";
      delete state.token;

      delete state.loading;
      var data = { email: this.state.email };
      axios
        .post(`${baseUrl}api/Auth/ForgetPassword`, data)
        .then((res) => {
          this.setState({ loading: false });
        })
        .catch((error) => {
          if (error.response.data.errors != null) {
            if (Array.isArray(error.response.data.errors)) {
              this.setState({ isArray: true });
              error.response.data.errors.forEach((item) => {
                if (item.includes("already taken")) {
                  return (item = "هذا البريد الالكترونى مستخدم من قبل");
                }
              });
              this.setState({ errorList2: error.response.data.errors });
            } else {
              this.setState({ isArray: false });

              this.setState({ errorList: error.response.data.errors });
            }
          } else {
            this.setState({
              errorList: {
                message: { 0: "تاكد من البريد الالكترونى وكلمة المرور" },
              },
            });
          }

          this.setState({
            loading: false,
          });

          // if(tifs != null){
          //   var tifs = error.response.data;
          //   console.log(tifs);
          //   Object.keys(tifs).map(function (key) {
          //     console.log(tifs[key]);
          //     toast.error(`${tifs[key]}`);
          //   });
          // }
        });
    };

    const { email, password, loading, errorList, errorList2, isArray } =
      this.state;
    return (
      <div>
        <Breadcrumb head="تسجيل الدخول" />
        <div className="login-page">
          <Container>
            <div className="login-form">
              <h5 className="login-header">تغيير كلمة المرور</h5>

              <form action="" onSubmit={submitForm}>
                <Grid container>
                  <Grid item xs={12} md={12} lg={12}>
                    <div className="form-group">
                      <input
                        type="email"
                        name="email"
                        id=""
                        value={email}
                        onChange={handelChange}
                        className="form-control"
                        placeholder="البريد الالكتروني"
                      />
                    </div>
                  </Grid>

                  <Grid sm={12} lg={12}>
                    <button type="submit" className="btn">
                      تاكيد
                    </button>
                  </Grid>

                  <Grid item xs={12} lg={12}>
                    {/* <button className="google-login"> */}
                    {/* <svg
                        xmlns="http://www.w3.org/2000/svg"
                        href="http://www.w3.org/1999/xlink"
                        viewBox="0 0 48 48"
                        version="1.1"
                        width="18px"
                        height="18px"
                      >
                        <g id="surface1">
                          <path
                            style={{ fill: "#FFC107" }}
                            d="M 43.609375 20.082031 L 42 20.082031 L 42 20 L 24 20 L 24 28 L 35.304688 28 C 33.652344 32.65625 29.222656 36 24 36 C 17.371094 36 12 30.628906 12 24 C 12 17.371094 17.371094 12 24 12 C 27.058594 12 29.84375 13.152344 31.960938 15.039063 L 37.617188 9.382813 C 34.046875 6.054688 29.269531 4 24 4 C 12.953125 4 4 12.953125 4 24 C 4 35.046875 12.953125 44 24 44 C 35.046875 44 44 35.046875 44 24 C 44 22.660156 43.863281 21.351563 43.609375 20.082031 Z "
                          />
                          <path
                            style={{ fill: "#FF3D00" }}
                            d="M 6.304688 14.691406 L 12.878906 19.511719 C 14.65625 15.109375 18.960938 12 24 12 C 27.058594 12 29.84375 13.152344 31.960938 15.039063 L 37.617188 9.382813 C 34.046875 6.054688 29.269531 4 24 4 C 16.316406 4 9.65625 8.335938 6.304688 14.691406 Z "
                          />
                          <path
                            style={{ fill: "#4CAF50" }}
                            d="M 24 44 C 29.164063 44 33.859375 42.023438 37.410156 38.808594 L 31.21875 33.570313 C 29.210938 35.089844 26.714844 36 24 36 C 18.796875 36 14.382813 32.683594 12.71875 28.054688 L 6.195313 33.078125 C 9.503906 39.554688 16.226563 44 24 44 Z "
                          />
                          <path
                            style={{ fill: "#1976D2" }}
                            d="M 43.609375 20.082031 L 42 20.082031 L 42 20 L 24 20 L 24 28 L 35.304688 28 C 34.511719 30.238281 33.070313 32.164063 31.214844 33.570313 C 31.21875 33.570313 31.21875 33.570313 31.21875 33.570313 L 37.410156 38.808594 C 36.972656 39.203125 44 34 44 24 C 44 22.660156 43.863281 21.351563 43.609375 20.082031 Z "
                          />
                        </g>
                      </svg> */}
                    {/* </button>{" "} */}
                  </Grid>
                </Grid>
              </form>
              {loading ? (
                <div className="loginloading">
                  <Loading />
                </div>
              ) : null}
            </div>
          </Container>
        </div>
        <Footer />
      </div>
    );
  }
}

export default ForgetPassword;
