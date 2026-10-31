#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ChengJiChengCuiCapability(Func<RuntimeEntries.CharacterChengJiChengCuiOlEntries> getCharacterChengJiChengCuiOlRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IChengJiChengCuiHost, ITousuiSelectionQuery
{
    public IReadOnlyList<Card> Cards(int owner) => getCharacterChengJiChengCuiOlRuntime().TousuiSources(matchParticipantsState.ReferenceToPlayers[owner]).SelectMany(getCharacterChengJiChengCuiOlRuntime().TousuiRequestCards).DistinctBy(c => c.Id).ToArray();
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveTousuiDodgeRequirement => getCharacterChengJiChengCuiOlRuntime().ResolveTousuiRequirement(f),
        SkillProgramEffectOp.ResolveXumingDamagePromise => getCharacterChengJiChengCuiOlRuntime().ResolveXumingDamage(f),
        SkillProgramEffectOp.ResolveXumingIssuedTurnEndUses => getCharacterChengJiChengCuiOlRuntime().BeginXumingEnding(f),
        _ => throw new InvalidOperationException("Unknown Cheng Ji Cheng Cui operation.")};
}
