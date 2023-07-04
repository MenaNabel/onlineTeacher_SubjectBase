import React, { Component } from "react";
import axios from "axios";
// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";

// component
import FAQ from "../component/FAQ";
import Footer from "../component/Footer";
import { baseUrl } from "../assets/baseUrl";
export default class Honors extends Component {
  state = {
    honors: [],
    levelInfo: {},
  };

  componentDidMount() {
    axios.get(`${baseUrl}api/home/Honers`).then((res) => {
      if (res.status == 200) {
        this.setState({
          honors: res.data,
          // levelInfo: res.data.level
        });
      }
    });
  }
  render() {
    const { honors, levelInfo } = this.state;
    return (
      <div>
        <Container>
          <Grid container spacing={4}>
            {honors.map((hon, index) => (
              <Grid item xs={12} md={4} lg={4}>
                <div className="honor-card">
                  {hon.studentPicture != null ? (
                    <div
                      className="card-img"
                      style={{ backgroundImage: `url(${hon.studentPicture})` }}
                    >
                      {" "}
                    </div>
                  ) : (
                    <div
                      className="card-img"
                      style={{
                        backgroundImage: `url('${process.env.PUBLIC_URL}images/avatar.png')`,
                      }}
                    >
                      {" "}
                    </div>
                  )}
                  <h5 className="card-name"> {hon.studentName} </h5>
                  <p className="card-level"> {hon.level.levelName} </p>
                  <ul>
                    <li>
                      <span>الدرجه</span>
                    </li>
                    <li>
                      <span className="card-degree">
                        {" "}
                        {hon.totalExamsDegree}{" "}
                      </span>
                    </li>
                  </ul>
                </div>
              </Grid>
            ))}
          </Grid>
        </Container>
      </div>
    );
  }
}
