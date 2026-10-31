#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPublicCardProtocolQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuSuMouRuntime>> getCharacterLuSuMouRuntime, Func<RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries> getCoreGameEngineRecipientCategoryMarkObserversRuntime) : IProgramPublicCardProtocolQueries
{
    public string CompletedUndamagedRevealBind => EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.CompletedUndamagedRevealBind;

    public bool ValidMouLuSuReceipt(ProgramSkillFrame frame) => getCharacterLuSuMouRuntime().Value.MouLuSuAnniversary.Valid(frame);
    public bool ValidRecipientCategoryMarkReceipt(ProgramSkillFrame frame) => getCoreGameEngineRecipientCategoryMarkObserversRuntime().ValidRecipientCategoryMarkReceipt(frame);
}
