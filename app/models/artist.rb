class Artist < ApplicationRecord
	self.implicit_order_column = [:name, nil]
	has_many(:catalog_entries, dependent: :delete_all)
end
