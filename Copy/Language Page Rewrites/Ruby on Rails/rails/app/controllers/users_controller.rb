class UsersController < ApplicationController
  def show
    # Find user by :id.
    @user = User.find_by(id: params[:id])
    # Check if user was found.
    if !@user
      # If not found, notify Airbrake of issue with associated hash params.
      notify_airbrake('User lookup not found.', {
          id: params[:id]
      })
    end
    Airbrake.create_deploy
  end
end
