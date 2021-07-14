
using Google.Cloud.Firestore;

namespace TopSecret
{
    [FirestoreData]
    public class SatelliteSplit
    {
        [FirestoreProperty]
        public double distance { get; set; }

        [FirestoreProperty]
        public string[] message { get; set; }

    }
}
