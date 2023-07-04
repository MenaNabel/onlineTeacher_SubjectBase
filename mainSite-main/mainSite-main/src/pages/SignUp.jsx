import React, { Component } from "react";
import jwt_decode from "jwt-decode";
import Grid from "@mui/material/Grid";
import ShowIcon from "@mui/icons-material/Visibility";
import ShowOffIcon from "@mui/icons-material/VisibilityOff";
import { GoogleLogin } from "react-google-login";
import Container from "@mui/material/Container";

// component
import FAQ from "../component/FAQ";
import Footer from "../component/Footer";
import Loading from "../component/Loading";

// react slick
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import Slider from "react-slick";
import axios from "axios";
import { toast } from "react-toastify";

// component
import Breadcrumb from "../component/Breadcrumb";
import { baseUrl } from "../assets/baseUrl";

class SignUp extends Component {
  state = {
    name: "",
    phoneNumber: "",
    levelID: 0,
    email: "",
    password: "",
    confirmPassword: "",
    levels: [],
    loading: false,
    showPassword: false,
    showConfirmPassword: false,
    googleId: "",
    name: "",
    errorList: {},
    errorList2: [],
    isArray: false,
  };

  componentDidMount() {
    document.getElementsByClassName("googleButton")[0].children[1].textContent =
      "تسجيل الدخول بأستخدام جوجل";
    axios.get(`${baseUrl}api/Levels`).then((res) => {
      this.setState({
        levels: res.data.items,
      });
    });
  }

  render() {
    const onFailure = (response) => {
      toast.error(response);
    };
    const onSuccess = (response) => {
      this.setState({
        loading: true,
      });
      const state = { ...this.state };
      state.googleId = response.profileObj.googleId;
      state.email = response.profileObj.email;
      state.name = response.profileObj.name;
      delete state.password;
      delete state.token;
      delete state.loading;
      delete state.errorList;
      delete state.errorList2;
      delete state.isArray;

      axios
        .post(`${baseUrl}api/Auth/GoogleLogin`, state)
        .then((res) => {
          const user = jwt_decode(res.data.message);
          const userRole =
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
          const userRoleRes = user[`${userRole}`];

          if (userRoleRes === "Student") {
            this.setState({
              token: res.data.message,
              loading: false,
              // loadintable: false,
            });
            sessionStorage.setItem("token", JSON.stringify(this.state.token));
            toast.success("تم تسجيل الدخول بنجاح");
            this.props.history.push("/profile");
            window.location.reload();
          }
        })
        .catch((error) => {
          this.setState({
            errorList: error.errors,
            loading: false,
          });
        });
    };
    const {
      levels,
      name,
      phoneNumber,
      levelID,
      email,
      password,
      confirmPassword,
      loading,
      errorList,
      errorList2,
      isArray,
    } = this.state;
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
      delete state.levels;
      delete state.googleId;
      delete state.errorList;
      delete state.errorList2;
      delete state.isArray;
      axios
        .post(`${baseUrl}api/Auth/Register`, state)
        .then((res) => {
          if (res.status == 200) {
            toast.success("تم التسجيل بنجاح برجاء الانتظار");
            setTimeout(() => {
              this.props.history.replace("/login");
              this.setState({
                loading: false,
              });
            }, 1500);
          }
        })
        .catch((error) => {
          console.log(error.response);

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
          if (error.response.status === 400) {
            var tifs = error.response.data;
            Object.keys(tifs).map(function (key) {});
          }

          this.setState({
            loading: false,
          });
        });
    };
    return (
      <div>
        <Breadcrumb head="تسجيل حساب جديد" />

        <div className="login-page sign-up-page">
          {" "}
          <Container>
            <div className="login-form sign-up-form">
              <h5 className="login-header">سجل حساب جديد</h5>
              {!isArray && Object.keys(this.state.errorList).length > 0 ? (
                <div>
                  <ul>
                    {Object.keys(errorList).map(function (key, index) {
                      return (
                        <li className="danger-text">{errorList[key][0]}</li>
                      );
                    })}
                  </ul>
                </div>
              ) : null}
              {isArray && errorList2.length > 0 ? (
                <div>
                  <ul>
                    {errorList2.map((error) => {
                      return <li className="danger-text">{error}</li>;
                    })}
                  </ul>
                </div>
              ) : null}
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
                        <option selected="true" disabled="disabled">
                          اختر المرحلة
                        </option>
                        {levels.map((lev, index) => (
                          <option value={lev.id} key={index}>
                            {lev.levelName}
                          </option>
                        ))}
                      </select>
                    </div>
                  </Grid>
                  <Grid item xs={12} lg={12}>
                    <div className="form-group input-group">
                      <div>
                        <input
                          type={this.state.showPassword ? "text" : "password"}
                          name="password"
                          id=""
                          className="form-control"
                          placeholder="كلمة المرور"
                          value={password}
                          onChange={handelChange}
                        />
                      </div>
                      <div>
                        <button
                          className="btn "
                          onClick={(e) => {
                            e.preventDefault();
                            this.setState({
                              showPassword: !this.state.showPassword,
                            });
                          }}
                        >
                          {" "}
                          {this.state.showPassword ? (
                            <ShowOffIcon />
                          ) : (
                            <ShowIcon />
                          )}
                        </button>
                      </div>
                    </div>
                  </Grid>
                  <Grid item xs={12} lg={12}>
                    <div className="form-group input-group">
                      <div>
                        <input
                          type={
                            this.state.showConfirmPassword ? "text" : "password"
                          }
                          name="confirmPassword"
                          id=""
                          className="form-control"
                          placeholder="تاكيد كلمة المرور"
                          value={confirmPassword}
                          onChange={handelChange}
                        />
                      </div>
                      <div>
                        <button
                          className="btn "
                          onClick={(e) => {
                            e.preventDefault();
                            this.setState({
                              showConfirmPassword:
                                !this.state.showConfirmPassword,
                            });
                          }}
                        >
                          {" "}
                          {this.state.showConfirmPassword ? (
                            <ShowOffIcon />
                          ) : (
                            <ShowIcon />
                          )}
                        </button>
                      </div>
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
                    <GoogleLogin
                      className="googleButton"
                      clientId="370218926467-27bb9dmk2l6l5scn5p81oq6kk1bcdajt.apps.googleusercontent.com"
                      onSuccess={onSuccess}
                      onFailure={onFailure}
                    />
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

export default SignUp;
