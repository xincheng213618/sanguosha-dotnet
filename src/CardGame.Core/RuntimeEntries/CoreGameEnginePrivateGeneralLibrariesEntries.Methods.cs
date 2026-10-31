#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateGeneralLibrariesEntries
{
    internal long CaptureCombinedProjectionDependencyStamp() => _resolve().CaptureCombinedProjectionDependencyStamp();
    internal global::CardGame.Core.PrivateGeneralLibrary DrawPrivateGeneralAvatars(global::CardGame.Core.PrivateGeneralLibrary library, int amount, global::System.Collections.Generic.IReadOnlyCollection<string>? excludedIds = default) => _resolve().DrawPrivateGeneralAvatars(library, amount, excludedIds);
    internal global::CardGame.Core.PrivateGeneralLibrary? EffectiveGeneralLibrary(global::CardGame.Core.CharacterState owner) => _resolve().EffectiveGeneralLibrary(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecutePrivateGeneralLibrary(global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExecutePrivateGeneralLibrary(e, f);
    internal global::CardGame.Core.PrivateGeneralLibrary? FindEffectiveGeneralLibrary(global::CardGame.Core.CharacterState owner) => _resolve().FindEffectiveGeneralLibrary(owner);
    internal void FinishPrivateGeneralLibrarySelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishPrivateGeneralLibrarySelection(f);
    internal string? GetPrivateGeneralLibraryFaction(global::CardGame.Core.CharacterState owner) => _resolve().GetPrivateGeneralLibraryFaction(owner);
    internal global::CardGame.Core.GeneralGender? GetPrivateGeneralLibraryGender(global::CardGame.Core.CharacterState owner) => _resolve().GetPrivateGeneralLibraryGender(owner);
    internal bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsGeneralLibraryGrantQualified(owner, grant);
    internal bool LibraryBaseGrantLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().LibraryBaseGrantLive(owner, grant);
    internal bool LibraryBaseLordProjectionLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().LibraryBaseLordProjectionLive(owner, grant);
    internal bool LibraryBaseSourceLive(global::CardGame.Core.PrivateGeneralLibrarySource source) => _resolve().LibraryBaseSourceLive(source);
    internal bool LibraryProjectionRelationLive(global::CardGame.Core.SkillGrant grant, global::CardGame.Core.GeneralLibraryProjectionSource relation) => _resolve().LibraryProjectionRelationLive(grant, relation);
    internal bool LibrarySourceQualified(global::CardGame.Core.PrivateGeneralLibrarySource source) => _resolve().LibrarySourceQualified(source);
    internal global::System.Collections.Generic.IEnumerable<string> PrivateGeneralCandidates() => _resolve().PrivateGeneralCandidates();
    internal global::System.Collections.Generic.IReadOnlyList<string> PrivateGeneralDeclarableSkills(string generalId, global::CardGame.Core.PrivateGeneralLibraryPolicy policy) => _resolve().PrivateGeneralDeclarableSkills(generalId, policy);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrivateGeneralLibraryChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrivateGeneralLibraryChoices(f);
    internal global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner) => _resolve().PrivateGeneralLibrarySuppressionInputs(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateGeneralLibrarySnapshot>? ProjectPrivateGeneralLibraries(global::CardGame.Core.CharacterState owner, int viewerSeat, bool revealAll) => _resolve().ProjectPrivateGeneralLibraries(owner, viewerSeat, revealAll);
    internal void PublishPrivateGeneralLibraryChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPrivateGeneralLibraryChoice(f);
    internal void ResolvePrivateGeneralLibraryChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePrivateGeneralLibraryChoice(choice);
    internal global::CardGame.Core.PrivateGeneralLibrarySource? ResolvePrivateGeneralLibrarySource(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect e) => _resolve().ResolvePrivateGeneralLibrarySource(f, e);
    internal bool ResumePrivateGeneralLibrarySelection(long id) => _resolve().ResumePrivateGeneralLibrarySelection(id);
    internal void SynchronizePrivateGeneralLibraries() => _resolve().SynchronizePrivateGeneralLibraries();
    internal bool HasPrivateGeneralLibraryCapability { get => _resolve().HasPrivateGeneralLibraryCapability; }
}
