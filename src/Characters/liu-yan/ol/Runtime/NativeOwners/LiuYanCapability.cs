#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuYanCapability(Func<RuntimeEntries.CharacterLiuYanOlEntries> getCharacterLiuYanOlRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : ILiuYanHost, ILiuYanSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuYanOlRuntime().BeginLiuYan(f, op);
    public bool CanUse(int owner, Card card, string skill) => getCharacterLiuYanOlRuntime().LiuYanLimuMaterialLegal(matchParticipantsState.ReferenceToPlayers[owner], card, skill);
}
