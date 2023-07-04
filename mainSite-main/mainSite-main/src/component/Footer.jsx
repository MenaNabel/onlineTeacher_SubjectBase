import React, { Component } from "react";
import { Container, Grid } from "@mui/material";
import axios from "axios";
import { baseUrl } from "../assets/baseUrl";

class Footer extends Component {
  state = {
    teacherInfo: {},
  };

  componentDidMount() {
    // axios.get(`http://hossam1234-001-site1.ftempurl.com/api/Setting`).then((res) => {
    axios.get(`${baseUrl}api/Setting`).then((res) => {
      this.setState({
        teacherInfo: res.data,
      });
    });
  }

  render() {
    const { teacherInfo } = this.state;
    return (
      <div className="site-footer">
        <footer>
          <Container>
            <Grid container fixed spacing={4}>
              <Grid item xs={12} sm={6} md={4} lg={5}>
                <div className="footer-logo-content">
                  <div className="logo">
                    <img src="./images/logo.svg" alt="" />
                    <p>
                      هنالك العديد من الأنواع المتوفرة لنصوص لوريم إيبسوم، ولكن
                      الغالبية تم تعديلها بشكل ما عبر إدخال بعض النوادر أو
                      الكلمات العشوائية إلى النص. إن كنت تريد أن تستخدم نص لوريم
                      إيبسوم ما، عليك أن تتحقق أولاً أن ليس هناك أي كلمات أو
                      عبارات محرجة أو غير لائقة مخبأة في هذا النص. بينما تعمل
                      جميع مولّدات نصوص لوريم إيبسوم على الإنترنت على إعادة
                      تكرار مقاطع من نص
                    </p>
                  </div>
                  <div className="social">
                    <a href={teacherInfo.whatsappLink} target="_blank">
                      <i className="fab fa-whatsapp"></i>
                    </a>
                    <a href={teacherInfo.telegramLink} target="_blank">
                      <i className="fab fa-telegram"></i>
                    </a>
                    <a href={teacherInfo.facebookLink} target="_blank">
                      <i className="fab fa-facebook-f"></i>
                    </a>
                  </div>
                </div>
              </Grid>
              <Grid item xs={12} sm={6} md={4} lg={3}>
                <div className="important-link">
                  <h5>روابط هامة</h5>
                  <ul className="list-unstyled">
                  <li>
                      <a href="https://www.youtube.com/c/PeterSalama">يوتيوب </a>
                    </li>
                    <li>
                      <a href="https://www.ekb.eg/"> بنك المعرفه المصري</a>
                    </li>
                    <li>
                      <a href="https://z-lib.org/">مكتبه المراجع العالميه </a>
                    </li>
                    <li>
                      <a href="https://www.geogebra.org/"> موقع جيزجيبرا للرسومات</a>
                    </li>
                    <li>
                      <a href="https://www.youtube.com/c/anaHr"> صفحه انا حر للرياضيات والفيزياء</a>
                    </li>
                    <li>
                      <a href="https://www.youtube.com/c/DroosOnline4u"> دروس اونلاين</a>
                    </li>
                  </ul>
                </div>
              </Grid>
              <Grid item xs={12} sm={6} md={4} lg={4}>
                <div className="more-important">
                  <h5>هام جدا</h5>
                  <ul className="list-unstyled">
                    <li>
                      <span>رقم المعلم: </span>
                      <span> {teacherInfo.phoneNumber} </span>
                    </li>
                    <li>
                      <span>رقم السكرتاريه: </span>
                      <span> {teacherInfo.secretarialPhoneNumber} </span>
                    </li>
                    <li>
                      <span>رقم فودافون كاش: </span>
                      <span> {teacherInfo.vodafonCachPhoneNumber} </span>
                    </li>
                    <li>
                      <span>رقم تفعيل الاشتراك : </span>
                      <span>
                        {" "}
                        {teacherInfo.activationSubscriptionPhoneNumber}{" "}
                      </span>
                    </li>
                    <li>
                      <span> العنوان : </span>
                      <span> {teacherInfo.address} </span>
                    </li>
                  </ul>
                </div>
              </Grid>
            </Grid>
          </Container>
        </footer>
        <div className="copy">
          <Container>
            <h5>جميع الحقوق محفوظه</h5>
          </Container>
        </div>
      </div>
    );
  }
}

export default Footer;
