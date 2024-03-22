import http from 'k6/http';
import { sleep } from 'k6';
import { SharedArray } from 'k6/data';

export const options = {
  vus: 120, // virtual users
  duration: '60s', // duration of the test in seconds
};

const PSP_TOKEN =
  'VPZxeLCk9vxZ5bOqtzJduCJARPLH1ruyrI89GY0RCdJ6cHvzJ4FlAHsSG85Wmy9i';
const MAX_PAYMENT_AMOUNT = 300000;

const keysData = new SharedArray('pixKeys', () => {
  const result = JSON.parse(open('../seed/pixKeys.json'));
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
  const randomPixKey = keysData[Math.floor(Math.random() * keysData.length)];

  // const randomAccount =
  //   accountsData[Math.floor(Math.random() * accountsData.length)];

  // const userByAccount = usersData.find(u => u.Id === randomAccount.UserId);

  const body = {
    user: {
      cpf: '57795',
    },
    account: {
      number: 56204701,
      agency: 10075231,
    },
    key: {
      value: randomPixKey.Value,
      type: randomPixKey.Type,
    },
    amount: Math.floor(Math.random() * MAX_PAYMENT_AMOUNT),
    description: `${new Date(Date.now()).toISOString()}`.slice(-20),
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

  const response = http.post('http://localhost:8080/payments', bodyString, {
    headers,
  });

  if (response.status != 201) {
    console.log(response.body);
  }
};
