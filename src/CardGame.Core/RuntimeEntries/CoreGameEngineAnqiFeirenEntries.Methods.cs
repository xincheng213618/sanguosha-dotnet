#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAnqiFeirenEntries
{
    internal global::CardGame.Core.Card? ActiveAnqiFeiren(int owner, string skillId, string instanceId) => _resolve().ActiveAnqiFeiren(owner, skillId, instanceId);
    internal string[] AnqiFeirenCandidates(int target) => _resolve().AnqiFeirenCandidates(target);
    internal bool CanOfferAnqiFeiren(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferAnqiFeiren(gate);
    internal void ExpireAnqiFeiren(global::CardGame.Core.TurnEndedEvent turn) => _resolve().ExpireAnqiFeiren(turn);
    internal bool HasSpentAnqiFeiren(int owner, int target) => _resolve().HasSpentAnqiFeiren(owner, target);
    internal bool IsAnqiFeirenGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsAnqiFeirenGrantQualified(owner, grant);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveAnqiFeiren(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveAnqiFeiren(supplied);
    internal bool HasAnqiFeirenCapability { get => _resolve().HasAnqiFeirenCapability; }
}
