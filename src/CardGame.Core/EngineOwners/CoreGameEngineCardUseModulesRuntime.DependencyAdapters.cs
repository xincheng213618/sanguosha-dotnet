#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardUseModulesCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICoreCardUseModulesCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramHandPhysicalRestricted(actorSeat, cardId);
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
