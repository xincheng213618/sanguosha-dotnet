#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpPangTongNativeEntries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : ISpPangTongNativeEntries
{
    public IReadOnlyList<PromptChoice> OwnedCardPaymentChoices(long frameId, int chooser, int owner, IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(frameId, chooser, owner, zones, intent);
    public bool BeginPublicHandReveal(long frameId, string returnKey) => getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow2(frameId, returnKey);
    public void Recover(long frameId, int owner, int target, int amount) => createProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    public void ChangeMaximumHp(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(frame, amount);
    public void PublishState() => advanceRulesAndPublishState();
}
