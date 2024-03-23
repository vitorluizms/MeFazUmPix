const { v4: uuid } = require('uuid');
const { faker } = require('@faker-js/faker');
const dotenv = require('dotenv');
const fs = require('fs');

dotenv.config();

const knex = require('knex')({
  client: 'pg',
  connection: process.env.DATABASE_URL,
});

const USERS = 1_000_000;

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

  let users = generateUsers();
  users = await populate('Users', users);
  generateJson('./seed/users.json', users);

  console.log('Closing DB connection...');
  await knex.destroy();

  const end = new Date();
  console.log('Done!');
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

function generateUsers() {
  console.log(`Generating ${USERS} users...`);
  const users = [];
  for (let i = 0; i < USERS; i++) {
    users.push({
      Name: `${Date.now() + i}`,
      CPF: (1 * 10 ** 10 + i).toString(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }

  return users;
}

run();

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
