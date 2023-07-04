import React, { Component } from "react";
import GetUserToken from "../component/getToken";
import axios from "axios";
import Breadcrumb from "../component/Breadcrumb";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Redirect } from "react-router-dom";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { Link } from "react-router-dom";
import Footer from "../component/Footer";
import TelegramLink from "../component/TelegramLink";
import LiveLectures from "../component/LiveLectures";
import { baseUrl } from "../assets/baseUrl";

export default class MonthOfLectures extends Component {
  state = {
    monthes: [],
    subid: "",
  };

  componentDidMount() {
    const token = GetUserToken();
    const subjectId = this.props.match.params.id;

    // axios.get("http://hossam1234-001-site1.ftempurl.com/api/Lectures/MonthsForSubject/" + subjectId, {
    axios
      .get(`${baseUrl}api/Lectures/MonthsForSubject/` + subjectId, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        this.setState({
          monthes: res.data.sort(function (a, b) {
            return a - b;
          }),
          subid: subjectId,
        });
      })
      .catch((error) => {
        toast.error("هناك خطا ما");
      });
  }

  render() {
    const { monthes, subid } = this.state;
    return (
      <div>
        <div
          className="breadcrumb"
          style={{
            backgroundImage: `url('${process.env.PUBLIC_URL}/images/breadcrumb.jpg')`,
          }}
          data-aos="fade-up"
          data-aos-duration="2000"
        >
          {" "}
          <h5>الشهور</h5>{" "}
        </div>
        <div className="month-page">
          <Container>
            <h5 className="list-header">
              يجب عليك اختيار الشهر لرؤيه المحاضرات{" "}
            </h5>
            <Grid container spacing={3}>
              {monthes.map((mo, index) => (
                <Grid item xs={12} sm={6} md={3} lg={2} key={index}>
                  <Link to={`/lectures/${subid}/${mo}`} className="month-card">
                    {" "}
                    {mo}{" "}
                  </Link>
                </Grid>
              ))}
            </Grid>
          </Container>

          <div className="online-lectures-page">
            <LiveLectures />
          </div>

          <TelegramLink />
        </div>
        <Footer />
      </div>
    );
  }
}
