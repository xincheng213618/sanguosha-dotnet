#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBossActivitiesRuntime
{
    internal int BossActivitySeat => (Math.Max(0, _coreMatchDefinitionOptionsPlayerCountAccess._options.HumanSeat) + 3) % _coreMatchDefinitionOptionsPlayerCountAccess._playerCount;
    internal int BossActivityStageIndex => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<BossActivityStageStartedEvent>().Select(e => e.StageIndex).DefaultIfEmpty(0).Last();
}
