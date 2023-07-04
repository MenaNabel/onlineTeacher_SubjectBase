import React, { Component } from "react";

import Container from "@mui/material/Container";
import { Link } from "react-router-dom";

class NotFound extends Component {
  render() {
    return (
      <div>
        <Container>
          <div className="nofound">
            <img src="./images/not-found.svg" alt="" />
            <Link to="/" className="to-home">
              الرجوع الى الرئيسية
            </Link>
          </div>
        </Container>
      </div>
    );
  }
}

export default NotFound;
