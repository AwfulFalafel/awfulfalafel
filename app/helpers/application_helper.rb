module ApplicationHelper
	def page_title
		["Awful Falafel", content_for(:title)].compact.join(" | ")
	end

	def local_request?
		Constraints::Local.matches?(request)
	end
end
