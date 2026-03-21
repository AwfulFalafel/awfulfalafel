# TODO: Remove this when https://github.com/rails/rails/pull/56842 is merged,
# and the Rails team stop being fucking dumbasses.
module CoreExtensions
	module ActiveRecord
		module SchemaDumper
			def self.included(other)
				other.class_eval do
					def table(table, stream)
						columns = @connection.columns(table)
						begin
							self.table_name = table

							tbl = StringIO.new

							# first dump primary key column
							pk = @connection.primary_key(table)

							tbl.print "  create_table #{relation_name(remove_prefix_and_suffix(table)).inspect}"

							case pk
							when String
								tbl.print ", primary_key: #{pk.inspect}" unless pk == "id"
								pkcol = columns.detect { |c| c.name == pk }
								pkcolspec = column_spec_for_primary_key(pkcol)
								unless pkcolspec.empty?
									if pkcolspec != pkcolspec.slice(:id, :default)
										pkcolspec = { id: { type: pkcolspec.delete(:id), **pkcolspec }.compact }
									end
									tbl.print ", #{format_colspec(pkcolspec)}"
								end
							when Array
								tbl.print ", primary_key: #{pk.inspect}"
							else
								tbl.print ", id: false"
							end

							table_options = @connection.table_options(table)
							if table_options.present?
								tbl.print ", #{format_options(table_options)}"
							end

							tbl.puts ", force: :cascade do |t|"

							# then dump all non-primary key columns
							columns.each do |column| # One single fucking line change
								raise StandardError, "Unknown type '#{column.sql_type}' for column '#{column.name}'" unless @connection.valid_type?(column.type)
								next if column.name == pk

								type, colspec = column_spec(column)
								if type.is_a?(Symbol)
									tbl.print "    t.#{type} #{column.name.inspect}"
								else
									tbl.print "    t.column #{column.name.inspect}, #{type.inspect}"
								end
								tbl.print ", #{format_colspec(colspec)}" if colspec.present?
								tbl.puts
							end

							indexes_in_create(table, tbl)
							remaining = check_constraints_in_create(table, tbl) if @connection.supports_check_constraints?
							exclusion_constraints_in_create(table, tbl) if @connection.supports_exclusion_constraints?
							unique_constraints_in_create(table, tbl) if @connection.supports_unique_constraints?

							tbl.puts "  end"

							if remaining
								tbl.puts
								tbl.print remaining.string
							end

							stream.print tbl.string
						rescue => e
							stream.puts "# Could not dump table #{table.inspect} because of following #{e.class}"
							stream.puts "#   #{e.message}"
							stream.puts
						ensure
							self.table_name = nil
						end
					end
				end
			end
		end
	end
end

ActiveSupport.on_load(:active_record) do
	ActiveRecord::SchemaDumper.include(CoreExtensions::ActiveRecord::SchemaDumper)
end
