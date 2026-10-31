#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGrantedEntityPhasesEntries
{
    internal SkillProgramStepOutcome BeginGrantedEntityPlayPhase(ProgramSkillFrame input) => _resolve().BeginGrantedEntityPlayPhase(input);
    internal SkillProgramStepOutcome BeginGrantedPhaseSlashClaim(ProgramSkillFrame input) => _resolve().BeginGrantedPhaseSlashClaim(input);
    internal bool CanOfferGrantedPhaseSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().CanOfferGrantedPhaseSlash(candidate, trigger);
    internal ProgramSkillFrame FreezeGrantedEntityPhaseEnd(ProgramSkillFrame f) => _resolve().FreezeGrantedEntityPhaseEnd(f);
    internal long? GrantedEntityTrailerHealthOwner(IGameEvent payload) => _resolve().GrantedEntityTrailerHealthOwner(payload);
    internal IReadOnlyList<PromptChoice> GrantedPhaseSlashChoices(ProgramSkillFrame f) => _resolve().GrantedPhaseSlashChoices(f);
    internal bool HasGrantedPhaseEntityDistance(CharacterState owner, IReadOnlyList<int>? ids) => _resolve().HasGrantedPhaseEntityDistance(owner, ids);
    internal bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor) => _resolve().HasIssuedGrantedPhaseEntityDistance(useId, actor);
    internal bool HasPotentialGrantedPhaseSlash(CharacterState owner) => _resolve().HasPotentialGrantedPhaseSlash(owner);
    internal bool IsGrantedEntityPhaseActive(int owner) => _resolve().IsGrantedEntityPhaseActive(owner);
    internal bool IsReturningGrantedEntityPhase(ProgramSkillFrame f) => _resolve().IsReturningGrantedEntityPhase(f);
    internal void IssueGrantedEntityPhaseBoundary(CharacterState actor) => _resolve().IssueGrantedEntityPhaseBoundary(actor);
    internal void IssueGrantedPhaseEntityUseDistance(long frameId, CardActionContext? action) => _resolve().IssueGrantedPhaseEntityUseDistance(frameId, action);
    internal void PublishGrantedPhaseSlash(ProgramSkillFrame f) => _resolve().PublishGrantedPhaseSlash(f);
    internal void ResolveGrantedPhaseSlashChoice(PromptChoice choice) => _resolve().ResolveGrantedPhaseSlashChoice(choice);
    internal bool ResumeGrantedEntityPhaseTrailer(long frameId) => _resolve().ResumeGrantedEntityPhaseTrailer(frameId);
    internal bool ResumeGrantedPhaseSlashClaim(long frameId) => _resolve().ResumeGrantedPhaseSlashClaim(frameId);
}
