#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ConvertingGiftProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries> getCoreGameEngineConvertingGiftsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IConvertingGiftProgramHost
{
    public SkillProgramStepOutcome GiveSelectedOwnedCardAndDamage(ProgramSkillFrame f, int limit) => getCoreGameEngineConvertingGiftsRuntime().BeginConvertingGiftDamage(f, limit);
    public SkillProgramStepOutcome ObserveDamageSourceHandAndGive(ProgramSkillFrame f, int limit) => getCoreGameEngineConvertingGiftsRuntime().BeginDamageSourceObservationGift(f, limit);
    public SkillProgramStepOutcome DrawToHandCount(ProgramSkillFrame f, int seat, int limit, SkillProgramNumberExpression? expression = null, bool capAtTargetMaxHp = false) => getCoreGameEngineConvertingGiftsRuntime().DrawProgramToHandCount(f, seat, expression == SkillProgramNumberExpression.LivingPlayerCount ? matchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive) : limit, capAtTargetMaxHp);
}
