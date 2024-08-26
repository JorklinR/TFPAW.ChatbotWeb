using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TFPAW.Models;

public class Choice
{
    [JsonPropertyName("message")]
    public Message Message { get; set; }
}
