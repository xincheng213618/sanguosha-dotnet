using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<int>? _actorHandLimitAnnouncedTargets;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<int>? ActorHandLimitAnnouncedTargets
    {
        get => _actorHandLimitAnnouncedTargets;
        init => _actorHandLimitAnnouncedTargets = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}
