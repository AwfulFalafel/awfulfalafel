module Admin
	class CatalogEntriesController < BaseController
		before_action :find_catalog_entry, only: [:edit, :update]
		before_action :find_artists, only: [:new, :edit]
		def index
			@catalog_entries = CatalogEntry.eager_load(:artist).order(artist: { name: :asc })
		end

		def new
			@catalog_entry = CatalogEntry.new
		end

		def create
			update_catalog_entry()
			redirect_to(admin_catalog_entries_path)
		end

		def update
			update_catalog_entry(@catalog_entry)
			redirect_to(admin_catalog_entries_path)
		end

		private

		def update_catalog_entry(catalog_entry = CatalogEntry.new)
			catalog_entry.title = catalog_entry_params[:title]
			catalog_entry.artist = Artist.find_or_create_by(name: catalog_entry_params[:artist_name])
			catalog_entry.save
			catalog_entry
		end

		def catalog_entry_params
			params.expect(catalog_entry: [:artist_name, :title])
		end

		def find_catalog_entry
			@catalog_entry = CatalogEntry.find(params[:id])
		end

		def find_artists
			@artists = Artist.all
		end
	end
end
