import React, { Component } from "react";

// material ui
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
import LiveLectures from "../component/LiveLectures";
import { baseUrl } from "../assets/baseUrl";

class Home extends Component {
    state = {
        teacherInfo: {},
        reviews: [],
    };

    componentDidMount() {
        axios
            .get(
                /*`http://hossam1234-001-site1.ftempurl.com/api/Setting`*/
                `${baseUrl}api/Setting`
            )
            .then((res) => {
                this.setState({
                    teacherInfo: res.data,
                });
            });

        axios.get(`${baseUrl}api/Reviews`).then((res) => {
            if (res.status == 200) {
                res.data = res.data.items.reverse();
                if (res.data.length >= 6) {
                    this.setState({ reviews: res.data.slice(0, 6) });
                } else {
                    this.setState({
                        reviews: res.data,
                    });
                }
            }
        });
    }

    render() {
        const handelChange = (e) => {};

        var settings = {
            dots: true,
            infinite: false,
            arrows: true,
            speed: 500,
            slidesToShow: 3,
            slidesToScroll: 1,
            initialSlide: 0,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                        infinite: true,
                        dots: true,
                    },
                },
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        initialSlide: 2,
                    },
                },
                {
                    breakpoint: 524,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
            ],
        };
        const { teacherInfo, reviews } = this.state;
        return (
            <div className="home-page">
                <div className="header" style={{ backgroundImage: `url('./images/header.jpg')` }}>
                    <div
                        className="bg-after"
                        // style={{ backgroundImage: `url('./images/header-after.webp')` }}
                    >
                        {/* <img src="./images/contact-after.png" alt="" /> */}
                    </div>
                    <Container>
                        <Grid container>
                            <Grid item xs={12} md={6} lg={6}>
                                <div className="header-content">
                                    <h5>فن التدريس هو فن المساعدة على الاكتشاف</h5>
                                    <p>منصة الأستاذ بيتر سلامة خاصة بتدريس الرياضيات للمرحلة الثانوية</p>
                                </div>
                            </Grid>
                            <Grid item xs={12} md={6} lg={6}>
                                <div className="header-content">
                                    <h5>الرياضيات فن وفكر وإبداع</h5>
                                </div>
                            </Grid>
                            <Grid item xs={12} md={1} lg={1}></Grid>
                            <Grid item xs={12} md={5} lg={4}>
                                {/* <SignUpForm /> */}
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                {/* about */}
                <div className="about">
                    <div className="ab-img-af">
                        <img src="./images/number-img-after.png" alt="" />
                    </div>
                    <Container>
                        <Grid container spacing={4}>
                            <Grid item xs={12} md={6} lg={6}>
                                <div className="about-img">
                                    <img src={teacherInfo.fileName} alt="" style={{ width: "400px" }} />
                                </div>
                            </Grid>
                            <Grid item xs={12} md={6} lg={6}>
                                <div className="about-content">
                                    <h5>{teacherInfo.name}</h5>
                                    <p>{teacherInfo.description}</p>
                                </div>
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                {/* services */}
                <div className="services">
                    <img src="./images/side-img.png" alt="" className="serv-after" />
                    <Container>
                        <div className="section-header">
                            <h6>خدماتنا</h6>
                            <h5>خدماتنا</h5>
                        </div>
                        <Grid container spacing={4}>
                            <Grid item xxs={12} xs={6} sm={4} md={4} lg={3}>
                                <div className="serv-card">
                                    <div className="card-img" style={{ backgroundColor: "#1fb354" }}>
                                        <img src="./images/palette.svg" alt="" />
                                        <label className="serv-name">محاضرات شرح</label>
                                    </div>
                                    {/* <h5 className="card-title">اسم الخدمة</h5> */}
                                </div>
                            </Grid>
                            <Grid item xxs={12} xs={6} sm={4} md={4} lg={3}>
                                <div className="serv-card">
                                    <div className="card-img" style={{ backgroundColor: "#1f3b64" }}>
                                        <img src="./images/briefcase.svg" alt="" />
                                        <label className="serv-name">مراجعات دورية</label>
                                    </div>
                                    {/* <h5 className="card-title">اسم الخدمة</h5> */}
                                </div>
                            </Grid>
                            <Grid item xxs={12} xs={6} sm={4} md={4} lg={3}>
                                <div className="serv-card">
                                    <div className="card-img" style={{ backgroundColor: "#7367f0" }}>
                                        <img src="./images/bulb.svg" alt="" />
                                        <label className="serv-name">ختبارات الكترونية على كل درس</label>
                                    </div>
                                    {/* <h5 className="card-title">اسم الخدمة</h5> */}
                                </div>
                            </Grid>
                            <Grid item xxs={12} xs={6} sm={4} md={4} lg={3}>
                                <div className="serv-card">
                                    <div className="card-img" style={{ backgroundColor: "#ad82e0" }}>
                                        <img src="./images/connection.svg" alt="" />
                                        <label className="serv-name">
                                            مذكرات تحتوى على شرح وافى لكل درس وتضمن تمارين تدريبية
                                        </label>
                                    </div>
                                    {/* <h5 className="card-title">اسم الخدمة</h5> */}
                                </div>
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                {/* Lectures */}
                <div className="lectures">
                    <div className="lecture-afte">
                        <img src="./images/dots.png" alt="" />
                    </div>
                    <div className="lecture-afte-2">
                        <img src="./images/dots.png" alt="" />
                    </div>
                    <Container>
                        <div className="section-header">
                            <h6>لوحة الشرف</h6>
                            <h5>لوحة الشرف </h5>
                        </div>
                        <Honors />
                    </Container>
                </div>

                {/* Faq */}
                <div className="faq">
                    <div className="faq-img-af">
                        <img src="./images/big-cir.png" alt="" />
                    </div>
                    <Container>
                        <Grid container spacing={4}>
                            <Grid item xs={12} md={7} lg={6}>
                                <div className="faq-content">
                                    <div className="section-header">
                                        <h6>الاساله</h6>
                                        <h5>الاساله الشائعة</h5>
                                    </div>
                                    <FAQ />
                                </div>
                            </Grid>
                            <Grid item xs={12} md={5} lg={6}>
                                <div className="faq-img">
                                    <img src="./images/faq.svg" alt="" />
                                </div>
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                <div className="live-lectures">
                    <LiveLectures />
                </div>

                <Container>
                    <div className="main-video-parent">
                        <div className="section-header">
                            <h6>كيف</h6>
                            <h5>كيف تعمل منصة</h5>
                        </div>
                        <div className="main-video" style={{ backgroundImage: `url('./images/about.png')` }}>
                            {/* <iframe
                width="100%"
                height="100%"
                style={{ border: "none", position: "absolute" }}
              ></iframe> */}
                            <iframe
                                src="https://player.vdocipher.com/playerAssets/1.x/vdo/embed/index.html#otp=20160313versUSE323zlKbCp3kpCzxIgeJOvwVT2G6jbvNxNAxHoEZnzsuhiiBxF&playbackInfo=eyJ2aWRlb0lkIjoiYzI3ZWM0N2VjNDA0NDk5YmJjNTU1NGEwYWI3ZjE2N2UifQ=="
                                style={{
                                    // border: "0",
                                    border: "none",
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
                    </div>
                </Container>

                {/* what they say */}
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
                            <Grid item xs={12} sm={6} lg={12}>
                                <div className="wrapper">
                                    <div className="btn-more">
                                        <Link to="/add-review">مشاهدة المزيد</Link>
                                    </div>
                                    <div className="btn-more btn-link">
                                        <Link to="/add-review">اضف تعليق</Link>
                                    </div>
                                </div>
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                {/* why */}
                <div className="why-patform" style={{ backgroundImage: `url('./images/why-wait-bg.png')` }}>
                    <Container>
                        <Grid container>
                            <Grid item xs={12} md={7} lg={6}>
                                <div className="why-patform-content">
                                    <div className="section-header">
                                        <h6>لماذا</h6>
                                        <h5 className="head">لماذا المنصه</h5>
                                    </div>
                                    <ul>
                                        <li>فى كل درس جديد هنراجع سوا على الدروس القديمة </li>
                                        <li>تقدر تحمل مذكرات كل درس وتطبعها وتذاكر من البيت</li>
                                        <li>تقدر تتأكد من استيعابك لكل درس لما تحل الامتحان الى عليه</li>
                                        <li>مرن نفسك اكتر وحل التمارين الموجودة فى المذكرات</li>
                                        <li>هتعرف ايه الدروس الى محتاج تذاكرها قبل ما تبدأ اى درس</li>
                                        <li>منحنى موجود فى صفحتك وبيوضح تقدمك فى امتحانات المراجعة</li>
                                    </ul>
                                </div>
                            </Grid>
                            <Grid item xs={12} md={5} lg={6}>
                                <div className="why-platform-img">
                                    <img src="./images/why-2.svg" alt="" />
                                </div>
                            </Grid>
                        </Grid>
                    </Container>
                </div>

                {/* number */}
                <div className="number">
                    <div className="number-af">
                        <img src="./images/sercice-img-bg.png" alt="" />
                    </div>
                    <Container>
                        <HomeNumbers />
                    </Container>
                </div>
                <Footer />
            </div>
        );
    }
}

export default Home;
