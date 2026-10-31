#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyWindowsEntries
{
    internal bool CanRunSlashTargetPenalty(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunSlashTargetPenalty(c, context);
    internal void CompleteSlashTargetPenaltyBinding(ProgramSkillFrame f) => _resolve().CompleteSlashTargetPenaltyBinding(f);
    internal void ContinueSlashTargetPenaltyWindow() => _resolve().ContinueSlashTargetPenaltyWindow();
    internal bool? DecideIssuedTrigger710(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger710(candidate, context);
    internal bool MatchesSlashTargetPenaltyUse(SlashTargetPenaltyIdentity r) => _resolve().MatchesSlashTargetPenaltyUse(r);
    internal bool SlashTargetPenaltyProgramMatches(ProgramSkillFrame f) => _resolve().SlashTargetPenaltyProgramMatches(f);
    internal bool SlashTargetPenaltyWindowMatches(SlashTargetPenaltyWindowFrame w) => _resolve().SlashTargetPenaltyWindowMatches(w);
    internal bool TryBeginSlashTargetPenalties(NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false) => _resolve().TryBeginSlashTargetPenalties(attack, legacy, afterActualTargets);
    internal bool HasSlashTargetPenaltyPrograms { get => _resolve().HasSlashTargetPenaltyPrograms; }
}
