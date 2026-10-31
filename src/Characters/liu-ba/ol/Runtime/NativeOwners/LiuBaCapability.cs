#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuBaCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuBaOlRuntime>> getCharacterLiuBaOlRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : ILiuBaHost, ILiuBaSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuBaOlRuntime().Value.BeginLiuBa(f, op);
    public int MaximumHp(int ownerSeat) => matchParticipantsState.ReferenceToPlayers[ownerSeat].MaxHp;
}
