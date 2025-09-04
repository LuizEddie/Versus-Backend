namespace Versus.Models.TemplateModels
{
    public class NewPasswordTemplateModel
    {
        public string UserName { get; set; } = string.Empty;
        public string ResetLink { get; set; } = string.Empty;
    }
}
