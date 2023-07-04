import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import axios from "axios";
import GetUserToken from "./getToken";
import { toast } from "react-toastify";
import Loading from "./Loading";
import { useHistory } from "react-router-dom";
import { baseUrl } from "../assets/baseUrl";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

export default function BasicModal(props) {
  const [open, setOpen] = React.useState(false);
  const [reason, setReason] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const history = useHistory();
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const handelChange = (e) => {
    setReason(e.target.value);
  };

  const examId = props.examid;
  const token = GetUserToken();

  const handelSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    const reopenob = {
      examID: examId,
      reason: reason,
    };
    axios
      .post(`${baseUrl}Student/Exams/Re open Exam`, reopenob, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        if (res.status == 200) {
          toast.success("تم ارسال الطلب بنجاح");
          setTimeout(() => {
            setLoading(false);
            history.push("/profile");
          }, 2000);
        }
      })
      .catch((error) => {
        setLoading(false);
        toast.error("يوجد مشكله في اعاده فتح الامتحان");
      });
  };

  return (
    <div className="reopenexammodal">
      <Button onClick={handleOpen}>طلب اعاده الامتحان</Button>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            ما هو سبب طلب اعاده الامتحان؟
          </Typography>
          <form action="" onSubmit={handelSubmit}>
            <textarea
              name="reason"
              id=""
              onChange={handelChange}
              className="form-control"
              placeholder="سبب طلب اعاده الامتحان"
            ></textarea>
            <button className="btn">تاكيد طلب اعادة الامتحان</button>
          </form>
          {loading ? (
            <div className="loginloading">
              <Loading />
            </div>
          ) : null}
        </Box>
      </Modal>
    </div>
  );
}
