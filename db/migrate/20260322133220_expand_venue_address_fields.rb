class ExpandVenueAddressFields < ActiveRecord::Migration[8.1]
	def change
		change_table(:venue) do |t|
			t.string(:city)
			t.string(:state)
			t.string(:postal_code)
			t.string(:social_link)
		end
	end
end
