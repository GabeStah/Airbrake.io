class HardWorker
  include Sidekiq::Worker

  def perform(*args)
    1/0
  end
end
