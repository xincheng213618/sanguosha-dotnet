#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthReplacementProgramsEntries
{
    internal bool ApplyAttackAsHpLoss(GameEngine.IDamageAttempt attack) => _resolve().ApplyAttackAsHpLoss(attack);
    internal void ObserveProgramHealthChange(IGameEvent payload) => _resolve().ObserveProgramHealthChange(payload);
}
