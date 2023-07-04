import React, { Component } from "react";

// react router dom
import { HashRouter as Router, Switch, Route, Link, Redirect } from "react-router-dom";


// css
import "./assets/css/style.css";
import "./assets/sass/new-style.css";

// component
import Navbar from "./component/Navbar";
import Footer from "./component/Footer";
import Sidebar from "./component/Sidebar";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// pages
import Home from "./pages/Home";
import FreeLectures from "./pages/FreeLectures";
import NotFound from "./pages/NotFound";
import Lectures from "./pages/Lectures";
import Exam from "./pages/Exam";
import WhoUs from "./pages/WhoUs";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import Profile from "./pages/Profile";
import Levels from "./pages/Levels";
import LevelDetails from "./pages/LevelDetails";
import SubjectDetails from "./pages/SubjectDetails";
import MyLectures from "./pages/MyLectures";
import MonthOfLectures from "./pages/MonthOfLectures";
import LectureDetails from "./pages/LectureDetails";
import AddAreview from "./pages/AddAreview";
import FreeLectureDetails from "./pages/FreeLectureDetails";
import ForgetPassword from "./pages/forget-password";
import ResetPassword from "./pages/ResetPassword";

class App extends Component {
  render() {
    const openMobSideMenu = () => {
      var sidebar = (document.querySelector(".Mysidebar").style.right = 0);
      document.querySelector(".bg-sidenavOpen").style.display = "block";
      document.body.classList.add("openMenuActive");
    };

    const closeSideMenue = () => {
      var sidebar = (document.querySelector(".Mysidebar").style.right =
        "-300px");
      document.querySelector(".bg-sidenavOpen").style.display = "none";
      document.body.classList.remove("openMenuActive");
    };

    const removeSide = (e) => {
      document.querySelector(".Mysidebar").style.right = "-300px";
      e.target.style.display = "none";
      document.body.classList.remove("openMenuActive");
    };

    return (
      <>
      <ToastContainer autoClose={5000} theme="colored" />
      <Router>
        <div class="bg-sidenavOpen" onClick={removeSide}></div>
        <Navbar openMobSideMenu={openMobSideMenu} />
        <Sidebar closeSideMenue={closeSideMenue} />
        <div> 
          <Switch>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/add-review" component={AddAreview} />
            <Route exact path="/lectures/:subid/:motid" component={Lectures} />
            <Route exact path="/lecture-details/:id" component={LectureDetails} />
            <Route exact path="/free-lecture-details/:id" component={FreeLectureDetails} />
            <Route exact path="/exam/:id" component={Exam} />
            <Route exact path="/who-us" component={WhoUs} />
            <Route exact path="/sign-up" component={SignUp} />
            <Route exact path="/profile" component={Profile} />
            <Route exact path="/levels" component={Levels} />
            <Route exact path="/mylectures" component={MyLectures} />
            <Route exact path="/ResetPassword"  component={ResetPassword} />
            <Route exact path="/free" component={FreeLectures} />
            <Route exact path="/forget-password" component={ForgetPassword} />
            <Route exact path="/month-of-lecture/:id" component={MonthOfLectures} />
            <Route exact path="/level-details/:id" component={LevelDetails} />
            <Route exact path="/subject-details/:id" component={SubjectDetails} />
            <Route exact component={NotFound} />
            <Route exact path="*" component={NotFound} />
            <Route exact path="/404" component={NotFound} />
            <Redirect to="/404" />
          </Switch>
        </div>
        {/* <Footer /> */}
      </Router>
      </>
    );
  }
}

export default App;
