module Constraints
	class Local
		def self.matches?(req)
			Rails.env.development? || req.ip.match?(/192\.168\.\d+\.\d+/)
		end
	end
end
