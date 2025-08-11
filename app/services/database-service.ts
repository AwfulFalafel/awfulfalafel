import { DatabaseSync } from "node:sqlite";
export class DatabaseService {
	getShows() {
		const database = new DatabaseSync("main.db");
		const selectUpcoming = database.prepare(`
			SELECT
				datetime(date) as date
				,	name
				,	event_link as eventLink
				,	location
				,	venue
				FROM
					show
				WHERE
					datetime(date) >= CURRENT_TIMESTAMP
				ORDER BY
					datetime(date) ASC
		`);
		const upcoming = selectUpcoming.all();
		const selectPast = database.prepare(`
			SELECT
				datetime(date) as date
				,	name
				,	event_link as eventLink
				,	location
				,	venue
				FROM
					show
				WHERE
					datetime(date) < CURRENT_TIMESTAMP
				ORDER BY
					datetime(date) DESC
		`);
		const past = selectPast.all();
		database.close();
		return {
			upcoming,
			past
		};
	}
	getCatalog() {
		const database = new DatabaseSync("main.db");
		const catalog = database.prepare(`SELECT * FROM catalog ORDER BY "artist" ASC, "title" ASC`).all();
		database.close();
		return catalog;
	}
}
