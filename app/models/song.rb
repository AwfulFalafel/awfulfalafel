class Song < ApplicationRecord
	self.implicit_order_column = [:name, nil]

	def full_title
		"#{artist} - #{title}"
	end
end
