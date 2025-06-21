namespace AlimentsUsinages.Web.Models.DTOs
{
    // R‚ponses API standardis‚es
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
Commande ECHO activ‚e.
        public static ApiResponse<T> SuccessResult(T data, string message = "Op‚ration r‚ussie")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
Commande ECHO activ‚e.
        public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

    // Pour les op‚rations CRUD (‚quivalent de vos boutons)
    public class CrudResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? Id { get; set; }
        public object? Data { get; set; }
Commande ECHO activ‚e.
        public static CrudResultDto Success(string message, int? id = null, object? data = null)
        {
            return new CrudResultDto
            {
                Success = true,
                Message = message,
                Id = id,
                Data = data
            };
        }
Commande ECHO activ‚e.
        public static CrudResultDto Error(string message)
        {
            return new CrudResultDto
            {
                Success = false,
                Message = message
            };
        }
    }

    // Pour le chargement des ComboBox (‚quivalent LoadComboBox methods)
    public class ComboBoxDataDto
    {
        public IEnumerable<AlimentDto> Aliments { get; set; } = new List<AlimentDto>();
        public IEnumerable<OrigineFormuleDto> OrigineFormules { get; set; } = new List<OrigineFormuleDto>();
        public IEnumerable<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    }
}
