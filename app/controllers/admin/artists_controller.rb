module Admin
	class ArtistsController < BaseController
		before_action :find_artist, only: [:edit, :update, :destroy]

		def index
			@artists = Artist.order(name: :asc)
		end
		def new
			@artist = Artist.new
		end

		def create
			Artist.create(artist_params)
			redirect_to(admin_artists_path)
		end

		def update
			if @artist.update(artist_params)
				redirect_to(admin_artists_path)
			else
				render
			end
		end

		def destroy
			@artist.destroy
			redirect_to(admin_artists_path)
		end


		private

		def find_artist
			@artist = Artist.find(params[:id])
		end

		def artist_params
			params.expect(artist: [:name])
		end
	end
end
