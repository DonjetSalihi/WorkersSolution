namespace WorkersSolution.Constants
{
    public static class AppConfig
    {
        public static string WorkersApiBaseUrl = ""; //Please add the base url

        //We can store the authorization token in secure storage from xamarin essentials,
        //but since in this project we are not retrieving it from login or somewhere, I didn't store it.
        public static string WorkersApiAuthorizationToken = ""; // Please add the authorization token value
    }
}
