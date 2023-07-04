import React, { Component } from "react";

import axios from "axios";

import { withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import GetUserToken from "../component/getToken";
import { baseUrl } from "../assets/baseUrl";

class ExamTimer extends Component {
  constructor() {
    super();
    this.state = { time: {}, seconds: 5 };
    this.timer = 0;
    this.countDown = this.countDown.bind(this);
  }

  secondsToTime(secs) {
    let hours = Math.floor(secs / (60 * 60));

    let divisor_for_minutes = secs % (60 * 60);
    let minutes = Math.floor(divisor_for_minutes / 60);

    let divisor_for_seconds = divisor_for_minutes % 60;
    let seconds = Math.ceil(divisor_for_seconds);

    let obj = {
      h: hours,
      m: minutes,
      s: seconds,
    };
    return obj;
  }

  componentDidMount() {
    this.setState({ seconds: this.props.examDuration * 60 });
    let timeLeftVar = this.secondsToTime(this.state.seconds);
    this.setState({ time: timeLeftVar });

    if (this.timer == 0 && this.state.seconds > 0) {
      this.timer = setInterval(this.countDown, 1000);
    }
  }

  countDown() {
    const token = GetUserToken();
    // Remove one second, set state so a re-render happens.
    let seconds = this.state.seconds - 1;
    this.setState({
      time: this.secondsToTime(seconds),
      seconds: seconds,
    });

    // Check if we're at zero.
    if (seconds == 0) {
      clearInterval(this.timer);
      toast.info("أنتهى الوقت");
      var count = 0;
      var newcorr = this.props.correctanswers;
      Object.keys(this.props.qarr).map((key, value) => {
        if (this.props.qarr[key] == newcorr[key]) {
          count++;
        }
      });

      var newquestions = this.props.questionarr.shift();
      const exmob = {
        examID: this.props.examId,
        totalDegree: this.props.totalDegree,
        actualDegree: count,
        questions: this.props.questionarr,
      };

      if (this.props.isSubmiteed == true) {
        axios
          .post(`${baseUrl}Student/Exams/ReExam`, exmob, {
            headers: { Authorization: `Bearer ${token}` },
          })
          .then((res) => {
            if (res.status == 200) {
              toast.success("تم انتهاء الوقت وتم التصحيح");
              this.props.history.replace("/profile");
            }
          })
          .catch((error) => {
            toast.error("هناك خطا فى تصحيح الامتحان");
          });
      } else {
        axios
          .post(`${baseUrl}Student/Exams`, exmob, {
            headers: { Authorization: `Bearer ${token}` },
          })
          .then((res) => {
            if (res.status == 200) {
              toast.success("تم انتهاء الوقت وتم التصحيح");
              this.props.history.replace("/profile");
            }
          })
          .catch((error) => {
            toast.error("هناك خطا فى تصحيح الامتحان");
          });
      }

      // this.props.history.replace("/login");
    }
  }

  render() {
    const goTo = (e) => {
      e.preventDefault();
      this.props.history.replace("/");
    };
    return (
      <div className="examtimer">
        <span className="label">دقائق</span>:{" "}
        <span className="time">{this.state.time.m}</span>{" "}
        <span className="label">ثواني</span> :{" "}
        <span className="time">{this.state.time.s}</span>
      </div>
    );
  }
}

export default withRouter(ExamTimer);
