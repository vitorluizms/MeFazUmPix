import http from 'k6/http';
import { sleep } from 'k6';
import { SharedArray } from 'k6/data';

export const options = {
  scenarios: {
    high_usage: {
      executor: 'constant-arrival-rate',
      duration: '60s',
      preAllocatedVUs: 90,
      rate: 3000,
      timeUnit: '60s',
    },
  },
};
const MAX_PAYMENT_AMOUNT = 300000;

const keysData = new SharedArray('pixKeys', () => {
  const result = JSON.parse(open('../seed/pixKeys.json'));
  return result;
});

const PSPsData = new SharedArray('paymentProviders', () => {
  const result = JSON.parse(open('../seed/paymentProviders.json'));
  return result;
});

const accountsData = new SharedArray('accounts', () => {
  const result = JSON.parse(open('../seed/accounts.json'));
  return result;
});

const usersData = new SharedArray('users', () => {
  const result = JSON.parse(open('../seed/users.json'));
  return result;
});

export default () => {
  console.log('Creating payment...');
  const randomPixKey = keysData[Math.floor(Math.random() * keysData.length)];
  const PSP_TOKEN = PSPsData[Math.floor(Math.random() * PSPsData.length)].Token;
  const randomAccount =
    accountsData[Math.floor(Math.random() * accountsData.length)];
  console.log(randomAccount);
  const body = {
    user: {
      cpf: randomAccount.Cpf,
    },
    account: {
      number: randomAccount.Number,
      agency: randomAccount.Agency,
    },
    key: {
      value: randomPixKey.Value,
      type: randomPixKey.Type,
    },
    amount: Math.floor(Math.random() * MAX_PAYMENT_AMOUNT),
    description: `${new Date(Date.now()).toISOString()}`,
  };

  const bodyString = JSON.stringify({
    origin: { user: body.user, account: body.account },
    destiny: { key: body.key },
    amount: body.amount,
    description: body.description,
  });

  const headers = {
    'Content-Type': 'application/json',
    Authorization: PSP_TOKEN,
  };

  const response = http.post('http://localhost:5089/payments', bodyString, {
    headers,
  });

  if (response.status != 201) {
    console.log(response.body);
  }
};
