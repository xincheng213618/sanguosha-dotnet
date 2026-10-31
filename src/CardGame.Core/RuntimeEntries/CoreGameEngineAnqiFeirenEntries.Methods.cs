#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAnqiFeirenEntries
{
    internal Card? ActiveAnqiFeiren(int owner, string skillId, string instanceId) => _resolve().ActiveAnqiFeiren(owner, skillId, instanceId);
    internal string[] AnqiFeirenCandidates(int target) => _resolve().AnqiFeirenCandidates(target);
    internal bool CanOfferAnqiFeiren(ProgramTriggerLegalityContext gate) => _resolve().CanOfferAnqiFeiren(gate);
    internal void ExpireAnqiFeiren(TurnEndedEvent turn) => _resolve().ExpireAnqiFeiren(turn);
    internal bool HasSpentAnqiFeiren(int owner, int target) => _resolve().HasSpentAnqiFeiren(owner, target);
    internal bool IsAnqiFeirenGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsAnqiFeirenGrantQualified(owner, grant);
    internal SkillProgramStepOutcome ResolveAnqiFeiren(ProgramSkillFrame supplied) => _resolve().ResolveAnqiFeiren(supplied);
    internal bool HasAnqiFeirenCapability { get => _resolve().HasAnqiFeirenCapability; }
}
