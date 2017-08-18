class AddLibraryToBook < ActiveRecord::Migration[5.1]
  def change
    add_column :books, :library, :string
  end
end
