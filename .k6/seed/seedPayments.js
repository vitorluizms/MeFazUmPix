const { v4: uuid } = require('uuid');
const { faker } = require('@faker-js/faker');
const dotenv = require('dotenv');
const fs = require('fs');

dotenv.config();

const knex = require('knex')({
  client: 'pg',
  connection: process.env.DATABASE_URL,
});

const MAX_PAYMENTS_AMOUNT = 300_000;

const PAYMENTS = 1_000_000;

const ERASE_DATA = true;

async function run() {
  if (ERASE_DATA) {
    await knex('Payments').del();
  }
  const start = new Date();
  const accounts = await knex.select('Id').table('Accounts');
  const pixKeys = await knex.select('Id').table('PixKeys');

  let payments = await generatePayments(accounts, pixKeys);
  payments = await populate('Payments', payments);
  generateJson('./seed/payments.json', payments);

  console.log('Closing DB connection...');
  await knex.destroy();

  const end = new Date();
  console.log('Done!');
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

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
