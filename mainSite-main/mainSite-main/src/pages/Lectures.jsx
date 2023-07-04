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
import { baseUrl } from "../assets/baseUrl";

class Lectures extends Component {
  state = {
    lectures: [],
  };

  componentDidMount() {
    const token = GetUserToken();
    const subjectId = this.props.match.params.subid;
    const montId = this.props.match.params.motid;
    axios
      .get(
        // `http://hossam1234-001-site1.ftempurl.com/Student/lectures/${subjectId}?Month=${montId}`,
        `${baseUrl}Student/lectures/${subjectId}?Month=${montId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      )
      .then((res) => {
        this.setState({
          lectures: res.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  render() {
    const { lectures } = this.state;
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
          <h5>المحاضرات</h5>{" "}
        </div>
        <div className="month-page lectures-page">
          <Container>
            <h5 className="list-header">اختر المحاضره</h5>
            <Grid container spacing={3}>
              {lectures.map((le, index) => (
                <>
                  {le.isAppear ? (
                    <Grid item xs={12} sm={6} md={4} lg={4} key={index}>
                      <Link
                        to={`/lecture-details/${le.id}`}
                        className="month-card" key={index}
                      >
                        {" "}
                        {le.name}{" "}
                      </Link>
                    </Grid>
                  ) : null}
                </>
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

export default Lectures;
