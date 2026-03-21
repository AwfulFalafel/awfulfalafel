class Venue < ApplicationRecord
	self.implicit_order_column = [:name, nil]
	has_many(:schedule_entries, dependent: :restrict_with_exception)
end
