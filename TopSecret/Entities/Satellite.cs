using Google.Cloud.Firestore;
namespace TopSecretLibrary
{
    [FirestoreData]
    public class Satellite
    {
        public string name {get; set;}

        [FirestoreProperty]
        public double distance { get; set; } 

        [FirestoreProperty]
        public string[] message { get; set; }
    }
}
