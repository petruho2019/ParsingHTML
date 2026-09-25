using System.Security.Cryptography;
using System.Text;
using AngleSharp.Dom;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using ParsingHTML.Entities;
using ParsingHTML.Models.DTO;
using ParsingHTML.Services.Abstractions;

namespace ParsingHTML.Controllers;

[ApiController]
public class MainController(IValidator<ParseRequestModel> validator, IHtmlElementsService htmlService, IElementsRepository elementsRepository) : ControllerBase
{

    [HttpGet("index")]
    public IActionResult Index()
    {
        return Ok("Pong");
    }

    [HttpPost("parse")]
    public async Task<IActionResult> Parse([FromBody] ParseRequestModel request)
    {
        if (request is null)
        {
            throw new ValidationException("The request body cannot be null");
        }
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var decodedPrivateKey = Convert.FromBase64String(request.KeyBytesB64);
        var decodedPlainText = Convert.FromBase64String(request.EncryptedTextBytesB64);

        var decryptedText = DecryptPageText(decodedPrivateKey, decodedPlainText);

        var decodedHtml = Encoding.UTF8.GetString(Convert.FromBase64String(request.PageB64));
        var encodedUrl = Encoding.UTF8.GetString(Convert.FromBase64String(request.UrlB64));

        var elements = await GetElementsByCssSelectorFromHtml(decodedHtml, request.Selector);

        var htmlElements = elements.Select(e => new HtmlElement()
        {
            AttributeValue = e.Attributes[request.Attribute]!.Value,
            HtmlValue = e.TextContent
        });

        var affected = await AddElementsToDb(htmlElements);

        var emails = htmlService.GetEmailsFromHtml(decodedHtml);

        var count = emails.Count();

        ShowToConsoleDecryptedText(request);

        var response = new ResponseDto
        {
            IsError = 0,
            ErrorCode = string.Empty,
            ErrorMessage = string.Empty,
            ElementsCount = elements.Count,
            EmailCount = count,
            Url = encodedUrl,
            DecryptedPlainText = decryptedText,
            ElementsAttrList = htmlElements.Select(he => he.AttributeValue).ToList(),
            EmailsList = emails.ToList()
        };

        return Ok(response);
    }

    private async Task<int> AddElementsToDb(IEnumerable<HtmlElement> htmlElements)
    {
        return await elementsRepository.AddElementsAsync(htmlElements);
    }

    private async Task<IHtmlCollection<IElement>> GetElementsByCssSelectorFromHtml(string decryptedHtml, string selector)
    {
        var elements = htmlService.GetElementsByCssSelectorFromHtml(selector, decryptedHtml);

        return elements;
    }


    private static void ShowToConsoleDecryptedText(ParseRequestModel request)
    {
        var key = Convert.FromBase64String(request.KeyBytesB64);
        var text = Convert.FromBase64String(request.EncryptedTextBytesB64);

        Console.WriteLine($"Decrypted text: {DecryptPageText(key, text)}");
    }

    private static string DecryptPageText(
    byte[] secretKeyBytes,
    byte[] encryptedTextBytes)
    {
        using var aes = Aes.Create();

        aes.Key = secretKeyBytes!;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;

        var decrypted = aes.DecryptEcb(encryptedTextBytes, PaddingMode.None);

        return Encoding.UTF8.GetString(decrypted);
    }

}