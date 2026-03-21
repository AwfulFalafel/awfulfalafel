# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# This file is the source Rails uses to define your schema when running `bin/rails
# db:schema:load`. When creating a new database, `bin/rails db:schema:load` tends to
# be faster and is potentially less error prone than running all of your
# migrations from scratch. Old migrations may fail to apply correctly if those
# migrations use external dependencies or application code.
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema[8.1].define(version: 2026_03_18_191956) do
  # These are extensions that must be enabled in order to support this database
  enable_extension "pg_catalog.plpgsql"

  create_table "artist", id: :uuid, default: -> { "gen_random_uuid()" }, force: :cascade do |t|
    t.string "name", null: false
  end

  create_table "catalog_entry", id: :uuid, default: -> { "gen_random_uuid()" }, force: :cascade do |t|
    t.uuid "artist_id"
    t.string "title", null: false
    t.index ["artist_id"], name: "index_catalog_entry_on_artist_id"
  end

  create_table "schedule_entry", id: :uuid, default: -> { "gen_random_uuid()" }, force: :cascade do |t|
    t.uuid "venue_id"
    t.datetime "date", null: false
    t.string "name"
    t.string "social_link"
    t.index ["venue_id"], name: "index_schedule_entry_on_venue_id"
  end

  create_table "venue", id: :uuid, default: -> { "gen_random_uuid()" }, force: :cascade do |t|
    t.string "name", null: false
    t.string "address"
  end

  add_foreign_key "catalog_entry", "artist"
  add_foreign_key "schedule_entry", "venue"

  create_view "gig", sql_definition: <<-SQL
      SELECT c.date,
      c.social_link,
      venue.name AS venue_name,
      venue.address AS location
     FROM (schedule_entry c
       JOIN venue ON ((c.venue_id = venue.id)))
    ORDER BY c.date DESC;
  SQL
  create_view "song", sql_definition: <<-SQL
      SELECT a.name AS artist,
      catalog_entry.title
     FROM (catalog_entry
       JOIN artist a ON ((catalog_entry.artist_id = a.id)))
    ORDER BY a.name, catalog_entry.title;
  SQL
end
