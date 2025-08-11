// Stdlib
import process from "node:process";
import { randomUUID } from "node:crypto";
import { join, dirname, basename, resolve } from "node:path";
import { fileURLToPath } from "node:url";

// Packages
import "dotenv/config";
import express from "express";
import morgan from "morgan";
import cors from "cors";

// Application code
import site from "./_data/site.json" with { type: "json" };
import { DatabaseService } from "./services/database-service.js";

function main() {
	const port = process.env.PORT;
	const root = dirname(fileURLToPath(import.meta.url));

	const app = express();
	app.use(express.json());
	app.set("views", join(root, "views"));
	app.set("view engine", "ejs");
	app.use(cors({
		origin: ["http://localhost:9093"],
		methods: ["GET"],
	}));
	app.locals.site = site;
	const databaseService = new DatabaseService();

	app.use(cors());
	app.use((_req, res, next) => {
		res.setHeader("X-Request-Id", randomUUID());
		res.setHeader("X-Powered-By", "Your mother");
		next();
	});
	app.use(morgan("combined"));

	app.use("/assets", express.static(join(root, "assets")));
	app.get("/", (_req, res) => {
		res.render("app", { title: null, page: "landing" });
	});
	app.get("/about", (_req, res) => {
		res.render("app", { title: site.about.title, page: "about" });
	});
	app.get("/songs", (_req, res) => {
		const catalog = databaseService.getCatalog();
		res.render("app", { title: site.songs.title, page: "songs", catalog });
	});
	app.get("/schedule", (_req, res) => {
		const shows = databaseService.getShows();
		res.render("app", { title: site.schedule.title, page: "schedule", shows });
	});
	app.get("/contact", (_req, res) => {
		res.render("app", { title: site.contact.title, page: "contact" });
	});

	// Catch-all 404 page
	app.all(/(.*)/, (_req, res) => {
		res.status(404);
		res.json({ "error": "Not found" });
	});
	const server = app.listen(port, () => {
		console.log(`App listening on port ${port}.`);
	});

	const close = () => {
		server.close(() => {
			console.log("Goodbye!");
		});
	};
	process.on("SIGUSR2", close);
	process.on("SIGHUP", close);
	process.on("SIGTERM", close);
	process.on("SIGINT", close);
}
main();