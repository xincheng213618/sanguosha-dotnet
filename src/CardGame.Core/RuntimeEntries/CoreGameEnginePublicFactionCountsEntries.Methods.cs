#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicFactionCountsEntries
{
    internal string? GetPublicEffectiveFactionId(CharacterState player) => _resolve().GetPublicEffectiveFactionId(player);
    internal int GetPublicLivingFactionCount() => _resolve().GetPublicLivingFactionCount();
}
