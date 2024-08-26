using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TFPAW.Models;

public class GptResponse
{
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
}
