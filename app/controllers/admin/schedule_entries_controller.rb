module Admin
	class ScheduleEntriesController < BaseController
		before_action :find_schedule_entry, only: [:edit, :update]
		before_action :find_venues, only: [:new, :edit]
		def index
			@schedule_entries = ScheduleEntry.eager_load(:venue).order(date: :desc)
		end

		def new
			@schedule_entry = ScheduleEntry.new
		end

		def create
			ScheduleEntry.create(schedule_entry_params)
			redirect_to(admin_schedule_entries_path)
		end

		def update
			@schedule_entry.update(schedule_entry_params)
			redirect_to(admin_schedule_entries_path)
		end

		private

		def schedule_entry_params
			params.expect(schedule_entry: [:venue_id, :date, :name, :social_link])
		end

		def find_schedule_entry
			@schedule_entry = ScheduleEntry.find(params[:id])
		end

		def find_venues
			@venues = Venue.all
		end
	end
end
