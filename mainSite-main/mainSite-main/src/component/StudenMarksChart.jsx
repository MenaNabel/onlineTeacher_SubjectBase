import React, { Component } from "react";

// chart js
import { Line } from "react-chartjs-2";

class StudenMarksChart extends Component {
  // state = {
  //   marks: [29, 30, 12, 10, 14, 10, 15, 20, 40],
  // }
  //this.props.examNames
  render() {
    const data = {
      labels: [""].concat(this.props.examNames).concat([""]),
      datasets: [
        {
          label: "#Marks",
          data: [0].concat(this.props.newgrades),
          backgroundColor: [
            "rgba(255, 99, 132, 0.2)",
            "rgba(54, 162, 235, 0.2)",
            "rgba(255, 206, 86, 0.2)",
            "rgba(75, 192, 192, 0.2)",
            "rgba(153, 102, 255, 0.2)",
            "rgba(255, 159, 64, 0.2)",
          ],
          borderColor: [
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(255, 159, 64, 1)",
          ],
          borderWidth: 1,
        },
      ],
    };

    const options = {
      scales: {
        x: {
          type: "category",
          beginAt: 5,
        },

        yAxes: [
          {
            ticks: {
              beginAtZero: true,
            },
          },
        ],
      },
    };
    return (
      <div>
        <Line data={data} options={options} />
      </div>
    );
  }
}

export default StudenMarksChart;
