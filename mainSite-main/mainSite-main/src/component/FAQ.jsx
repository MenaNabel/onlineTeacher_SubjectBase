import * as React from "react";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import Typography from "@mui/material/Typography";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

export default function SimpleAccordion() {
    return (
        <div>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />} aria-controls="panel1a-content" id="panel1a-header">
                    <h5>ما المراحل المتوفرة بالمنصة؟</h5>
                </AccordionSummary>
                <AccordionDetails>
                    <p className="card-text" style={{ color: "black" }}>
                        متوفر منهج الرياضيات المصرى بالثلاث مراحل الثانوية العامة
                    </p>
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />} aria-controls="panel2a-content" id="panel2a-header">
                    <h5>هل المحاضرات مقيدة بوقت معين خلال العام الدراسى؟</h5>
                </AccordionSummary>
                <AccordionDetails>
                    <p className="card-text" style={{ color: "black" }}>
                        الامتحانات والمحاضرات هتكون متوفرة معك طول العام الدراسى
                    </p>
                </AccordionDetails>
            </Accordion>

            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />} aria-controls="panel3a-content" id="panel3a-header">
                    <h5>ما الاجهزة التى تعمل عليها المنصة؟</h5>
                </AccordionSummary>
                <AccordionDetails>
                    <p className="card-text" style={{ color: "black" }}>
                        جميع أجهزة الكمبيوتر واللاب توب والتابلت والموبايل
                    </p>
                </AccordionDetails>
            </Accordion>

            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />} aria-controls="panel4a-content" id="panel4a-header">
                    <h5>كيف يمكننى التواصل معكم؟</h5>
                </AccordionSummary>
                <AccordionDetails>
                    <p className="card-text" style={{ color: "black" }}>
                        عن طريق أرقامنا الموضحة بالصفحة أو عن طريق مجموعات التليجرام وصفحة الفيس
                    </p>
                </AccordionDetails>
            </Accordion>
        </div>
    );
}
