import React, { Component } from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

// import 'mdb-react-ui-kit/dist/css/mdb.min.css'
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Redirect } from "react-router-dom";
import axios from "axios";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import PropTypes from "prop-types";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import jwt_decode from "jwt-decode";

// component
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";
import GetUserToken from "../component/getToken";
import StudenMarksChart from "../component/StudenMarksChart";
import Login from "../pages/Login";
import Loading from "../component/Loading";
import { baseUrl } from "../assets/baseUrl";

const token = GetUserToken();
export default class AddAreview extends Component {
    state = {
        description: "",
        loading: false,
        reviews: [],
        index: 0,
        size: 10,
    };

    componentDidMount() {
        axios.get(`${baseUrl}api/Reviews?&` + `index=${this.state.index}&size=${this.state.size}`).then((res) => {
            if (res.status == 200) {
                this.setState({
                    reviews: res.data.items,
                });
            }
        });
    }

    render() {
        const submitReview = (e) => {
            e.preventDefault();
            this.setState({
                loading: true,
            });
            const state = { ...this.state };
            delete state.loading;
            delete state.reviews;
            if (state.description == "") {
                toast.error("لا يمكن ترك الحقل فارغ");
                this.setState({
                    loading: false,
                });
            }
            axios
                .post(`${baseUrl}api/Reviews`, state, {
                    headers: { Authorization: `Bearer ${token}` },
                })
                .then((res) => {
                    if (res.status == 200) {
                        toast.success("تم اضافه التعليق وسوف يتم مراجعته");
                        this.setState({
                            loading: false,
                        });
                    }
                })
                .catch((error) => {
                    toast.error("قم بتسجيل الدخول لاضافة تعليق");
                    this.setState({ loading: false });
                });
        };

        const handelChange = (e) => {
            this.setState({
                [e.target.name]: e.target.value,
            });
        };

        const { loading, reviews } = this.state;

        return (
            <div className="add-review-page">
                <div>
                    <div>
                        <Breadcrumb head="التعليقات" />

                        <div className="what-say">
                            <div className="what-say-af">
                                <img src="./images/mog.png" alt="" />
                            </div>
                            <div className="what-say-af-2">
                                <img src="./images/mog.png" alt="" />
                            </div>
                            <Container>
                                <div className="section-header">
                                    <h6>ماذا</h6>
                                    <h5>قالو عنا</h5>
                                </div>
                                <Grid container spacing={5}>
                                    {reviews.map((rev, index) => (
                                        <Grid key={index} item xs={12} sm={6} md={6} lg={4}>
                                            <div className="teste-card">
                                                <div
                                                    className="card-img"
                                                    style={{
                                                        backgroundImage: `url('./images/avatar.png')`,
                                                    }}
                                                ></div>
                                                <h5 className="card-name">{rev.studentName}</h5>
                                                {/* <span className="card-date">12-10-2020</span> */}
                                                <p className="card-text">{rev.description}</p>
                                            </div>
                                        </Grid>
                                    ))}
                                </Grid>
                            </Container>
                        </div>

                        <div className="add-review">
                            <Container>
                                <Grid container spacing={5}>
                                    <Grid item xs={12} sm={12} md={12} lg={9}>
                                        <h5>اضف تعليق</h5>
                                        <form action="" className="formStyle" onSubmit={submitReview}>
                                            <div className="form-group">
                                                <textarea
                                                    name="description"
                                                    onChange={handelChange}
                                                    id=""
                                                    placeholder="اضف تعليقك هنا"
                                                ></textarea>
                                            </div>
                                            <button type="submit">اضافه</button>
                                        </form>
                                        {loading ? (
                                            <div className="loginloading">
                                                <Loading />
                                            </div>
                                        ) : null}
                                    </Grid>
                                </Grid>
                            </Container>
                        </div>
                    </div>
                </div>
                <Footer />
            </div>
        );
    }
}
