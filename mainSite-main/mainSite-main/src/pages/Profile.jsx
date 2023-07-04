import React, { Component } from "react";

import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

// import 'mdb-react-ui-kit/dist/css/mdb.min.css'
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Redirect } from "react-router-dom";
import axios from "axios";

import { MDBDataTable } from "mdbreact";

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
import { baseUrl } from "../assets/baseUrl";
import { Pagination, Stack } from "@mui/material";

function TabPanel(props) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box sx={{ p: 3 }}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}

TabPanel.propTypes = {
    children: PropTypes.node,
    index: PropTypes.number.isRequired,
    value: PropTypes.number.isRequired,
};

function a11yProps(index) {
    return {
        id: `vertical-tab-${index}`,
        "aria-controls": `vertical-tabpanel-${index}`,
    };
}

class Profile extends Component {
    state = {
        value: 0,
        newstudentId: 0,
        disabledbtn: true,
        studentDetails: {},
        levelDetails: {},
        studentSbject: [],
        levels: [],
        Name: "",
        ImageOrFile: "",
        Phone: "",
        City: "",
        LevelID: "",
        UserID: "",
        grades: [],
        newgrades: [],
        examNames: [],
        loadintable: false,
        studentId: 0
    };

     async componentDidMount() {
        const tokenString = sessionStorage.getItem("token");
        // const URL_PASE = "http://hossam1234-001-site1.ftempurl.com/api/";
        const URL_PASE = "https://localhost:44334/api/";
        const userToken = JSON.parse(tokenString);

        if (userToken != null) {
            const decoderole = jwt_decode(userToken);
            const urlrole = "StudentID";
            this.setState({
                newstudentId: decoderole[urlrole],
                UserID: decoderole[urlrole],
            });
            await axios
                .get(`${baseUrl}api/Students/me`, {
                    headers: { Authorization: `Bearer ${userToken}` },
                })
                .then((res) => {
                    this.setState({
                        studentDetails: res.data,
                        levelDetails: res.data.level,
                        Name: res.data.name,
                        Phone: res.data.phone,
                        ImageOrFile: res.data.image,
                        City: res.data.city,
                        LevelID: res.data.level.id,
                        studentId: res.data.id
                    });
                })
                .catch((error) => {
                    console.log(error);
                });

                await axios
                // .get("http://hossam1234-001-site1.ftempurl.com/api/Subscribtion/my", {
                .get(`${baseUrl}api/Subscribtion/my`, {
                    headers: { Authorization: `Bearer ${userToken}` },
                })
                .then((res) => {
                    if (res.status == 200) {
                        this.setState({
                            studentSbject: res.data,
                        });
                    }
                })
                .catch((error) => {
                    toast.error("هناك خطا ما");
                    console.log(error);
                });

            const token = GetUserToken();

            await axios
                .get(`${baseUrl}api/Levels`, {
                    headers: { Authorization: `Bearer ${userToken}` },
                })
                .then((res) => {
                    this.setState({
                        levels: res.data.items,
                    });
                });

             await axios
                .get(
                    `${baseUrl}Student/Exams/Grades/${this.state.studentId}`,
                    // `http://hossam1234-001-site1.ftempurl.com/Student/Exams/Grades/${this.state.newstudentId}`,
                    {
                        headers: { Authorization: `Bearer ${userToken}` },
                    }
                )
                .then((res) => {
                    this.setState(
                        {
                            grades: res.data,
                        },
                        () => {
                            var newg = [];
                            var newname = [];
                            this.state.grades.map((ge, index) => {
                                newg.push(ge.degree);
                                newname.push(ge.examName);
                                this.setState({
                                    newgrades: newg,
                                    examNames: newname,
                                });
                            });
                        }
                    );

                    
                })
                .catch((error) => {
                    toast.error("هناك خطا ما");
                    console.log(error);
                });
        }
        
    }

     handleCLick = (e) => {
            
        setTimeout(() => {
            document.querySelector('[aria-label="Previous"]').innerHTML = "السابق";
            document.querySelector('[aria-label="Next"]').innerHTML = "التالي";
            document.querySelector(".dataTables_length label").childNodes[0].textContent = "أظهار الحقول";
            document.querySelector(".mdb-datatable-filter input").placeholder = "بحث";
    
        }, 100);
        };


