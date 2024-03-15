import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
    vus: 100, // virtual users
    duration: "20s", // duration of the test in seconds
};

const data = new SharedArray("users",() => {
    const result = JSON.parse(open("../seed/users.json"));
    return result
});

export default () => {
    const validPSPToken = "VPZxeLCk9vxZ5bOqtzJduCJARPLH1ruyrI89GY0RCdJ6cHvzJ4FlAHsSG85Wmy9i"
    const user = data[Math.floor(Math.random() * data.length)]
    console.log(data.length)

    const keyData = {
        key: {
            value: `${Date.now() + 1}@gmail.com`,
            type: "Email",
        },
        user: {
            cpf: user.CPF,
        },
        account: {
            number: Number(generateRandomNumberString(8)),
            agency: Number(generateRandomNumberString(4)),
        }
    }
    const body = JSON.stringify(keyData)

    const headers = {"Content-Type": "application/json", "Authorization": `${validPSPToken}`}
    const response = http.post("http://localhost:5090/keys", body, {headers: headers})
    console.log(response)
    // sleep(1)
};

function generateRandomNumberString(digits) {
    let randomNumberString = '';
    const numDigits = digits;

    for (let i = 0; i < numDigits; i++) {
        const randomDigit = Math.floor(Math.random() * 10);
        randomNumberString += randomDigit.toString();
    }

    return randomNumberString;
};