module ApplicationHelper
  def is_menu_tab_active?(tab)
    case tab
      when :about
        return true if controller_name == 'about'
      when :book
        return true if controller_name == 'books'
      when :index
        return true if controller_name == 'index'
      when :contact
        return true if controller_name == 'contact'
      else
        false
    end
  end
end
