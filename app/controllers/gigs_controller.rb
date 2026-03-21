class GigsController < ApplicationController
	def index
		@upcoming = Gig.upcoming
		@past = Gig.past
	end

	def show
	end
end
