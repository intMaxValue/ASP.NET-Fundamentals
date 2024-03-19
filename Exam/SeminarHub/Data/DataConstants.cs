namespace SeminarHub.Data
{
    public static class DataConstants
    {
        // Seminar Data Constants
        public const int SeminarTopicMinLength = 3;
        public const int SeminarTopicMaxLength = 100;

        public const int SeminarLecturerMinLength = 5;
        public const int SeminarLecturerMaxLength = 60;

        public const int SeminarDetailsMinLength = 10;
        public const int SeminarDetailsMaxLength = 500;

        public const int SeminarDurationMinLength = 30;
        public const int SeminarDurationMaxLength = 180;

        //Category Data Constants
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;


        public const string DateTimeFormat = "MM/dd/yyyy HH:mm";

        public const string LengthErrorMessage = "The field {0} should be between {2} and {1} characters.";
    }
}
