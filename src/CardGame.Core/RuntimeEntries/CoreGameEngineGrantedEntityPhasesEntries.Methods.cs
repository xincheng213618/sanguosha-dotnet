#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGrantedEntityPhasesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGrantedEntityPlayPhase(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginGrantedEntityPlayPhase(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGrantedPhaseSlashClaim(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginGrantedPhaseSlashClaim(input);
    internal bool CanOfferGrantedPhaseSlash(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanOfferGrantedPhaseSlash(candidate, trigger);
    internal global::CardGame.Core.ProgramSkillFrame FreezeGrantedEntityPhaseEnd(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FreezeGrantedEntityPhaseEnd(f);
    internal long? GrantedEntityTrailerHealthOwner(global::CardGame.Core.IGameEvent payload) => _resolve().GrantedEntityTrailerHealthOwner(payload);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GrantedPhaseSlashChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantedPhaseSlashChoices(f);
    internal bool HasGrantedPhaseEntityDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? ids) => _resolve().HasGrantedPhaseEntityDistance(owner, ids);
    internal bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor) => _resolve().HasIssuedGrantedPhaseEntityDistance(useId, actor);
    internal bool HasPotentialGrantedPhaseSlash(global::CardGame.Core.CharacterState owner) => _resolve().HasPotentialGrantedPhaseSlash(owner);
    internal bool IsGrantedEntityPhaseActive(int owner) => _resolve().IsGrantedEntityPhaseActive(owner);
    internal bool IsReturningGrantedEntityPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsReturningGrantedEntityPhase(f);
    internal void IssueGrantedEntityPhaseBoundary(global::CardGame.Core.CharacterState actor) => _resolve().IssueGrantedEntityPhaseBoundary(actor);
    internal void IssueGrantedPhaseEntityUseDistance(long frameId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueGrantedPhaseEntityUseDistance(frameId, action);
    internal void PublishGrantedPhaseSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGrantedPhaseSlash(f);
    internal void ResolveGrantedPhaseSlashChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveGrantedPhaseSlashChoice(choice);
    internal bool ResumeGrantedEntityPhaseTrailer(long frameId) => _resolve().ResumeGrantedEntityPhaseTrailer(frameId);
    internal bool ResumeGrantedPhaseSlashClaim(long frameId) => _resolve().ResumeGrantedPhaseSlashClaim(frameId);
}
