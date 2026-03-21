class Gig < ApplicationRecord
	self.implicit_order_column = [:date, nil]
	scope(:upcoming, -> { where("date > ?", Time.now.end_of_day).order(date: :asc) })
	scope(:past, -> { where("date < ?", Time.now.beginning_of_day).order(date: :desc) })

	def readonly?
		true
	end
end
