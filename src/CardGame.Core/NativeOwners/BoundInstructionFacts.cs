#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundInstructionFacts(global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuGuoTaiBoundaryMobileEntries> getCharacterWuGuoTaiBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLiejieSourceDiscardsEntries> getCoreGameEngineLiejieSourceDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries> getCoreGameEngineCardDistributionPresentationRuntime) : IProgramBoundInstructionFacts
{
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public bool HasForeignDiscardCapability => getCoreForeignEquipmentDiscardPoliciesRuntime().HasForeignDiscardCapability;
    public bool HasAtomicDiscardCapability => getCoreHellMengpoProgramsRuntime().HasHellShiyouCapability;

    public int? SelectionActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => getCharacterWuGuoTaiBoundaryMobileRuntime().BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
    public int Participant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
    public IReadOnlyList<int> TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context = null) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTargetSeats(owner, kind, context);
    public bool IsAtomicDiscard(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineLiejieSourceDiscardsRuntime().IsAtomicLiejieCostMove(frame, sourceBind);
    public IReadOnlyList<PromptChoice> PresentDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string sourceBind, SkillProgramTargetKind targetKind) => getCoreGameEngineCardDistributionPresentationRuntime().PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
}
