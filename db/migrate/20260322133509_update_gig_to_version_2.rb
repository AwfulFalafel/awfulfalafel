class UpdateGigToVersion2 < ActiveRecord::Migration[8.1]
	def change
		update_view(:gig, version: 2, revert_to_version: 1)
	end
end
