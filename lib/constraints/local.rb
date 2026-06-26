module Constraints
	class Local
		def self.matches?(req)
			Rails.env.development? || req.ip.match?(/192\.168\.\d+\.\d+/) || req.ip.match?("127.0.0.1")
		end
	end
end
