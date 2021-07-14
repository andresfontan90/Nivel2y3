
using TopSecretLibrary;

namespace TopSecret
{
    public class Response
    {
        private Position _position;
        private string _message;

        public Position position
        {
            get{
                return _position;
            }
            set{
                _position = value;
            }
        }

        public string message
        {
            get{
                return _message;
            }
            set{
                _message = value;
            }
        }
    }
}
