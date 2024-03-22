const { v4: uuid } = require('uuid');
const { faker } = require('@faker-js/faker');
const dotenv = require('dotenv');
const fs = require('fs');

dotenv.config();

const knex = require('knex')({
  client: 'pg',
  connection: process.env.DATABASE_URL,
});

const TOKEN_PROVIDER =
  'VPZxeLCk9vxZ5bOqtzJduCJARPLH1ruyrI89GY0RCdJ6cHvzJ4FlAHsSG85Wmy9i';
const MAX_PAYMENTS_AMOUNT = 300_000;

const USERS = 1_000_000;
const PIX_KEYS = 1_000_000;
const PAYMENTS = 1_000_000;
const PSP = 1_000_000;
const ACCOUNTS = 1_000_000;

const ERASE_DATA = true;

async function run() {
  if (ERASE_DATA) {
    await knex('PaymentProviders').del();
    await knex('Users').del();
    await knex('Accounts').del();
    await knex('PixKeys').del();
    await knex('Payments').del();
  }
  const start = new Date();

  let paymentProviders = generatePaymentProviders();
  paymentProviders = await populate('PaymentProviders', paymentProviders);
  generateJson('./seed/paymentProviders.json', paymentProviders);

  let users = generateUsers();
  users = await populate('Users', users);
  generateJson('./seed/users.json', users);

  let accounts = generateAccounts(paymentProviders, users);
  accounts = await populate('Accounts', accounts);
  generateJson('./seed/accounts.json', accounts);

  let pixKeys = await generatePixKeys(accounts, paymentProviders);
  pixKeys = await populate('PixKeys', pixKeys);
  generateJson('./seed/pixKeys.json', pixKeys);

//   let payments = await generatePayments(accounts, pixKeys);
//   payments = await populate('Payments', payments);
//   generateJson('./seed/payments.json', payments);

  console.log('Closing DB connection...');
  await knex.destroy();

  const end = new Date();
  console.log('Done!');
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

function generatePaymentProviders() {
  console.log(`Generating ${PSP} payment providers...`);
  const paymentProviders = [];

  for (let i = 0; i < PSP; i++) {
    paymentProviders.push({
      Token: TOKEN_PROVIDER,
      Name: faker.company.name(),
      Webhook: 'http://localhost:5039/payments/pix',
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return paymentProviders;
}

function generateUsers() {
  console.log(`Generating ${USERS} users...`);
  const users = [];

  for (let i = 0; i < USERS; i++) {
    users.push({
      CPF: i.toString(),
      Name: faker.person.firstName(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return users;
}

function generateAccounts(paymentProviders, users) {
  console.log(`Generating ${ACCOUNTS} payment provider accounts...`);
  const accounts = [];

  for (let i = 0; i < ACCOUNTS; i++) {
    const paymentProvider =
      paymentProviders[Math.floor(Math.random() * paymentProviders.length)];
    const user = users[Math.floor(Math.random() * users.length)];

    accounts.push({
      Agency: i,
      Number: i,
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider.Id,
      UserId: user.Id,
    });
  }
  return accounts;
}

async function generatePixKeys(accounts, psps) {
  console.log(`Generating ${PIX_KEYS} pix keys...`);
  const pixKeys = [];

  for (let i = 0; i < PIX_KEYS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];
    const psp = psps[Math.floor(Math.random() * psps.length)];

    pixKeys.push({
      AccountId: account.Id,
      PaymentProviderId: psp.Id,
      Type: 'Random',
      Value: faker.string.uuid().substring(0, 32),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return pixKeys;
}

async function generatePayments(accounts, pixKeys) {
  console.log(`Generating ${PAYMENTS} payments...`);
  const payments = [];

  for (let i = 0; i < PAYMENTS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];
    const pixKey = pixKeys[Math.floor(Math.random() * pixKeys.length)];

    payments.push({
      PixKeyId: pixKey.Id,
      PaymentProviderAccountId: account.Id,
      Status: 'SUCCESS',
      Amount: faker.number.int({ min: 1, max: MAX_PAYMENTS_AMOUNT }),
      Description: faker.lorem.sentence(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return payments;
}

async function populate(tableName, entities) {
  console.log(`Storing ${tableName} on DB...`);
  return knex.batchInsert(tableName, entities).returning('*');
}

const generateJson = (filepath, data) => {
  if (fs.existsSync(filepath)) {
    fs.unlinkSync(filepath);
  }

  fs.writeFileSync(filepath, JSON.stringify(data));
};
