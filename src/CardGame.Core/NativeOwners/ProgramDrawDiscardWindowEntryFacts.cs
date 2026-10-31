#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDrawDiscardWindowEntryFacts(Func<RuntimeEntries.CoreGameEngineGuanduActivitiesEntries> getCoreGameEngineGuanduActivitiesRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangNiBoundaryMobileRuntime>> getCharacterZhangNiBoundaryMobileRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangShiPingOlRuntime>> getCharacterZhangShiPingOlRuntime, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramDrawDiscardWindowEntryFacts
{
    public bool IsGuanduActivity => getCoreGameEngineGuanduActivitiesRuntime().IsGuanduActivity;

    public bool HasLivingBindings(SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().HasLivingProgramTriggerBindings(window);
    public bool HasPendingBoundaryWurongDrawSkip(int ownerSeat) => getCharacterZhangNiBoundaryMobileRuntime().Value.HasPendingBoundaryWurongDrawSkip(ownerSeat);
    public bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => getCharacterZhangShiPingOlRuntime().Value.HasOutstandingHongjiPhase(owner, window);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
