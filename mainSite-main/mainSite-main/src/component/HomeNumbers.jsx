import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";

// component
import FAQ from "../component/FAQ";
import Footer from "../component/Footer";

import axios from "axios";
import CircularProgress from "@mui/material/CircularProgress";
import { baseUrl } from "../assets/baseUrl";

export default class HomeNumbers extends Component {
  state = {
    homeNymber: {},
    loading: true,
  };

  componentDidMount() {
    // axios.get(`http://hossam1234-001-site1.ftempurl.com/api/Home`).then((res) => {
    axios
      .get(`${baseUrl}api/Home`)
      .then((res) => {
        if (res.status == 200) {
          this.setState({
            homeNymber: res.data,
            loading: false,
          });
        }
      })
      .catch((error) => {
        this.setState({
          loading: false,
        });
      });
  }

  render() {
    const { homeNymber, loading } = this.state;
    return (
      <div>
        <div
          className="number-parent"
          // style={{ backgroundImage: `url('./images/header.png')` }}
        >
          <Grid container>
            <Grid item xs={12} md={4} lg={3}>
              <div className="number-card">
                <div className="card-img">
                  <img src="./images/document.svg" alt="" />
                </div>
                {loading ? (
                  <CircularProgress />
                ) : (
                  <h5 className="card-title"> {homeNymber.filesCount} </h5>
                )}
                <p className="card-text">عدد الملفات</p>
              </div>
            </Grid>
            <Grid item xs={12} md={4} lg={3}>
              <div className="number-card">
                <div className="card-img">
                  <img src="./images/lectures.svg" alt="" />
                </div>
                {loading ? (
                  <CircularProgress />
                ) : (
                  <h5 className="card-title"> {homeNymber.lecturesCount} </h5>
                )}
                <p className="card-text">عدد المحاضرات</p>
              </div>
            </Grid>
            <Grid item xs={12} md={4} lg={3}>
              <div className="number-card">
                <div className="card-img">
                  <img src="./images/students.svg" alt="" />
                </div>
                {loading ? (
                  <CircularProgress />
                ) : (
                  <h5 className="card-title"> {homeNymber.studentsCount} </h5>
                )}
                <p className="card-text">عدد الطلبة</p>
              </div>
            </Grid>
            <Grid item xs={12} md={4} lg={3}>
              <div className="number-card">
                <div className="card-img">
                  <img src="./images/exam.svg" alt="" />
                </div>
                {loading ? (
                  <CircularProgress />
                ) : (
                  <h5 className="card-title"> {homeNymber.examsCount} </h5>
                )}
                <p className="card-text">عدد الامتحانات</p>
              </div>
            </Grid>
          </Grid>
        </div>
      </div>
    );
  }
}
