import React, { Component } from "react";
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import CircularProgress from "@mui/material/CircularProgress";
import { Link } from "react-router-dom";

// component
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";
import axios from "axios";
import GetUserToken from "../component/getToken";
import { toast } from "react-toastify";
import jwt_decode from "jwt-decode";
import { baseUrl } from "../assets/baseUrl";

export default class SubjectDetails extends Component {
  state = {
    subscribtions: null,
    subjectdetails: {},
    levelDetails: {},
    loading: true,
    studentID: 0,
    subjectID: 0,
    studentlevelDetails: {},
    subscribedisabled: false,
    isSubscribed: false,
    show: false,
    dataRetrieved: false,
  };

  async componentDidMount() {
    const subjectId = this.props.match.params.id;
    // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
    const URL_PASE = "https://localhost:44334/api/";
    const token = GetUserToken();
    if (token != null) {
      await axios
        // .get("http://hossam1234-001-site1.ftempurl.com/api/Subscribtion/my", {
        .get(`${baseUrl}api/Subscribtion/my`, {
          headers: { Authorization: `Bearer ${token}` },
        })
        .then((res) => {
          if (res.status == 200) {
            this.setState({
              subscribtions: res.data,
            });
          }
        });
    }

    await axios
      .get(`${baseUrl}api/Subjects/${subjectId}`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        if (res.status == 200) {
          this.setState({
            subjectdetails: res.data,
            levelDetails: res.data.level,
            loading: false,
          });
        }
      })
      .catch((error) => {
        console.log(error);
      });

    // const token = GetUserToken();
    if (token != null) {
      await axios
        .get(`${baseUrl}api/Students/me`, {
          headers: { Authorization: `Bearer ${token}` },
        })
        .then((res) => {
          this.setState({
            studentlevelDetails: res.data.level,
          });
        });
    }

    if (token != null) {
      for (let index = 0; index < this.state.subscribtions.length; index++) {
        const element = this.state.subscribtions[index];
        if (element.subjectID === this.state.subjectdetails.id) {
          this.setState({ isSubscribed: true });
          break;
        }
      }

      this.setState({ show: true });
    }
    if (this.state.studentlevelDetails.id !== this.state.levelDetails.id) {
      this.setState({
        subscribedisabled: true,
      });
    }
    this.setState({ dataRetrieved: true });
  }

  render() {
    const {
      subjectdetails,
      levelDetails,
      loading,
      subscribedisabled,
      isSubscribed,
      show,
    } = this.state;

    const subjectSubscripe = (e) => {
      e.preventDefault();
      this.setState({
        loading: true,
      });
      const token = GetUserToken();
      let subjectID = this.props.match.params.id;
      if (token === null) {
        toast.error("يجب عليك تسجيل الدخول اولا");
        return;
      }
      const user = jwt_decode(token);
      const userRole = "StudentID";
      const userRoleRes = user[`${userRole}`];
      // this.setState({
      //   studentID: userRoleRes,
      //   subjectID: subjectID
      // })
      const state = { ...this.state };
      state.studentID = parseInt(userRoleRes);
      state.subjectID = parseInt(subjectID);
      delete state.subjectdetails;
      delete state.levelDetails;
      delete state.loading;
      delete state.studentlevelDetails;
      delete state.subscribedisabled;
      delete state.dataRetrieved;
      // const studentID = parseInt(userRoleRes);
      // subjectID = parseInt(subjectID);
      // console.log(user);
      // console.log(userRoleRes, subjectID)
      axios
        .post(`${baseUrl}api/Subscribtion`, state, {
          headers: { Authorization: `Bearer ${token}` },
        })
        .then((res) => {
          if (res.status == 200) {
            this.setState({
              loading: false,
            });
            toast.success("تم الاشتراك في الماده بنجاح");
            this.props.history.replace("/levels");
          }
        })
        .catch((error) => {
          if (error.response.status === 400) {
            var tifs = error.response.data.errors;
            if (tifs != null) {
              Object.keys(tifs).map(function (key) {
                toast.error(`${tifs[key]}`);
              });
            } else {
              toast.error("انت مشترك بالفعل في هذه الماده");
            }
          }
        });
    };

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
          <h5>تفاصيل الكورس</h5>
        </div>
        <div className="subject-details-page">
          <Container>
            {loading ? null : (
              <>
                <h5 className="subject-name">{subjectdetails.name}</h5>
                <Grid container spacing={3}>
                  <Grid item xs={12} sm={6} md={4} lg={6}>
                    <div className="card-img">
                      <img src={subjectdetails.imagePath && subjectdetails.imagePath.includes("base64") ? `${subjectdetails.imagePath}` :`${baseUrl + subjectdetails.imagePath}`} alt="" />
                    </div>
                  </Grid>
                  <Grid item xs={12} sm={6} md={4} lg={6}>
                    <ul>
                      <li>
                        <span>السعر</span>{" "}
                        <span className="price"> {subjectdetails.price} </span>{" "}
                      </li>
                      <li>
                        <span>المستوى</span>{" "}
                        <span> {levelDetails.levelName} </span>{" "}
                      </li>
                      {/* <li><span>رابط المستوى</span> <span> <a href={levelDetails.telegeramLink} target="_blank"> <i className='fab fa-telegram'></i> </a> </span> </li> */}
                    </ul>
                    {this.state.dataRetrieved ? (
                      <div>
                        {subscribedisabled ? (
                          "انت لست في نفس المرحله"
                        ) : (
                          <div>
                            {isSubscribed === true ? (
                              <span disabled>
                                أنت مشترك بالفعل فى هذه المادة
                              </span>
                            ) : (
                              <div>
                                {show ? (
                                  <button
                                    className="btn-subscripe"
                                    disabled={subscribedisabled ? true : false}
                                    onClick={subjectSubscripe}
                                  >
                                    اشتراك
                                  </button>
                                ) : (
                                  <></>
                                )}
                              </div>
                            )}
                          </div>
                        )}
                      </div>
                    ) : (
                      <CircularProgress />
                    )}
                  </Grid>
                </Grid>
              </>
            )}
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
