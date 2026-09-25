namespace ParsingHTML.Models.DTO;



public record ResponseDto
{
    public int IsError { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public int ElementsCount { get; set; }
    public int EmailCount { get; set; }
    public string Url { get; set; }
    public string DecryptedPlainText { get; set; }
    public IEnumerable<string> ElementsAttrList { get; set; }
    public IEnumerable<string> EmailsList { get; set; }
}