const { v4: uuid } = require('uuid');
const { faker } = require('@faker-js/faker');
const dotenv = require('dotenv');
const fs = require('fs');

dotenv.config();

const knex = require('knex')({
  client: 'pg',
  connection: process.env.DATABASE_URL,
});

const PIX_KEYS = 1_000_000;
const ACCOUNTS = 1_000_000;

const ERASE_DATA = false;

async function run() {
  if (ERASE_DATA) {
    await knex('PaymentProviders').del();
    await knex('Users').del();
    await knex('Accounts').del();
    await knex('PixKeys').del();
    await knex('Payments').del();
  }
  const start = new Date();

  let accounts = await generateAccounts();
  console.log(accounts);
  accounts = await populate('Accounts', accounts);
  generateJson('./seed/accounts.json', accounts);

  let pixKeys = await generatePixKeys();
  pixKeys = await populate('PixKeys', pixKeys);
  generateJson('./seed/pixKeys.json', pixKeys);

  console.log('Closing DB connection...');
  await knex.destroy();

  const end = new Date();
  console.log('Done!');
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

async function generateAccounts() {
  console.log(`Generating ${ACCOUNTS} payment provider accounts...`);
  const accounts = [];

  const users = await knex.select('Id').table('Users');
  const psps = await knex.select('Id').table('PaymentProviders');
  console.log(psps);

  for (let i = 0; i < ACCOUNTS; i++) {
    const randomUserId = users[Math.floor(Math.random() * users.length)];
    const randomPspId = psps[Math.floor(Math.random() * psps.length)];
    accounts.push({
      Agency: i,
      Number: i,
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: randomPspId.Id,
      UserId: randomUserId.Id,
    });
  }
  return accounts;
}

async function generatePixKeys() {
  console.log(`Generating ${PIX_KEYS} pix keys...`);
  const pixKeys = [];
  const accounts = await knex
    .select('Id', 'PaymentProviderId')
    .table('Accounts');

  for (let i = 0; i < PIX_KEYS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];

    pixKeys.push({
      AccountId: account.Id,
      PaymentProviderId: account.PaymentProviderId,
      Type: 'Random',
      Value: faker.string.uuid().substring(0, 32),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return pixKeys;
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
