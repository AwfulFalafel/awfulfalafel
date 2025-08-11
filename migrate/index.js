import { readdirSync, readFileSync } from "fs";
import { backup, DatabaseSync } from "node:sqlite";
import { dirname, basename, join } from "path/posix";
import { fileURLToPath } from "url";
import "dotenv/config";

async function main() {
	const __filename = fileURLToPath(import.meta.url);
	const __dirname = dirname(__filename);
	const migrations = readdirSync(join(__dirname, "migrations"));
	const database = new DatabaseSync(":memory:", { allowExtension: true });
	database.exec(`PRAGMA foreign_keys = ON`);
	for (const migration of migrations) {
		const sql = readFileSync(join(__dirname, "migrations", migration), { encoding: "utf8" });
		database.exec(sql);
	}
	await backup(database, "main.db");
}
main();