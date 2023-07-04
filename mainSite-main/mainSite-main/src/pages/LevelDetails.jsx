import React, { Component } from "react";
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import { Link } from "react-router-dom";

// component
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";
import axios from "axios";
import GetUserToken from "../component/getToken";
import TelegramLink from "../component/TelegramLink";
import { baseUrl } from "../assets/baseUrl";

export default class LevelDetails extends Component {
    state = {
        levelid: "",
        levelsubject: [],
        levelname: "",
        levelsubjectNoSubject: "",
        loading: true,
    };

    componentDidMount() {
        const levelId = this.props.match.params.id;
        // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
        const URL_PASE = "https://localhost:44334/api/";

        const token = GetUserToken();
        axios
            .get(`${baseUrl}api/Subjects/GetAllLectures/${levelId}`, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((res) => {
                if (res.status == 200) {
                    if (res.data.length == 0) {
                        this.setState({
                            levelsubjectNoSubject: "لا توجد مواد",
                            loading: false,
                        });
                    }
                    this.setState({
                        levelsubject: res.data.items,
                        levelname: res.data.items[0].level.levelName,
                        loading: false,
                    });
                }
            })
            .catch((errror) => {
                console.log(errror);
                this.setState({
                    loading: false,
                });
            });

        this.setState({
            levelid: levelId,
        });
    }
    render() {
        const { levelsubject, levelname, loading, levelsubjectNoSubject } = this.state;
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
                    <h5>{levelname}</h5>
                </div>

                <div className="level-details">
                    <Container>
                        <h5 className="lev-head">مواد {levelname}</h5>
                        <Grid container spacing={3}>
                            {levelsubject.length == 0 ? (
                                <> {levelsubjectNoSubject} </>
                            ) : (
                                <>
                                    {levelsubject.map((sub, index) => (
                                        <Grid item xs={12} sm={6} md={4} lg={4} key={index}>
                                            <Link to={"/subject-details/" + sub.id} className="sub-card">
                                                <div
                                                    className="card-img"
                                                    style={{ backgroundImage:  sub.imagePath && sub.imagePath.includes("base64") ? `url(${ sub.imagePath})` :`url(${baseUrl + sub.imagePath})` }}
                                                ></div>
                                                <div className="card-body">
                                                    <h5 className="card-title">{sub.name}</h5>
                                                    <h6 className="card-price">
                                                        {sub.price} <span>جنيه</span>{" "}
                                                    </h6>
                                                </div>
                                            </Link>
                                        </Grid>
                                    ))}
                                </>
                            )}
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

                    <TelegramLink />
                </div>
                {loading ? null : <Footer />}
            </div>
        );
    }
}
