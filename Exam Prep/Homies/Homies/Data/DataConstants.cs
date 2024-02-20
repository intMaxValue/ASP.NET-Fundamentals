namespace Homies.Data
{
    public static class DataConstants
    {
        // Event
        public const int EventNameMinLength = 5;
        public const int EventNameMaxLength = 20;

        public const int EventDescriptionMinLength = 15;
        public const int EventDescriptionMaxLength = 150;

        public const string DateTimeFormat = "yyyy-MM-dd H:mm";

        public const string LengthErrorMessage = "The field {0} must be between {2} and {1} characters.";
        public const string RequiredErrorMessage = "The field {0} is required.";

        // Type
        public const int TypeNameMinLength = 5;
        public const int TypeNameMaxLength = 15;
    }
}
