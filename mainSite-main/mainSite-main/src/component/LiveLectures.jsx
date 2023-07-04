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
import SignUpForm from "../component/SignUpForm";
import HomeNumbers from "../component/HomeNumbers";
import Honors from "../component/Honors";
import axios from "axios";
import { Link } from "react-router-dom";
import GetUserToken from "./getToken";
import { baseUrl } from "../assets/baseUrl";

export default class LiveLectures extends Component {
  state = {
    onlineLecture: [],
  };

  componentDidMount() {
    const token = GetUserToken();

    axios
      .get(`${baseUrl}api/Student/lectures/2`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        this.setState({
          onlineLecture: res.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  render() {
    const { onlineLecture } = this.state;
    return (
      <div>
        <Container>
          <div className="section-header">
            <h6>المحاضرات</h6>
            <h5>المحاضرات المباشره</h5>
          </div>
          <Grid container spacing={5}>
            {onlineLecture.map((onl, index) => (
              <Grid item xs={12} sm={6} md={6} lg={4}>
                <div className="freeLecture-card">
                  <iframe src={onl.lectureLink} frameborder="0"></iframe>
                  <h5 className="card-title">{onl.description}</h5>
                  {/* <a target="_blank" href={onl.lectureLink}>
                    مشاهدة المحاضره
                  </a> */}
                </div>
              </Grid>
            ))}
          </Grid>
        </Container>
      </div>
    );
  }
}
