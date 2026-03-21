class CatalogEntry < ApplicationRecord
	self.implicit_order_column = [:title, nil]
	belongs_to(:artist)
	delegate(:name, to: :artist, prefix: true, allow_nil: true)
end
