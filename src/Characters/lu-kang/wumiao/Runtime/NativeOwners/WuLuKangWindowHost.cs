#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuKangWindowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost<global::CardGame.Core.WuLuKangPindianFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
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
