using System.Windows.Threading;
using Forms = System.Windows.Forms;


namespace InfiniteMouse.Model
{

    public class InfiniteMouseService
    {
        const byte FPS = 120;
        const byte DISTANCEREBORD = 10;

        private DispatcherTimer _Timer = new();

        private int _TopX = 0;
        private int _TopY =0;
        private int _DownX = 0;
        private int _DownY = 0;

        public bool Actif { get; set; } = false;
        public int PosX { get { return Cursor.Position.X; } }
        public int PosY { get { return Cursor.Position.Y; } }
        public Point Position { get { return Cursor.Position; } }
        public bool EstInitialiser { get; private set; }
        public Parametre Parametre { get; set; } = new();
        public List<Forms.Screen> LstScreen { get { return Forms.Screen.AllScreens.ToList(); } }


        public InfiniteMouseService()
        {
            _Timer.Tick += _Timer_Tick;
        }

        public void Reload() 
        {
            _Timer.Stop();
            Initialiser();

            if (Parametre.DemarrageActiver)
            {
                Activer();
            }
            else
            {
                Desactiver();
            }
        }
        public void Initialiser()
        {
            _Timer.Interval = TimeSpan.FromMilliseconds(1000.0 / FPS);
            EstInitialiser = true;

            //reset
            _TopX = 0;
            _TopY =0;
           _DownX = 0;
           _DownY = 0; 

            foreach (Forms.Screen screen in LstScreen)
            {
                if (screen.Bounds.Right > _TopX)
                {
                    _TopX = screen.Bounds.Right;
                }

                if (screen.Bounds.Bottom > _TopY)
                {
                    _TopY = screen.Bounds.Bottom;
                }

                if (screen.Bounds.Left < _DownX)
                {
                    _DownX = screen.Bounds.Left;
                }

                if (screen.Bounds.Top < _DownY)
                {
                    _DownY = screen.Bounds.Top;
                }
            }

        }

        
        public void Activer()
        {
            if (EstInitialiser)
            {
                _Timer.Start();
                Actif = true;
            }
            else
            {
                throw new Exception("Vous devez initialiser l'objet avant de commencer.");
            }

        }

        public void Desactiver()
        {
            if (EstInitialiser)
            {
                _Timer.Stop();
                Actif = false;
            }
            else
            {
                throw new Exception("Vous devez initialiser l'objet avant de commencer.");
            }
          
        }
        private void _Timer_Tick(object? sender, EventArgs e)
        {
            int X = PosX;
            int Y = PosY;
            int newX = X;
            int newY = Y;

            if (X >= _TopX - 1)
            {
                newX = _DownX + DISTANCEREBORD;
            }
            else if (X <= _DownX + 1)
            {
                newX = _TopX - DISTANCEREBORD;
            }

            if (Y >= _TopY - 1)
            {
                newY = _DownY + DISTANCEREBORD;
            }
            else if (Y <= _DownY + 1)
            {
                newY = _TopY - DISTANCEREBORD;
            }

            if (newX != X || newY != Y)
            {
                Forms.Cursor.Position = new Point(newX, newY);
            }

        }
    }
}
