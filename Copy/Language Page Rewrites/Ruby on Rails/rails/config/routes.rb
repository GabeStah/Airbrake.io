Rails.application.routes.draw do
  get 'user/:id', to: 'users#show'
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html
  root 'users#new'
end
