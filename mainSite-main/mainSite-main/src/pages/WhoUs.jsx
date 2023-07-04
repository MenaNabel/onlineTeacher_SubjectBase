import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";

// component
import Breadcrumb from "../component/Breadcrumb";
import Footer from "../component/Footer";

class WhoUs extends Component {
    render() {
        return (
            <div>
                <Breadcrumb head="من نحن" />
                <div className="about-us">
                    <div className="who-us">
                        <Container>
                            <div className="who-content">
                                <img src="./images/Excel from Beginner to Advanced.jpg" alt="" />
                                <div className="section-header">
                                    <h6>من نحن</h6>
                                    <h5>هذا النص هو مثال لنص</h5>
                                </div>
                                <p>
                                    ذا النص هو مثال لنص يمكن أن يستبدل في نفس المساحة، لقد تم توليد هذا النص من مولد
                                    النص العربى، حيث يمكنك أن تولد مثل هذا النص أو العديد من النصوص الأخرى إضافة إلى
                                    زيادة عدد الحروف التى يولدها التطبيق. إذا كنت تحتاج إلى عدد أكبر من الفقرات يتيح لك
                                    مولد النص العربى زيادة عدد الفقرات كما تريد، النص لن يبدو مقسما ولا يحوي أخطاء
                                    لغوية، مولد النص العربى مفيد لمصممي المواقع على وجه الخصوص، حيث يحتاج العميل فى كثير
                                    من الأحيان أن يطلع على صورة حقيقية لتصميم الموقع
                                </p>
                                <p>
                                    ذا النص هو مثال لنص يمكن أن يستبدل في نفس المساحة، لقد تم توليد هذا النص من مولد
                                    النص العربى، حيث يمكنك أن تولد مثل هذا النص أو العديد من النصوص الأخرى إضافة إلى
                                    زيادة عدد الحروف التى يولدها التطبيق. إذا كنت تحتاج إلى عدد أكبر من الفقرات يتيح لك
                                    مولد النص العربى زيادة عدد الفقرات كما تريد، النص لن يبدو مقسما ولا يحوي أخطاء
                                    لغوية، مولد النص العربى مفيد لمصممي المواقع على وجه الخصوص، حيث يحتاج العميل فى كثير
                                    من الأحيان أن يطلع على صورة حقيقية لتصميم الموذا النص هو مثال لنص يمكن أن يستبدل في
                                    نفس المساحة، لقد تم توليد هذا النص من مولد النص العربى، حيث يمكنك أن تولد مثل هذا
                                    النص أو العديد من النصوص الأخرى إضافة إلى زيادة عدد الحروف التى يولدها التطبيق. إذا
                                    كنت تحتاج إلى عدد أكبر من الفقرات يتيح لك مولد النص العربى زيادة عدد الفقرات كما
                                    تريد، النص لن يبدو مقسما ولا يحوي أخطاء لغوية، مولد النص العربى مفيد لمصممي المواقع
                                    على وجه الخصوص، حيث يحتاج العميل فى كثير من الأحيان أن يطلع على صورة حقيقية لتصميم
                                    الموقع
                                </p>
                            </div>
                            <div class="aboutCards">
                                <Grid container spacing={4}>
                                    <Grid item xs={12} sm={6} md={6} lg={4}>
                                        <div class="aboutCar">
                                            <div
                                                class="card-img"
                                                style={{
                                                    backgroundImage: `url('./images/header.jpg')`,
                                                    height: "109px",
                                                }}
                                            >
                                                <h5>المهمه</h5>
                                            </div>
                                            <p class="card-text">
                                                مساعدتك لفهم الرياضيات بأستخدام أحدث الوسائل التعليمية وتدريبك على حل
                                                امتحانات متطورة بصفة مستمرة وتقييم مستواك بأستخدام التغذية الراجعة
                                            </p>
                                        </div>
                                    </Grid>
                                    <Grid item xs={12} sm={6} md={6} lg={4}>
                                        <div class="aboutCar">
                                            <div
                                                class="card-img"
                                                style={{
                                                    backgroundImage: `url('./images/header.jpg')`,
                                                    height: "188px",
                                                }}
                                            >
                                                <h5>الروؤية</h5>
                                            </div>
                                            <p class="card-text">طالب فاهم - قادر على حل المشكلات بشكل متجدد</p>
                                        </div>
                                    </Grid>
                                    <Grid item xs={12} sm={6} md={6} lg={4}>
                                        <div class="aboutCar">
                                            <div
                                                class="card-img"
                                                style={{
                                                    backgroundImage: `url('./images/header.jpg')`,
                                                    height: "271px",
                                                }}
                                            >
                                                <h5>الاهداف</h5>
                                            </div>
                                            <p class="card-text">
                                                ان نجعلك دائماً
                                                <ul>
                                                    <li>واثقاً فى قدراتك </li>
                                                    <li>مستعد دائماً للأمتحان</li>
                                                    <li>عدم إهمال أى جزء من المنهج </li>
                                                </ul>
                                            </p>
                                        </div>
                                    </Grid>
                                </Grid>
                            </div>
                        </Container>
                    </div>
                </div>
                <Footer />
            </div>
        );
    }
}

export default WhoUs;
