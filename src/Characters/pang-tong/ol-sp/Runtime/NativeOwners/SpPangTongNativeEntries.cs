#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SpPangTongNativeEntries(Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : ISpPangTongNativeEntries
{
    public IReadOnlyList<PromptChoice> OwnedCardPaymentChoices(long frameId, int chooser, int owner, IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(frameId, chooser, owner, zones, intent);
    public bool BeginPublicHandReveal(long frameId, string returnKey) => getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow2(frameId, returnKey);
    public void Recover(long frameId, int owner, int target, int amount) => createProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    public void ChangeMaximumHp(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(frame, amount);
    public void PublishState() => advanceRulesAndPublishState();
}
