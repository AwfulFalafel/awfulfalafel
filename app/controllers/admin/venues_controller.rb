module Admin
	class VenuesController < BaseController
		before_action :find_venue, only: [:edit, :update]

		def index
			@venues = Venue.order(name: :asc)
		end
		def new
			@venue = Venue.new
		end

		def create
			Venue.create(venue_params)
			redirect_to(admin_venues_path)
		end

		def update
			if @venue.update(venue_params)
				redirect_to(admin_venues_path)
			else
				render
			end
		end

		private

		def find_venue
			@venue = Venue.find(params[:id])
		end

		def venue_params
			params.expect(venue: [:name, :address, :city, :state, :postal_code, :social_link])
		end
	end
end
