#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResponseExchangeProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries> getCoreGameEngineResponseEntityExchangeRuntime) : IResponseExchangeProgramHost
{
    public SkillProgramStepOutcome ExchangeRespondedCardEntities(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineResponseEntityExchangeRuntime().BeginResponseEntityExchange(frame, effect);
    public SkillProgramStepOutcome DrawPublicSuitThenEscalatingDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineResponseEntityExchangeRuntime().BeginPublicSuitEscalatingDiscard(frame, effect);
}
