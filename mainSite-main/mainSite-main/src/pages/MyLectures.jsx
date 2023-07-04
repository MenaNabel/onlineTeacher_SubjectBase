import React, { Component } from "react";
import GetUserToken from "../component/getToken";
import axios from "axios";
import Breadcrumb from "../component/Breadcrumb";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Redirect } from "react-router-dom";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { Link } from "react-router-dom";
import Footer from "../component/Footer";
import TelegramLink from "../component/TelegramLink";
import { baseUrl } from "../assets/baseUrl";

export default class MyLectures extends Component {
  state = {
    studentSbject: [],
  };

  componentDidMount() {
    const token = GetUserToken();
    axios
      // .get("http://hossam1234-001-site1.ftempurl.com/Student/Subjects/me", {
      .get(`${baseUrl}Student/Subjects/me`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        if (res.status == 200) {
          this.setState({
            studentSbject: res.data,
          });
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }
  render() {
    const { studentSbject } = this.state;
    return (
      <div>
        <Breadcrumb head="محاضراتي" />
        <div className="my-lectures mylectures-list">
          <Container>
            <h5 className="list-header">يجب عليك اختيار الماده اولا</h5>
            <Grid container spacing={3}>
              {studentSbject.map((sub, index) => (
                <Grid item xs={12} sm={6} md={4} lg={4} key={index}>
                  <Link to={"/month-of-lecture/" + sub.id} className="sub-card">
                    <div
                      className="card-img"
                      style={{
                        backgroundImage: `url(${sub.imagePath})`,
                      }}
                    ></div>
                    <div className="card-body">
                      <h5 className="card-title">{sub.name}</h5>
                      {/* <h6 className="card-price">
                      {sub.price} <span>جنيه</span>{" "}
                    </h6> */}
                    </div>
                  </Link>
                </Grid>
              ))}
            </Grid>
          </Container>

          <TelegramLink />
        </div>
        <Footer />
      </div>
    );
  }
}
