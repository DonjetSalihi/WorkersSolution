namespace WorkersSolution.Constants
{
    public static class AppConfig
    {
        public static string WorkersApiBaseUrl = "https://dev-api-1.sitedocs.com/api/v1/";

        //We can store the authorization token in secure storage from xamarin essentials,
        //but since in this project we are not retrieving it from login or somewhere, I didn't store it.
        public static string WorkersApiAuthorizationToken = "Token 44798935-c223-47e6-b0eb-84df6c6210c7";
    }
}
