using System.Globalization;
using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] public bool ShenXunYuEditing { get; init; }
}
public sealed partial class GameEngine
{
}
