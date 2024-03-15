import http from "k6/http";

export const options = {
    vus: 100, // virtual users
    duration: "10s", // duration of the test in seconds
}

export default () => {
    http.get("http://localhost:5090/health");
}