// En Models/Auth/RefreshResponse.cs
using System.Text.Json.Serialization;

namespace frontendnet.Models;


public class RefreshResponse
{
    [JsonPropertyName("accessToken")] // Mapea el JSON de Node.js
    public string AccessToken { get; set; } = string.Empty;
}