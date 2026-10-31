#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DouyuTriggerWindowHost(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : ProgramTriggerWindowHost<DouyuWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
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
