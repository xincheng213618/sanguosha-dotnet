#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPublicCardProtocolQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries> getCharacterLuSuMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries> getCoreGameEngineRecipientCategoryMarkObserversRuntime) : IProgramPublicCardProtocolQueries
{
    public string CompletedUndamagedRevealBind => global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.CompletedUndamagedRevealBind;

    public bool ValidMouLuSuReceipt(ProgramSkillFrame frame) => getCharacterLuSuMouRuntime().MouLuSuAnniversary.Valid(frame);
    public bool ValidRecipientCategoryMarkReceipt(ProgramSkillFrame frame) => getCoreGameEngineRecipientCategoryMarkObserversRuntime().ValidRecipientCategoryMarkReceipt(frame);
}
