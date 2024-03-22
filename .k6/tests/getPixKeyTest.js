import http from 'k6/http';
import { sleep } from 'k6';
import { SharedArray } from 'k6/data';

export const options = {
  vus: 120, // virtual users
  duration: '60s', // duration of the test in seconds
};

const keysData = new SharedArray('pixKeys', () => {
  const result = JSON.parse(open('../seed/pixKeys.json'));
  return result;
});

export default () => {
  const validPSPToken =
    'VPZxeLCk9vxZ5bOqtzJduCJARPLH1ruyrI89GY0RCdJ6cHvzJ4FlAHsSG85Wmy9i';
  const randomPixKey = keysData[Math.floor(Math.random() * keysData.length)];

  const headers = {
    'Content-Type': 'application/json',
    Authorization: validPSPToken,
  };

  const response = http.get(
    `http://localhost:8080/keys/${randomPixKey.Type}/${randomPixKey.Value}`,
    { headers },
  );

  if (response.status != 200) {
    console.log(response.body);
  }
};
