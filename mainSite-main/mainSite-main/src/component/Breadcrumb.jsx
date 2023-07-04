import React, { Component } from "react";

// material ui
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";

class Breadcrumb extends Component {
  render() {
    return (
      <div>
        <div
          className="breadcrumb"
          style={{
            backgroundImage: `url('${process.env.PUBLIC_URL}images/breadcrumb.jpg')`,
          }}
          data-aos="fade-up"
          data-aos-duration="2000"
        >
          <h5>{this.props.head}</h5>
          {/* <ul className="list-unstyled">
            <li>
              <a href="">الرئيسية</a>
            </li>
            <li>
              <a href="">{this.props.head}</a>
            </li>
          </ul> */}
        </div>
      </div>
    );
  }
}

export default Breadcrumb;
