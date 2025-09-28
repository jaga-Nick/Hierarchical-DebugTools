using UnityEngine;



namespace HierarchicalDebugTools {

    public class LogGenerator : MonoBehaviour {

        public bool LogEnable;
        private Rigidbody rb;


    
        void Start() {

            if (LogEnable)
            {
                Debug.Log("Test log message for " + gameObject.name, gameObject);
                
                string message = "Very long error message for " + gameObject.name + " that exceeds the default width of the console window. This is to test how the console handles long messages and if it wraps them correctly or not. Let's see how it behaves with this long string. This is a test to ensure that the console can handle long messages without any issues. We want to make sure that the text is readable and doesn't overflow or cause any problems in the console window. This is important for debugging purposes and for maintaining a clean and organized console output.";
                Debug.LogError(message, gameObject);
                
                Debug.LogWarning("Test warning message for " + gameObject.name, gameObject);
                
                Debug.Assert(false, "Test assert message for " + gameObject.name, gameObject);
                
                Rigidbody rigidbody = null;
                rigidbody.AddForce(Vector3.one); // This will cause a NullReferenceException
            }
            else
            {
                Debug.Log("Test log message for " + gameObject.name);
                
                string message = "Very long error message for " + gameObject.name + " that exceeds the default width of the console window. This is to test how the console handles long messages and if it wraps them correctly or not. Let's see how it behaves with this long string. This is a test to ensure that the console can handle long messages without any issues. We want to make sure that the text is readable and doesn't overflow or cause any problems in the console window. This is important for debugging purposes and for maintaining a clean and organized console output.";
                Debug.LogError(message);
                
                Debug.LogWarning("Test warning message for " + gameObject.name);
                
                Debug.Assert(false, "Test assert message for " + gameObject.name);
                
                Rigidbody rigidbody = null;
                rigidbody.AddForce(Vector3.one); // This will cause a NullReferenceException
            }

        }


    }

}
