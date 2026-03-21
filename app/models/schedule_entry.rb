class ScheduleEntry < ApplicationRecord
	self.implicit_order_column = [:date, nil]
	belongs_to(:venue)
end
