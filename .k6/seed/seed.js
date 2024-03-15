const dotenv = require("dotenv")
const fake = require("@faker-js/faker")
const { faker } = fake
const { v4: uuidv4 } = require("uuid")
const fs = require("fs")
dotenv.config()

const knex = require("knex")({
    client: "pg",
    connection: process.env.DATABASE_URL
})

const USERS = 50000;
const VALID_PSP_TOKEN = "VPZxeLCk9vxZ5bOqtzJduCJARPLH1ruyrI89GY0RCdJ6cHvzJ4FlAHsSG85Wmy9i";
const ERASE_DATA = true;

const run = async () => {
    if (ERASE_DATA) {
        await knex("Users").del()
    }
    const start = new Date();

    const users = generateUsers();
    await populateUsersDatabase(users)
    generateJson("./seed/users.json", users)

    console.log("Closing DB connection...")
    await knex.destroy();

    const end = new Date();
    console.log("Done!")
    console.log(`Finished in ${(end - start) / 1000} seconds`)
}

const generateUsers = () => {
    console.log(`Generating ${USERS} users...`)

    const users = []
    for (let i = 0; i < USERS; i++) {
        const cpf = generateRandomCPFString()
        users.push({
            Name: faker.person.firstName(),
            CPF: cpf,
        })
    }
    console.log(users[0].CPF)
    return users
}

const generateRandomCPFString = () => {
    const randomDigit = () => Math.floor(Math.random() * 10);

    const cpfDigits = Array.from({ length: 9 }, randomDigit);
    
    const cpfString = cpfDigits.join('').replace(/(\d{3})(\d{3})(\d{3})/, '$1.$2.$3-') +
                        randomDigit() + randomDigit();
    
    return cpfString;

}

const populateUsersDatabase = async (users) => {
    console.log("Storing on DB ...")

    const tableName = "Users"
    await knex.batchInsert(tableName, users)
}

run()

const generateJson = (filepath, data) => {
    if(fs.existsSync(filepath)) {
        fs.unlinkSync(filepath);
    }

    fs.writeFileSync(filepath, JSON.stringify(data));
}