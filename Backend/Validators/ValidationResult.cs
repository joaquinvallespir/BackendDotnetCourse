namespace Backend.Validators
{
    public class ValidationResult
    {

        public bool IsValid { get; set; }
        public ValidationErrorType ErrorType { get; set; }
        public string ErrorMessage { get; set; }

    }
    public enum ValidationErrorType
    {
        NotFound,
        InvalidQuantity,
        Duplicate
        // Otros tipos de error, si los hay
    }

}
