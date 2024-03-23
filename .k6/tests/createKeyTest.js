import http from 'k6/http';
import { sleep } from 'k6';
import { SharedArray } from 'k6/data';

export const options = {
  scenarios: {
    high_usage: {
      executor: 'constant-arrival-rate',
      duration: '60s',
      preAllocatedVUs: 100,
      rate: 30000,
      timeUnit: '60s',
    },
  },
};
const data = new SharedArray('users', () => {
  const result = JSON.parse(open('../seed/users.json'));
  return result;
});

const PSPsData = new SharedArray('paymentProviders', () => {
  const result = JSON.parse(open('../seed/paymentProviders.json'));
  return result;
});

export default () => {
  const validPSPToken =
    PSPsData[Math.floor(Math.random() * PSPsData.length)].Token;
  const user = data[Math.floor(Math.random() * data.length)];

  const keyData = {
    key: {
      value: `${Date.now()}${Math.floor(Math.random() * 100)}`,
      type: 'Random',
    },
    user: {
      cpf: user.CPF,
    },
    account: {
      number: generateRandomNumber(1, 99999999),
      agency: generateRandomNumber(1, 9999),
    },
  };
  const body = JSON.stringify(keyData);
  console.log(`Creating key: ${keyData.key.value}`);

  const headers = {
    'Content-Type': 'application/json',
    Authorization: `${validPSPToken}`,
  };
  const response = http.post('http://localhost:5089/keys', body, {
    headers: headers,
  });

  if (response.status !== 201) {
    console.log(`Failed to create key: ${response.body}`);
  }
};

function generateRandomNumber(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}
