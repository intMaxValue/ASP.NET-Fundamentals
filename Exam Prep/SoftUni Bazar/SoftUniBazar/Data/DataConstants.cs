namespace SoftUniBazar.Data
{
    public static class DataConstants
    {
        // Ad Data Constants
        public const int AdNameMinLength = 5;
        public const int AdNameMaxLength = 25;

        public const int AdDescriptionMinLength = 15;
        public const int AdDescriptionMaxLength = 250;

        // Category Data Constants
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 15;


        // Error Messages
        public const string LengthErrorMessage = "The field {0} must be between {2} and {1} characters long.";

        // DateTime Format
        public const string DateTimeFormat = "yyyy-MM-dd H:mm";
    }
}
