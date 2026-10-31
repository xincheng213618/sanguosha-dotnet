#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateGeneralLibrariesEntries
{
    internal long CaptureCombinedProjectionDependencyStamp() => _resolve().CaptureCombinedProjectionDependencyStamp();
    internal PrivateGeneralLibrary DrawPrivateGeneralAvatars(PrivateGeneralLibrary library, int amount, IReadOnlyCollection<string>? excludedIds = default) => _resolve().DrawPrivateGeneralAvatars(library, amount, excludedIds);
    internal PrivateGeneralLibrary? EffectiveGeneralLibrary(CharacterState owner) => _resolve().EffectiveGeneralLibrary(owner);
    internal SkillProgramStepOutcome ExecutePrivateGeneralLibrary(SkillProgramEffect e, ProgramSkillFrame f) => _resolve().ExecutePrivateGeneralLibrary(e, f);
    internal PrivateGeneralLibrary? FindEffectiveGeneralLibrary(CharacterState owner) => _resolve().FindEffectiveGeneralLibrary(owner);
    internal void FinishPrivateGeneralLibrarySelection(ProgramSkillFrame f) => _resolve().FinishPrivateGeneralLibrarySelection(f);
    internal string? GetPrivateGeneralLibraryFaction(CharacterState owner) => _resolve().GetPrivateGeneralLibraryFaction(owner);
    internal GeneralGender? GetPrivateGeneralLibraryGender(CharacterState owner) => _resolve().GetPrivateGeneralLibraryGender(owner);
    internal bool IsGeneralLibraryGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsGeneralLibraryGrantQualified(owner, grant);
    internal bool LibraryBaseGrantLive(CharacterState owner, SkillGrant grant) => _resolve().LibraryBaseGrantLive(owner, grant);
    internal bool LibraryBaseLordProjectionLive(CharacterState owner, SkillGrant grant) => _resolve().LibraryBaseLordProjectionLive(owner, grant);
    internal bool LibraryBaseSourceLive(PrivateGeneralLibrarySource source) => _resolve().LibraryBaseSourceLive(source);
    internal bool LibraryProjectionRelationLive(SkillGrant grant, GeneralLibraryProjectionSource relation) => _resolve().LibraryProjectionRelationLive(grant, relation);
    internal bool LibrarySourceQualified(PrivateGeneralLibrarySource source) => _resolve().LibrarySourceQualified(source);
    internal IEnumerable<string> PrivateGeneralCandidates() => _resolve().PrivateGeneralCandidates();
    internal IReadOnlyList<string> PrivateGeneralDeclarableSkills(string generalId, PrivateGeneralLibraryPolicy policy) => _resolve().PrivateGeneralDeclarableSkills(generalId, policy);
    internal IReadOnlyList<PromptChoice> PrivateGeneralLibraryChoices(ProgramSkillFrame f) => _resolve().PrivateGeneralLibraryChoices(f);
    internal IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(CharacterState owner) => _resolve().PrivateGeneralLibrarySuppressionInputs(owner);
    internal IReadOnlyList<PrivateGeneralLibrarySnapshot>? ProjectPrivateGeneralLibraries(CharacterState owner, int viewerSeat, bool revealAll) => _resolve().ProjectPrivateGeneralLibraries(owner, viewerSeat, revealAll);
    internal void PublishPrivateGeneralLibraryChoice(ProgramSkillFrame f) => _resolve().PublishPrivateGeneralLibraryChoice(f);
    internal void ResolvePrivateGeneralLibraryChoice(PromptChoice choice) => _resolve().ResolvePrivateGeneralLibraryChoice(choice);
    internal PrivateGeneralLibrarySource? ResolvePrivateGeneralLibrarySource(ProgramSkillFrame f, SkillProgramEffect e) => _resolve().ResolvePrivateGeneralLibrarySource(f, e);
    internal bool ResumePrivateGeneralLibrarySelection(long id) => _resolve().ResumePrivateGeneralLibrarySelection(id);
    internal void SynchronizePrivateGeneralLibraries() => _resolve().SynchronizePrivateGeneralLibraries();
    internal bool HasPrivateGeneralLibraryCapability { get => _resolve().HasPrivateGeneralLibraryCapability; }
}
