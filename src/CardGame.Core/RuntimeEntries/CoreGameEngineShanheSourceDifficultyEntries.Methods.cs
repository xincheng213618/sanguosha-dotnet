#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceDifficultyEntries
{
    internal void RestoreShanheSourceRoundArmor() => _resolve().RestoreShanheSourceRoundArmor();
    internal bool RequiresShanheSourceArmorTracking { get => _resolve().RequiresShanheSourceArmorTracking; }
}
