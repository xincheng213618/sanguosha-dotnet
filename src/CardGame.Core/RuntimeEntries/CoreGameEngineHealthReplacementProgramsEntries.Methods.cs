#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthReplacementProgramsEntries
{
    internal bool ApplyAttackAsHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ApplyAttackAsHpLoss(attack);
    internal void ObserveProgramHealthChange(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveProgramHealthChange(payload);
}
