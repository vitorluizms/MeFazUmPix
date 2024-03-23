const { v4: uuid } = require('uuid');
const { faker } = require('@faker-js/faker');
const dotenv = require('dotenv');
const fs = require('fs');

dotenv.config();

const knex = require('knex')({
  client: 'pg',
  connection: process.env.DATABASE_URL,
});

const PSP = 1_000_000;

const ERASE_DATA = false;

async function run() {
  if (ERASE_DATA) {
    await knex('PaymentProviders').del();
  }
  const start = new Date();

  let paymentProviders = generatePaymentProviders();
  paymentProviders = await populate('PaymentProviders', paymentProviders);
  generateJson('./seed/paymentProviders.json', paymentProviders);

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
      Token: faker.string.uuid().substring(0, 32),
      Name: faker.company.name(),
      Webhook: 'http://localhost:5039/payments/pix',
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return paymentProviders;
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
  