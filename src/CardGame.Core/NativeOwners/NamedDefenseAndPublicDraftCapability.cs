#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NamedDefenseAndPublicDraftCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedDefenseAndPublicDraftEntries> getCoreGameEngineNamedDefenseAndPublicDraftRuntime) : INamedDefenseAndPublicDraftHost
{
    public SkillProgramStepOutcome BeginNamedTargetDefense(ProgramSkillFrame frame, string ledgerId, IReadOnlyList<CardKind> names) => getCoreGameEngineNamedDefenseAndPublicDraftRuntime().BeginNamedTargetDefense(frame, ledgerId, names);
    public SkillProgramStepOutcome BeginLowHandPublicDraft(ProgramSkillFrame frame) => getCoreGameEngineNamedDefenseAndPublicDraftRuntime().BeginLowHandPublicDraft(frame);
}
