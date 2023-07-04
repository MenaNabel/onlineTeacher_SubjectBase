import React, { Component } from "react";
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";

// component
import FAQ from "../component/FAQ";
import Footer from "../component/Footer";

// react slick
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import Slider from "react-slick";
import axios from "axios";
import { toast } from "react-toastify";
import { baseUrl } from "../assets/baseUrl";

export default class SignUpForm extends Component {
  state = {
    name: "",
    phoneNumber: "",
    levelID: 0,
    email: "",
    password: "",
    confirmPassword: "",
    levels: [],
  };

  componentDidMount() {
    const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
    axios.get(`${baseUrl}api/Levels`).then((res) => {
      this.setState({
        levels: res.data,
      });
    });
  }

  render() {
    const {
      levels,
      name,
      phoneNumber,
      levelID,
      email,
      password,
      confirmPassword,
    } = this.state;
    const handelChange = (e) => {
      this.setState({
        [e.target.name]: e.target.value,
      });
    };
    const submitForm = (e) => {
      e.preventDefault();
      const state = { ...this.state };
      const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
      delete state.levels;

      axios
        .post(`${baseUrl}api/Auth/Register`, state)
        .then((res) => {
          if (res.status == 200) {
            toast.success("تم التسجيل بنجاح برجاء الانتظار");
          }
        })
        .catch((error) => {
          if (error.response.status === 400) {
            var tifs = error.response.data.errors;
            Object.keys(tifs).map(function (key) {
              toast.error(`${tifs[key]}`);
            });
          }
        });
    };
    return (
      <div className="header-form">
        <h5>قم بانشاء حساب جديد</h5>
        <p>واستمتع بجميع المزايا</p>
        <form action="" onSubmit={submitForm}>
          <Grid container>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <input
                  type="text"
                  name="name"
                  id=""
                  className="form-control"
                  placeholder="الاسم"
                  value={name}
                  onChange={handelChange}
                />
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <input
                  type="email"
                  name="email"
                  id=""
                  className="form-control"
                  placeholder="البريد الالكتروني"
                  value={email}
                  onChange={handelChange}
                />
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <input
                  type="number"
                  name="phoneNumber"
                  id=""
                  className="form-control"
                  placeholder="رقم الهاتف"
                  value={phoneNumber}
                  onChange={handelChange}
                />
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <select
                  name="levelID"
                  onChange={handelChange}
                  className="form-control"
                  placeholder="المرحله"
                  id=""
                >
                  {levels.map((lev, index) => (
                    <option value={lev.id} key={index}>
                      {lev.levelName}
                    </option>
                  ))}
                </select>
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <input
                  type="password"
                  name="password"
                  id=""
                  className="form-control"
                  placeholder="كلمة المرور"
                  value={password}
                  onChange={handelChange}
                />
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="form-group">
                <input
                  type="password"
                  name="confirmPassword"
                  id=""
                  className="form-control"
                  placeholder="تاكيد كلمة المرور"
                  value={confirmPassword}
                  onChange={handelChange}
                />
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <button className="btn">التسجيل</button>
            </Grid>
            <Grid item xs={12} lg={12}>
              <div className="or">
                <span></span>
                <h5>او</h5>
                <span></span>
              </div>
            </Grid>
            <Grid item xs={12} lg={12}>
              <button className="google-login">
                <span>اكمل باستخدام جوجل</span>
                <svg
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
                </svg>
              </button>
            </Grid>
          </Grid>
        </form>
      </div>
    );
  }
}
