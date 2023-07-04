import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";

class Login extends Component {
  render() {
    return (
      <div>
        <Breadcrumb head="تسجيل الدخول" />
        <div className="login-page">
          <Container>
            <div className="login-form">
              <h5 className="login-header">سجل الدخول </h5>
              <Grid container>
                <Grid item xs={12} md={12} lg={12}>
                  <div className="form-group">
                    <input type="number" name="" id="" className="form-control" placeholder="رقم الهاتف" />
                  </div>
                </Grid>
                <Grid item xs={12} md={12} lg={12}>
                  <div className="form-group">
                    <input type="password" name="" id="" className="form-control" placeholder="كلمة المرور" />
                  </div>
                </Grid>
                <Grid item xs={12} md={12} lg={12}>
                  <div className="new-account">
                    <a href="">حساب جديد</a>
                  </div>
                </Grid>
                <Grid sm={12} lg={12}>
                  <button type="submit" className="btn">الدخول</button>
                </Grid>
              </Grid>
            </div>
          </Container>
        </div>
        <Footer />
      </div>
    );
  }
}

export default Login;
