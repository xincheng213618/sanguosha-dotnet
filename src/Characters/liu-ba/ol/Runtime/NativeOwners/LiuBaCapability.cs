#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuBaCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries> getCharacterLiuBaOlRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : ILiuBaHost, ILiuBaSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuBaOlRuntime().BeginLiuBa(f, op);
    public int MaximumHp(int ownerSeat) => matchParticipantsState.ReferenceToPlayers[ownerSeat].MaxHp;
}
