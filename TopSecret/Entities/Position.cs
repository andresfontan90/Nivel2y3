
namespace TopSecretLibrary
{
    public class Position
    {
        private double _X;
        private double _Y;

        public double X
        {
            get{
                return _X;
            }
            set{
                _X = value;
            }
        }

        public double Y
        {
            get{
                return _Y;
            }
            set{
                _Y = value;
            }
        }

        public Position(double X, double Y)
        {
            _X = X;
            _Y = Y;
        }
    }
}
