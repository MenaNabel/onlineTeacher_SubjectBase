import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { Link } from "react-router-dom";

// component
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";
import axios from "axios";
import GetUserToken from "../component/getToken";
import { baseUrl } from "../assets/baseUrl";
export default class Levels extends Component {
  state = {
    levels: [],
    levelName: "",
    loading: true,
  };

  componentDidMount() {
    // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
    const URL_PASE = "https:/localhost:44334/api/";
    const levelId = this.props.match.params.id;
    const token = GetUserToken();
    axios
      .get(`${baseUrl}api/Levels`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        this.setState({
          levels: res.data.items,
          levelName: res.data.levelName,
          loading: false,
        });
      })
      .catch((er) => {
        console.log(er);
        this.setState({
          loading: false,
        });
      });
  }
  render() {
    const { levelName, levels, loading } = this.state;
    return (
      <div>
        <Breadcrumb head="المراحل" />
        <div className="levels-section">
          <Container>
            <Grid container spacing={3}>
              {this.state.levels.map((lev, index) => (
                <Grid item xs={12} sm={6} md={4} lg={4} key={index}>
                  <Link to={"/level-details/" + lev.id} className="level-card">
                    <h5 className="card-title">{lev.levelName}</h5>
                  </Link>
                </Grid>
              ))}
            </Grid>
          </Container>
          {loading ? (
            <div className="loading">
              <button class="btn">
                <span class="spinner"></span>
                <span class="btn__text">برجاء الانتظار...</span>
              </button>
            </div>
          ) : null}
        </div>
        {loading ? null : <Footer />}
      </div>
    );
  }
}
