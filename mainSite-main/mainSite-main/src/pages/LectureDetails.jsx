import React, { Component } from "react";
import GetUserToken from "../component/getToken";
import axios from "axios";
import Breadcrumb from "../component/Breadcrumb";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Redirect } from "react-router-dom";
import swal from "sweetalert2";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { Link } from "react-router-dom";
import Footer from "../component/Footer";
import TelegramLink from "../component/TelegramLink";
import { baseUrl } from "../assets/baseUrl";

export default class LectureDetails extends Component {
    state = {
        lectureDetails: {},
        subjectDetails: {},
        loading: false,
        loading1: false,
        lectureMessage: "",
        fileeData: null,
    };

    async componentDidMount() {
        const lectureId = this.props.match.params.id;
        const token = GetUserToken();
        this.setState({ loading: true, loading1: true });
        await axios
            .get(`${baseUrl}api/StudyLectures/Files/` + lectureId, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((res) => {
                this.setState({ fileeData: res, loading1: false });
            })
            .catch((errors) => {
                console.log(errors);
            });
        await axios
            .get(
                // "http://hossam1234-001-site1.ftempurl.com/api/Lectures/" + lectureId,
                `${baseUrl}api/Lectures/` + lectureId,
                {
                    headers: { Authorization: `Bearer ${token}` },
                }
            )
            .then((res) => {
                this.setState({
                    loading: false,
                    lectureDetails: res.data,
                    subjectDetails: res.data.subject,
                    lectureMessage: res.data.description,
                });
            })
            .catch((errors) => {
                if (errors.response && errors.response.status && errors.response.status == "401") {
                    toast.error("تاكد من تسجيل الدخول");
                }
                if (errors.response && errors.response.data === "Watching limit exceeded" || errors.response.data.title == "Not Found") {
                    swal.fire({
                        title: "لا يمكنك مشاهدة المحاضرة مرة اخرى!",
                        icon: "error",
                        confirmButtonColor: "#1fb354",
                        cancelButtonColor: "#555",
                        confirmButtonText: "طلب فتح المحاضرة",
                        showCancelButton: false,
                        showConfirmButton: true
                    }).then((result) => {
                        let reOpenLecture = { LectureID: lectureId, Reason: "-" };
                        if (result.isConfirmed) {
                            axios
                                .post(`${baseUrl}api/StudyLectures/Request_Open_Watching`, reOpenLecture
                                , {
                                    headers: { Authorization: `Bearer ${token}` },
                                })
                                .then((res) => {
                                    swal.fire("تم ارسال طلبك", "", "success");
                                });
                        }
                    });
                }
                console.log(errors.response);
            });
    }

    render() {
        const { lectureDetails, subjectDetails, lectureMessage, fileeData, loading, loading1 } = this.state;

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
                    <h5>تفاصيل المحاضره</h5>{" "}
                </div>
                <div className="lectures-details subject-details-page">
                    {loading && loading1 ? (
                        <div className="loading">
                            <button class="btn">
                                <span class="spinner"></span>
                                <span class="btn__text">برجاء الانتظار...</span>
                            </button>
                        </div>
                    ) : null}
                    <Container>
                        {lectureMessage == "" ? (
                            <h5 className="list-header">{lectureMessage}</h5>
                        ) : (
                            <>
                                <h5 className="list-header">{lectureDetails.name}</h5>
                                <Grid container spacing={3}>
                                    <Grid item xs={12} sm={6} md={4} lg={6}>
                                        <h5 className="box-title">فيديو المحاضره</h5>
                                        <div style={{ paddingTop: "56.25%", position: "relative" }}>
                                            <iframe
                                                src={lectureDetails.lectureLink}
                                                style={{
                                                    // border: "0",
                                                    maxWidth: "100%",
                                                    position: "absolute",
                                                    top: "0",
                                                    left: "0",
                                                    height: "100%",
                                                    width: "100%",
                                                }}
                                                allow="accelerometer; fullscreen; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                            ></iframe>
                                        </div>
                                    </Grid>
                                    <Grid item xs={12} sm={6} md={4} lg={6}>
                                        <h5 className="box-title">تفاصيل المحاضره</h5>
                                        <ul>
                                            <li>
                                                <span>الاسم</span>{" "}
                                                <span className="price"> {lectureDetails.name} </span>{" "}
                                            </li>
                                            <li>
                                                <span>الماده التابعه لها</span> <span> {subjectDetails.name} </span>{" "}
                                            </li>
                                            <li>
                                                <span>الشهر التابعه لها</span> <span> {lectureDetails.month} </span>{" "}
                                            </li>
                                            <li>
                                                <span>الحاله</span>{" "}
                                                <span> {lectureDetails.isFree ? "مجانيه" : "مدفوعه"} </span>{" "}
                                            </li>
                                            <li>
                                                <span>ملف المحاضرة</span>{" "}
                                                <span>
                                                    {" "}
                                                    <a download={fileeData.data.name} href={fileeData.data.fileData}>
                                                        {lectureDetails.fileName}
                                                    </a>
                                                </span>{" "}
                                            </li>
                                            <li>
                                                <span>الدخول للامتحان</span>{" "}
                                                <Link to={`/exam/${lectureDetails.examID}`}>
                                                    <i className="fas fa-arrow-left"></i>
                                                </Link>{" "}
                                            </li>
                                        </ul>
                                    </Grid>
                                </Grid>
                            </>
                        )}
                    </Container>
                    <TelegramLink />
                </div>
                <Footer />
            </div>
        );
    }
}
