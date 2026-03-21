class Initial < ActiveRecord::Migration[8.1]
	def change
		create_table(:venue, id: :uuid) do |t|
			t.string(:name, null: false)
			t.string(:address)
		end

		create_table(:schedule_entry, id: :uuid) do |t|
			t.references(:venue, type: :uuid, foreign_key: true)
			t.datetime(:date, null: false)
			t.string(:name)
			t.string(:social_link)
		end

		create_table(:artist, id: :uuid) do |t|
			t.string(:name, null: false)
		end

		create_table(:catalog_entry, id: :uuid) do |t|
			t.references(:artist, type: :uuid, foreign_key: true)
			t.string(:title, null: false)
		end

		create_view(:song)
		create_view(:gig)
	end
end
