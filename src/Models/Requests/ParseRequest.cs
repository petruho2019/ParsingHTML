using System.Text.Json.Serialization;
using FluentValidation;


namespace Models.Requests;

public record ParseRequestModel
{
    public string Selector { get; set; }
    public string Attribute { get; set; }

    [JsonPropertyName("url_b64")]
    public string UrlB64 { get; set; }

    [JsonPropertyName("encrypted_text_bytes_b64")]
    public string EncryptedTextBytesB64 { get; set; }

    [JsonPropertyName("key_bytes_b64")]
    public string KeyBytesB64 { get; set; }

    [JsonPropertyName("page_b64")]
    public string PageB64 { get; set; }
}


public class ParseRequestModelValidator : AbstractValidator<ParseRequestModel>
{
    public ParseRequestModelValidator()
    {
        RuleFor(x => x.Selector).NotEmpty().WithMessage("selector is required.");
        RuleFor(x => x.Attribute).NotEmpty().WithMessage("attribute is required.");
        RuleFor(x => x.UrlB64).NotEmpty().WithMessage("url_b64 is required.");
        RuleFor(x => x.EncryptedTextBytesB64).NotEmpty().WithMessage("encrypted_text_bytes_b64 is required.");
        RuleFor(x => x.KeyBytesB64).NotEmpty().WithMessage("key_bytes_b64 is required.");
        RuleFor(x => x.PageB64).NotEmpty().WithMessage("page_b64 is required.");
    }
}