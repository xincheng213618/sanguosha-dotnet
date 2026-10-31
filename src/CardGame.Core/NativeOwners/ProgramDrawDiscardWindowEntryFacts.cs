#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDrawDiscardWindowEntryFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries> getCoreGameEngineGuanduActivitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangNiBoundaryMobileEntries> getCharacterZhangNiBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramDrawDiscardWindowEntryFacts
{
    public bool IsGuanduActivity => getCoreGameEngineGuanduActivitiesRuntime().IsGuanduActivity;

    public bool HasLivingBindings(SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().HasLivingProgramTriggerBindings(window);
    public bool HasPendingBoundaryWurongDrawSkip(int ownerSeat) => getCharacterZhangNiBoundaryMobileRuntime().HasPendingBoundaryWurongDrawSkip(ownerSeat);
    public bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => getCharacterZhangShiPingOlRuntime().HasOutstandingHongjiPhase(owner, window);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
