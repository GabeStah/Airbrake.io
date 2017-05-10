using Utility;

namespace Adapter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create new XBox console.
            var xbox = new XBox();
            // Play with default controller.
            xbox.Play();
            // Create new PlayStationController.
            var playStationController = new PlayStationController();
            // Create controller adapter for PlayStation controller.
            var adapter = new ControllerAdapter(playStationController);
            // Play with adapted controller.
            xbox.Play(adapter);

            Logging.Log("-----------------");

            // Create new PlayStation console.
            var playstation = new PlayStation();
            // Play with default controller.
            playstation.Play();
            // Create new XBoxController.
            var xboxController = new XBoxController();
            // Create controller adapter for XBox controller.
            var adapter2 = new ControllerAdapter(xboxController);
            // Play with adapted controller.
            playstation.Play(adapter2);
        }
    }

    interface IController { }

    class Controller : IController { }

    class PlayStationController : Controller { }

    class XBoxController : Controller { }

    /// <summary>
    /// Used to adapt incompatible controllers within console calls.
    /// </summary>
    class ControllerAdapter
    {
        public Controller Controller { get; set; }

        public ControllerAdapter(Controller controller)
        {
            // Assign controller to adapter.
            Controller = controller;
            Logging.Log($"Using adapter on {controller.GetType().Name}.");
        }
    }

    interface IConsole
    {
        Controller Controller { get; set; }
        void Play();
    }

    class Console : IConsole
    {
        private Controller _Controller;

        /// <summary>
        /// Controller field with custom set method to output controller being activated.
        /// </summary>
        public Controller Controller
        {
            get { return _Controller; }
            set
            {
                _Controller = value;
                Logging.Log($"Plugging {Controller.GetType().Name} into {this.GetType().Name} console.");
            }
        }

        public Console() { }

        /// <summary>
        /// Invoke Play call using default controller.
        /// </summary>
        public void Play()
        {
            Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
        }

        /// <summary>
        /// Invoke Play with associated adapter controller.
        /// </summary>
        /// <param name="adapter"></param>
        public void Play(ControllerAdapter adapter)
        {
            Controller = adapter.Controller;
            Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
        }
    }

    /// <summary>
    /// Basic XBox console.
    /// </summary>
    class XBox : Console
    {
        public XBox() {
            // Associate new XBoxController as default.
            Controller = new XBoxController();
        }
    }

    /// <summary>
    /// Basic PlayStation console.
    /// </summary>
    class PlayStation : Console
    {
        public PlayStation()
        {
            // Associate new PlayStationController as default.
            Controller = new PlayStationController();
        }
    }

}
