
namespace ConsultasTSC
{
    public class SystemParameters
    {
        //private static readonly string BasePath = "Resources\\Images";
        private static readonly string BasePath = "C:\\Users\\jhmedina\\Documents\\24\\ConsultasTSC\\Resources\\Images";
        //private static readonly string BasePath = "C:\\Users\\jhmedina\\Documents\\jm\\Proyectos 23\\Go_Files-main\\GO_FILES_UTL\\Resources\\Images";
        private static readonly string XHRUserName = "user";  
        private static readonly string XHRPassword = "password";
        private static readonly bool IsRelative = false;
        private static readonly string AWSAccessKey = "***********";
        private static readonly string AwsSecretAccessKey = "***********";
        private static readonly string Region = "**-****-*";
        private static readonly string BucketName = "****";

        public static string AppBasePath { get; } = BasePath;
        public static bool FolderIsRelative { get; } = IsRelative;
        public static string AppUserName { get; } = XHRUserName;
        public static string AppPassword { get; } = XHRPassword;
        public static string AppAwsAccessKey { get; } = AWSAccessKey;
        public static string AppAwsSecretAccessKey { get; } = AwsSecretAccessKey;
        public static string AppRegion { get; } = Region;
        public static string AppBucketName { get; } = BucketName; 
    }
}
