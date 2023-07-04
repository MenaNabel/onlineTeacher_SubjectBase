import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";
import CircularProgress from "@mui/material/CircularProgress";
import Modal from "@mui/material/Modal";
import { Button } from "semantic-ui-react";
import { Modal as Modal1 } from "semantic-ui-react";
import { Redirect } from "react-router-dom";
import jwt_decode from "jwt-decode";

// component
import Footer from "../component/Footer";
import ExamTimer from "../component/ExamTimer";
import GetUserToken from "../component/getToken";

import axios from "axios";
import { toast } from "react-toastify";
import "mathlive";
import ModalReopenExam from "../component/ModalReopenExam";
import { baseUrl } from "../assets/baseUrl";

export default class Exam extends Component {
    state = {
        questions: [],
        qindex: 0,
        correctAnswers: {},
        examStart: false,
        examOpen: true,
        isSubmiteed: false,
        Duration: 0,
        Degree: 0,
        ExamId: null,
        examDate: "",
        expired: false,
        loading: true,
        examSubmitted: false,
        isOpen: true,
        qCount: 0,
    };

    componentDidMount() {
        const examId = this.props.match.params.id;
        this.setState({ ExamId: examId });
        const token = GetUserToken();

        axios
            // .get(`http://hossam1234-001-site1.ftempurl.com/Student/Exams/${examId}`, {
            .get(`${baseUrl}Student/Exams/${examId}`, {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((res) => {
                if (res.status == 200) {
                    this.setState({
                        examDate: res.data.dateAndExamminationExpireTime,
                        examOpen: true,
                        Duration: res.data.duration,
                        Degree: res.data.degree,
                        isSubmiteed: res.data.isSubmited,
                        loading: false,
                    });
                    var x = this.state.examDate;
                    x = x.replace("T", " ");

                    this.setState({ examDate: x });

                    var today = new Date();
                    var date = today.getFullYear() + "-" + (today.getMonth() + 1) + "-" + today.getDate();

                    var time = today.getHours() + ":" + today.getMinutes();
                    var dateTime = date + " " + time;
                    var isLarger = new Date(dateTime) > new Date(x) ? true : false;

                    var examDateAndTime = x.split(" ");

                    if (isLarger == true) {
                        this.setState({ expired: true });
                    }
                    this.getExamQuestions(res);
                }
            })
            .catch((error) => {
                this.setState({
                    examOpen: false,
                    loading: false,
                });
                // this.getExamQuestions();
            });
    }

    componentWillUnmount() {
        // fix Warning: Can't perform a React state update on an unmounted component
        this.setState = (state, callback) => {
            return;
        };
        this.props.history.replace("/profile");

        window.location.reload();
    }

    getExamQuestions(res) {
        const examId = this.props.match.params.id;
        const token = GetUserToken();
        if (res.status == 200) {
            // console.log(res.data.questions);
            this.setState(
                {
                    questions: res.data.questions,
                    examStart: true,
                },
                () => {
                    var newq = {};
                    this.state.questions.map((qu, index) => {
                        newq[qu.id] = qu.correctAnswer;
                        this.setState({
                            correctAnswers: newq,
                        });
                    });
                }
            );
        }

        // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api";
        // axios
        //   .get(`${baseUrl}api/Exams/${examId}`, {
        //     headers: { Authorization: `Bearer ${token}` },
        //   })
        //   .then((res) => {
        //     if (res.status == 200) {
        //       console.log(res.data);
        //       // console.log(res.data.questions);
        //       this.setState(
        //         {
        //           questions: res.data.questions,
        //           examStart: true,
        //         },
        //         () => {
        //           var newq = {};
        //           this.state.questions.map((qu, index) => {
        //             newq[qu.id] = qu.correctAnswer;
        //             this.setState({
        //               correctAnswers: newq,
        //             });
        //           });
        //         }
        //       );
        //     }
        //   });
    }

    // componentDidMount(){
    //   axios.get("https://jsonplaceholder.typicode.com/posts").then((res)=>{
    //     console.log(res);
    //     if(res.status === 200){
    //       this.props.history.replace("/");
    //     }
    //   })
    // }

    render() {
        const showNextQuestion = (e) => {
            e.target.closest(".current-question").style.display = "none";
            e.target.closest(".current-question").nextElementSibling.style.display = "block";
            // this.props.history.replace("/login")
            // this.props.history.replace("/")
        };

        const showPreviosQuestion = (e) => {
            e.target.closest(".current-question").style.display = "none";
            e.target.closest(".current-question").previousElementSibling.style.display = "block";
        };

        var qarr = {};
        var newb = [];
        var questionarr = [];
        var count = 0;
        var questionObj = {
            id: 0,
            correctAnswer: 0,
            actualAnswer: 0,
        };
        questionarr.push(questionObj);
        let unique = [];
        const handelQuestion = (e, id) => {
            // console.log(e.target.value, id);
            var newcorr = this.state.correctAnswers;
            // console.log(inpo)
            var key = `${id}`;
            qarr[id] = e.target.value;
            // console.log(key)
            // qarr.push(e.target.value);
            questionObj = {
                id: key, // question id
                correctAnswer: newcorr[id],
                actualAnswer: e.target.value,
            };

            newb.push(qarr);
            var flag = true;
            // console.log(questionarr);
            questionarr.map((que, index) => {
                flag = true;
                // console.log(que);
                // console.log(que.id, questionObj.id);
                if (que.id != questionObj.id) {
                    // questionarr.push(questionObj);
                    // console.log(que.id, questionObj.id);
                    flag = false;
                } else if (que.id == questionObj.id) {
                    questionarr[index] = questionObj;
                } else {
                    flag = true;
                }
            });
            if (flag == false) {
                questionarr.push(questionObj);
            }

            // unique = [...new Set(questionarr.map(itm => JSON.stringify(itm)))].map(i => JSON.parse(i))
            // newb = `${qarr}.${key} = ${e.target.value}`;
        };

        const submitForm = () => {
            // alert("submit");
            // console.log(qarr)
            // console.log(newb)
            // console.log(this.state.correctAnswers)
            var newcorr = this.state.correctAnswers;
            Object.keys(qarr).map((key, value) => {
                // console.log(qarr[key], value)
                if (qarr[key] == newcorr[key]) {
                    count++;
                }
            });
            this.setState({ qCount: count });
            questionarr.shift();
            // console.log(count);
            // console.log(questionarr);
            const token = GetUserToken();
            const examId = this.props.match.params.id;
            // console.log(unique);
            this.setState({ isOpen: true });
            //document.getElementById("modal").click();
            this.setState({ examSubmitted: true });

            const exmob = {
                examID: examId,
                totalDegree: this.state.Degree,
                actualDegree: count,
                questions: questionarr,
            };

            if (this.state.isSubmiteed == true) {
                axios
                    .post(
                        // "http://hossam1234-001-site1.ftempurl.com/Student/Exams/ReExam",
                        `${baseUrl}Student/Exams/ReExam`,
                        exmob,
                        {
                            headers: { Authorization: `Bearer ${token}` },
                        }
                    )
                    .then((res) => {
                        if (res.status == 200) {
                            toast.success(`تم التصحيح`);
                            this.setState({ examSubmitted: true });
                            this.setState = (state, callback) => {
                                return;
                            };

                            this.props.history.replace("/profile");
                        }
                    })
                    .catch((error) => {
                        toast.error("هناك خطا فى تصحيح الامتحان");
                    });
            } else {
                axios
                    .post(
                        // "http://hossam1234-001-site1.ftempurl.com/Student/Exams",
                        `${baseUrl}Student/Exams`,
                        exmob,
                        {
                            headers: { Authorization: `Bearer ${token}` },
                        }
                    )
                    .then((res) => {
                        if (res.status == 200) {
                            toast.success(`تم التصحيح`);
                            this.props.history.replace("/profile");
                            this.setState({ examSubmitted: true });
                        }
                    })
                    .catch((error) => {
                        toast.error("هناك خطا فى تصحيح الامتحان");
                    });
            }
        };

        const getToken = () => {
            const tokenString = sessionStorage.getItem("token");
            const userToken = JSON.parse(tokenString);

            const token = userToken === null ? false : true;
            return token;
        };

        const reOpenExam = () => {
            // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/Student/";
            const URL_PASE = "https://localhost:44334/Student/";
            const token = GetUserToken();
            // const decoderole = jwt_decode(token);
            // // console.log(decoderole);
            // const urlrole = "StudentID";
            // // console.log(decoderole[urlrole]);
            // const studentId = decoderole[urlrole];
            // const examId = this.props.match.params.id;

            // const reopenob = {
            //   examID: examId,
            //   reason: ""
            // }
            // axios.post(`${URL_PASE}Exams/Re open Exam`, reopenob).then((res)=>{
            //   console.log(res)
            // }).catch((error)=>{
            //   console.log(error)
            // })
        };

        const { examStart, Duration, Degree, ExamId, isSubmiteed, loading, examSubmitted, qCount } = this.state;
        const examId = this.props.match.params.id;

        const token = getToken();
        if (!token) {
            return <Redirect push to="/login" />;
        }

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
                    <h5>الامتحان</h5>{" "}
                </div>
                <div className="exam">
                    {examSubmitted ? (
                        <div>
                            <Modal keepMounted>
                                <div></div>
                            </Modal>
                        </div>
                    ) : null}
                    {this.state.isOpen ? (
                        <div>
                            <Modal keepMounted>
                                <div></div>
                            </Modal>
                        </div>
                    ) : null}

                    <Container>
                        <div className="exam-img-watermark">
                            <img src="./images/logo.svg" alt="" />
                        </div>
                        <div className="section-header">
                            <h6>الامتحان</h6>
                            <h5>الامتحان</h5>
                        </div>
                        <div className="exam-content">
                            {loading ? (
                                <div style={{ display: "flex", justifyContent: "center" }}>
                                    <CircularProgress />
                                </div>
                            ) : null}
                            {examStart && this.state.expired == false ? (
                                <ExamTimer
                                    examDuration={Duration}
                                    questionarr={questionarr} // each object containing actualAnswer and correctAnser and questionId
                                    qarr={qarr} // actual answer
                                    correctanswers={this.state.correctAnswers}
                                    examId={ExamId}
                                    totalDegree={Degree}
                                    isSubmiteed={isSubmiteed}
                                />
                            ) : null}
                            <Grid container>
                                {this.state.examOpen != true || this.state.expired == true ? (
                                    <div className="noExam">
                                        {" "}
                                        <h5>لا يوجد امتحان متوافق معك</h5> <ModalReopenExam examid={examId} />
                                    </div>
                                ) : (
                                    <>
                                        {this.state.questions.map((item, index) => (
                                            <Grid
                                                key={index}
                                                className="current-question"
                                                item
                                                xs={12}
                                                sm={12}
                                                md={12}
                                                lg={12}
                                                style={{ display: index === 0 ? "block" : "none" }}
                                            >
                                                <Grid item xs={12} sm={12} md={12} lg={12}>
                                                    <div className={"question-card"}>
                                                        <Grid item xs={12} sm={12} md={12} lg={12}>
                                                            <div className="card-title description">
                                                                <span className="q-number">{index + 1}</span>
                                                                <span>
                                                                    <math-field id="mf" read-only>
                                                                        {" "}
                                                                        {item.description}{" "}
                                                                    </math-field>
                                                                </span>
                                                            </div>
                                                        </Grid>

                                                        <div className="card-body">
                                                            <Grid container>
                                                                <Grid item xs={12} sm={12} md={6} lg={6}>
                                                                    <div className="form-group ">
                                                                        <input
                                                                            type="radio"
                                                                            name={item.id}
                                                                            id={item.oneAnswer + item.id}
                                                                            value="1"
                                                                            className="form-control"
                                                                            onChange={(e) => handelQuestion(e, item.id)}
                                                                        />{" "}
                                                                        <label htmlFor={item.oneAnswer + item.id}>
                                                                            {" "}
                                                                            <math-field read-only id="mf">
                                                                                {" "}
                                                                                {item.oneAnswer}{" "}
                                                                            </math-field>
                                                                        </label>
                                                                    </div>
                                                                </Grid>
                                                                <Grid item xs={12} sm={12} md={6} lg={6}>
                                                                    <div className="form-group">
                                                                        <input
                                                                            type="radio"
                                                                            name={item.id}
                                                                            id={item.secondAnswer + item.id}
                                                                            value="2"
                                                                            className="form-control"
                                                                            onChange={(e) => handelQuestion(e, item.id)}
                                                                        />{" "}
                                                                        <label htmlFor={item.secondAnswer + item.id}>
                                                                            {" "}
                                                                            <math-field read-only id="mf">
                                                                                {" "}
                                                                                {item.secondAnswer}{" "}
                                                                            </math-field>
                                                                        </label>
                                                                    </div>
                                                                </Grid>
                                                                <Grid item xs={12} sm={12} md={6} lg={6}>
                                                                    <div className="form-group">
                                                                        <input
                                                                            type="radio"
                                                                            name={item.id}
                                                                            id={item.thirdAnswer + item.id}
                                                                            value="3"
                                                                            className="form-control"
                                                                            onChange={(e) => handelQuestion(e, item.id)}
                                                                        />{" "}
                                                                        <label htmlFor={item.thirdAnswer + item.id}>
                                                                            {" "}
                                                                            <math-field read-only id="mf">
                                                                                {" "}
                                                                                {item.thirdAnswer}{" "}
                                                                            </math-field>
                                                                        </label>
                                                                    </div>
                                                                </Grid>
                                                                <Grid item xs={12} sm={12} md={6} lg={6}>
                                                                    <div className="form-group">
                                                                        <input
                                                                            type="radio"
                                                                            name={item.id}
                                                                            id={item.fourthAnswer + item.id}
                                                                            value="4"
                                                                            className="form-control"
                                                                            onChange={(e) => handelQuestion(e, item.id)}
                                                                        />{" "}
                                                                        <label htmlFor={item.fourthAnswer + item.id}>
                                                                            {" "}
                                                                            <math-field read-only id="mf">
                                                                                {" "}
                                                                                {item.fourthAnswer}{" "}
                                                                            </math-field>
                                                                        </label>
                                                                    </div>
                                                                </Grid>
                                                                <Grid item xs={12} sm={12} md={12} lg={12}>
                                                                    {item.fileName != null ? (
                                                                        <div
                                                                            style={{
                                                                                display: "flex",
                                                                                justifyContent: "center",
                                                                                alignItems: "center",
                                                                                marginBottom: 10 + "px",
                                                                                marginTop: 10 + "px",
                                                                            }}
                                                                        >
                                                                            <img
                                                                                src={item.fileName}
                                                                                alt=""
                                                                                style={{
                                                                                    width: 500 + "px",
                                                                                    height: 300 + "px",
                                                                                }}
                                                                            />
                                                                        </div>
                                                                    ) : (
                                                                        <span></span>
                                                                    )}
                                                                </Grid>
                                                            </Grid>
                                                        </div>
                                                        <div className="card-footer">
                                                            {this.state.questions.length - 1 === index ? (
                                                                <button
                                                                    className="btn btn-correct"
                                                                    onClick={submitForm}
                                                                >
                                                                    التصحيح
                                                                </button>
                                                            ) : (
                                                                <button className="btn" onClick={showNextQuestion}>
                                                                    السؤال التالي
                                                                </button>
                                                            )}

                                                            {index === 0 ? (
                                                                ""
                                                            ) : (
                                                                <button className="btn" onClick={showPreviosQuestion}>
                                                                    السؤال السابق
                                                                </button>
                                                            )}
                                                        </div>
                                                    </div>
                                                </Grid>
                                            </Grid>
                                        ))}
                                    </>
                                )}
                            </Grid>
                        </div>
                    </Container>
                </div>
                <Footer />
            </div>
        );
    }
}

// <ul>
// <li>
//   <label class="container">
//     <span class="word-label">
//       لجواب رقم 1 طويل للاختيار وللتاكد من المسافات
//     </span>
//     <input type="radio" name="q-1" />
//     <span class="checkmark"></span>
//   </label>
// </li>
// <li>
//   <label class="container">
//     <span class="word-label">
//       لجواب رقم 1 طويل للاختيار وللتاكد من المسافات
//     </span>
//     <input type="radio" name="q-1" />
//     <span class="checkmark"></span>
//   </label>
// </li>
// <li>
//   <label class="container">
//     <span class="word-label">
//       لجواب رقم 1 طويل للاختيار وللتاكد من المسافات
//     </span>
//     <input type="radio" name="q-1" />
//     <span class="checkmark"></span>
//   </label>
// </li>
// <li>
//   <label class="container">
//     <span class="word-label">
//       لجواب رقم 1 طويل للاختيار وللتاكد من المسافات
//     </span>
//     <input type="radio" name="q-1" />
//     <span class="checkmark"></span>
//   </label>
// </li>
// </ul>
