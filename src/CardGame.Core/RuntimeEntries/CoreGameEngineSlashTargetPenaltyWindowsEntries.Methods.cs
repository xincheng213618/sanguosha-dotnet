#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyWindowsEntries
{
    internal bool CanRunSlashTargetPenalty(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSlashTargetPenalty(c, context);
    internal void CompleteSlashTargetPenaltyBinding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteSlashTargetPenaltyBinding(f);
    internal void ContinueSlashTargetPenaltyWindow() => _resolve().ContinueSlashTargetPenaltyWindow();
    internal bool? DecideIssuedTrigger710(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger710(candidate, context);
    internal bool MatchesSlashTargetPenaltyUse(global::CardGame.Core.SlashTargetPenaltyIdentity r) => _resolve().MatchesSlashTargetPenaltyUse(r);
    internal bool SlashTargetPenaltyProgramMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SlashTargetPenaltyProgramMatches(f);
    internal bool SlashTargetPenaltyWindowMatches(global::CardGame.Core.SlashTargetPenaltyWindowFrame w) => _resolve().SlashTargetPenaltyWindowMatches(w);
    internal bool TryBeginSlashTargetPenalties(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false) => _resolve().TryBeginSlashTargetPenalties(attack, legacy, afterActualTargets);
    internal bool HasSlashTargetPenaltyPrograms { get => _resolve().HasSlashTargetPenaltyPrograms; }
}