    render() {
        
        const handleChange = (event, newValue) => {
            this.setState({
                value: newValue,
            });
        };

        const datatable = {
            columns: [
                { label: "اسم الامتحان", field: "examName" },
                { label: "الدرجة", field: "degree" },
                // { label: "الاختيارات", field: "option" },
            ],
            rows: this.state.grades
        };
        

        // const enableEdit=(e)=>{
        //   e.preventDefault();
        //   this.setState({
        //     disabledbtn: false
        //   })
        // }

        const handelSetting = (e) => {
            this.setState({
                [e.target.name]: e.target.value,
            });
        };

        const handelUploadPicture = (e) => {
            this.setState({
                // picturePreview: URL.createObjectURL(e.target.files[0]),
                ImageOrFile: e.target.files[0],
            });
        };

        const handleSubjectsClick = (e) => {
            this.props.history.replace("/mylectures");
        };

       

        

        const submitForm = (e) => {
            e.preventDefault();
            const state = { ...this.state };
            // const PASE_URL = "http://hossam1234-001-site1.ftempurl.com/api/";
            const PASE_URL = "https://localhost:44334/api/";
            const token = GetUserToken();
            delete state.value;
            delete state.newstudentId;
            delete state.disabledbtn;
            delete state.studentDetails;
            delete state.levelDetails;
            delete state.studentSbject;
            delete state.levels;

            const formData = new FormData();
            formData.append("file", this.state.ImageOrFile);
            Object.keys(state).forEach((key) => {
                formData.append(key, this.state[key]);
            });

            axios
                .put(`${baseUrl}api/Students`, formData, {
                    headers: { Authorization: `Bearer ${token}` },
                })
                .then((res) => {
                    toast.success("تم التعديل بنجاح");
                    const tokenString = sessionStorage.getItem("token");
                    const userToken = JSON.parse(tokenString);
                    axios
                        // .get("http://hossam1234-001-site1.ftempurl.com/api/Students/me", {
                        .get(`${baseUrl}api/Students/me`, {
                            headers: { Authorization: `Bearer ${userToken}` },
                        })
                        .then((res) => {
                            this.setState({
                                studentDetails: res.data,
                                levelDetails: res.data.level,
                                Name: res.data.name,
                                Phone: res.data.phone,
                                ImageOrFile: res.data.image,
                                City: res.data.city,
                                LevelID: res.data.level.id,
                            });
                        });
                })
                .catch((error) => {
                    toast.error("تأكد من ادخال البيانات");
                    console.log(error);
                });
        };

        const getToken = () => {
            const tokenString = sessionStorage.getItem("token");
            const userToken = JSON.parse(tokenString);
            const token = userToken === null ? false : true;
            return token;
        };

        const {
            value,
            newstudentId,
            disabledbtn,
            studentDetails,
            levelDetails,
            studentSbject,
            levels,
            Name,
            Phone,
            ImageOrFile,
            City,
            LevelID,
            UserID,
            loadintable
        } = this.state;
        const token = getToken();
        if (!token) {
            return <Redirect push to="/login" />;
        }
       
    
        return (
            <div>
                <div>
                    <Breadcrumb head="الملف الشخصي" />
                    <Container>
                        <div className="my-account">
                            <Grid container>
                                <Grid item xs={12} md={6} lg={3}>
                                    <Tabs
                                        orientation="vertical"
                                        variant="scrollable"
                                        value={value}
                                        onChange={handleChange}
                                        aria-label="Vertical tabs example"
                                        sx={{ borderRight: 1, borderColor: "divider" }}
                                    >
                                        <Tab label="الملف الشخصي" {...a11yProps(0)} />
                                        <Tab label="درجاتي" {...a11yProps(1)} />
                                        <Tab label="موادي" {...a11yProps(2)} onClick={handleSubjectsClick} />
                                        <Tab label="الاعدادات" {...a11yProps(3)} />
                                        <Tab label="جدول الدرجات" {...a11yProps(4)} onClick={() => this.handleCLick()}/>
                                    </Tabs>
                                </Grid>
                                <Grid item xs={12} md={6} lg={9}>
                                    <TabPanel value={value} index={0}>
                                        <form action="" className="profile-form">
                                            <Grid container spacing={3}>
                                                <div className="user-card">
                                                    <div
                                                        className="user-imag"
                                                        style={{
                                                            backgroundImage: `url(${studentDetails.image})`,
                                                        }}
                                                    >
                                                        {" "}
                                                    </div>
                                                    <h5 className="user-name">{studentDetails.name}</h5>
                                                    <p className="user-phone">{studentDetails.phone}</p>
                                                    <p className="user-laevel">{levelDetails.levelName}</p>
                                                </div>
                                            </Grid>
                                        </form>
                                    </TabPanel>
                                    <TabPanel value={value} index={1}>
                                        <StudenMarksChart
                                            newgrades={this.state.newgrades}
                                            examNames={this.state.examNames}
                                        />
                                    </TabPanel>
                                    <TabPanel value={value} index={2}>
                                        <Grid container spacing={3}>
                                            {studentSbject.map((sub, index) => (
                                                <Grid item xs={12} sm={6} md={4} lg={4} key={index}>
                                                    <div
                                                        onClick={(e) => {
                                                            this.props.history.push(
                                                                "/subject-details/" + sub.subjectID
                                                            );
                                                        }}
                                                        style={{ cursor: "pointer" }}
                                                        to={"/subject-details/" + sub.id}
                                                        className="sub-card"
                                                    >
                                                        {/* <div
                              className="card-img"
                              style={{
                                backgroundImage: `url(${sub.imagePath})`,
                              }}
                            ></div> */}
                                                        <div className="card-body">
                                                            <h5 className="card-title">{sub.subjectName}</h5>
                                                            {sub.isActive ? (
                                                                <h6 className="card-price">تم الموافقه على الاشتراك</h6>
                                                            ) : (
                                                                <h6 className="card-price notaccept">
                                                                    بانتظار الموافقه على الاشتراك
                                                                </h6>
                                                            )}
                                                        </div>
                                                    </div>
                                                </Grid>
                                            ))}
                                        </Grid>
                                    </TabPanel>
                                    <TabPanel value={value} index={3}>
                                        <form action="" onSubmit={submitForm} className="profile-form">
                                            <Grid container spacing={3}>
                                                <Grid item xs={12} lg={6}>
                                                    <div className="form-group">
                                                        <label htmlFor="">الاسم</label>
                                                        <input
                                                            onChange={handelSetting}
                                                            type="text"
                                                            name="Name"
                                                            className="form-control"
                                                            value={Name}
                                                        />
                                                    </div>
                                                </Grid>
                                                <Grid item xs={12} lg={6}>
                                                    <div className="form-group">
                                                        <label htmlFor="">الهاتف</label>
                                                        <input
                                                            onChange={handelSetting}
                                                            type="text"
                                                            name="Phone"
                                                            className="form-control"
                                                            value={Phone}
                                                        />
                                                    </div>
                                                </Grid>
                                                <Grid item xs={12} lg={6}>
                                                    <div className="form-group">
                                                        <label htmlFor="">المدينه</label>
                                                        <input
                                                            onChange={handelSetting}
                                                            type="text"
                                                            name="City"
                                                            className="form-control"
                                                            value={City}
                                                        />
                                                    </div>
                                                </Grid>
                                                <Grid item xs={12} lg={6}>
                                                    <div className="form-group">
                                                        <label htmlFor="">المستوى</label>
                                                        {/* <input className="form-control" value={studentDetails.city} /> */}
                                                        <select
                                                            name="LevelID"
                                                            value={LevelID}
                                                            className="form-control"
                                                            id=""
                                                            onChange={handelSetting}
                                                        >
                                                            {levels.map((lev, index) => (
                                                                <option key={index} value={lev.id}>
                                                                    {lev.levelName}
                                                                </option>
                                                            ))}
                                                        </select>
                                                    </div>
                                                </Grid>
                                                <Grid item xs={12} lg={6}>
                                                    <div className="form-group">
                                                        <label htmlFor="">الصوره</label>
                                                        <input
                                                            name="imageOrFile"
                                                            type="file"
                                                            className="form-control"
                                                            onChange={handelUploadPicture}
                                                        />
                                                    </div>
                                                </Grid>
                                                <Grid item xs={12} lg={12}>
                                                    <div className="btn-submit">
                                                        <button>حفظ الاعدادات</button>
                                                    </div>
                                                </Grid>
                                            </Grid>
                                        </form>
                                    </TabPanel>
                                    <TabPanel value={value} index={4} >
                                        <div className="main-content">
                                            <div className="dashboard-header">
                                                <h5>الدرجات</h5>
                                                {/* <div className="add-aNew">
            <Link className="add-new-lnk" to="/add-new-level">اضافة طالب</Link>
          </div> */}
                                            </div>
                                            <div className="dashboard-content">
                                                <div className="dasboard-box">
                                                    <MDBDataTable
                                                        entriesOptions={[5, 10, 15, 20]}
                                                        entries={5}
                                                        pagesAmount={4}
                                                        hover
                                                        large
                                                        responsive
                                                        data={datatable}
                                                    />
                                                    {loadintable ? (
                                                        <div className="loading-par">
                                                            <div className="sp sp-volume"></div>
                                                            <h5>برجاء الانتظار</h5>
                                                        </div>
                                                    ) : null}
{/* 
                                                    <Stack spacing={2}>
                                                        <Pagination style={{ direction: 'ltr' }} count={5} shape="rounded" onChange={this.handlePaginationChange} />
                                                    </Stack> */}
                                                </div>
                                            </div>
                                        </div>
                                    </TabPanel>
                                </Grid>
                            </Grid>
                        </div>
                    </Container>
                </div>
                <Footer />
            </div>
        );
    }
}

export default Profile;
