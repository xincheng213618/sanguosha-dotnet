#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangWindowHost(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : ProgramTriggerWindowHost<WuLuKangPindianFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
{
    protected override ProgramTriggerCandidate Candidate(WuLuKangPindianFrame w) => w.Entries[w.CandidateIndex].Candidate;
    public override bool IsComplete(WuLuKangPindianFrame w) => w.CandidateIndex == w.Entries.Count;
    public override ProgramTriggerWindowEntry Read(WuLuKangPindianFrame w) => new(Candidate(w), getCharacterLuKangWumiaoRuntime().WuLuKangContext(w, Candidate(w)));
    public override void Skip(WuLuKangPindianFrame w, ProgramTriggerCandidate c) => getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
    public override ProgramTriggerWindowFlow Close(WuLuKangPindianFrame w)
    {
        getCharacterLuKangWumiaoRuntime().CloseWuLuKangWindow(w);
        return ProgramTriggerWindowFlow.Yield;
    }
}
