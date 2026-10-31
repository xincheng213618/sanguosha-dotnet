#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSteadyHandEntries
{
    internal bool IsShanheSteadyHandModifierQualified(int ownerSeat, string skillId) => _resolve().IsShanheSteadyHandModifierQualified(ownerSeat, skillId);
}
