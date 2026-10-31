#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DouyuTriggerWindowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost<global::CardGame.Core.DouyuWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
{
    public override bool IsComplete(DouyuWindowFrame f) => f.CandidateIndex == f.Entries.Count;
    public override ProgramTriggerWindowFlow Close(DouyuWindowFrame f)
    {
        getCharacterPangJiOlRuntime().CloseDouyuWindow(f);
        return ProgramTriggerWindowFlow.Yield;
    }

    protected override ProgramTriggerCandidate Candidate(DouyuWindowFrame f) => f.Entries[f.CandidateIndex].Candidate;
    public override ProgramTriggerWindowEntry Read(DouyuWindowFrame f) => new(Candidate(f), getCharacterPangJiOlRuntime().DouyuContext(f, Candidate(f)));
    public override void Skip(DouyuWindowFrame f, ProgramTriggerCandidate c) => getCharacterPangJiOlRuntime().SkipDouyuCandidate(f, c);
}
