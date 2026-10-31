#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpDamageShieldsEntries
{
    internal SkillProgramStepOutcome BeginHpDamageShieldPayment(ProgramSkillFrame frame, PlayerMarkerKind marker) => _resolve().BeginHpDamageShieldPayment(frame, marker);
    internal bool CanContinuePaidHpDamageShield(ProgramSkillFrame frame) => _resolve().CanContinuePaidHpDamageShield(frame);
    internal double GetHpDamageShieldAiTargetValue(CharacterState owner) => _resolve().GetHpDamageShieldAiTargetValue(owner);
    internal IReadOnlyList<PromptChoice> HpDamageShieldChoices(ProgramSkillFrame frame) => _resolve().HpDamageShieldChoices(frame);
    internal int IndependentMarkerCount(CharacterState target, PlayerMarkerKind marker, int source) => _resolve().IndependentMarkerCount(target, marker, source);
    internal ProgramDamageInterception InterceptOneUseDamageShield(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().InterceptOneUseDamageShield(attack, amount, nature);
    internal void ResolveHpDamageShieldPayment(PromptChoice choice) => _resolve().ResolveHpDamageShieldPayment(choice);
    internal bool ResumeHpDamageShieldPayment(long frameId) => _resolve().ResumeHpDamageShieldPayment(frameId);
    internal PromptChoice SelectAiHpDamageShield(PendingDecision decision) => _resolve().SelectAiHpDamageShield(decision);
    internal bool TryConsumeOneUseDamageShield(GameEngine.IDamageAttempt attack, int amount) => _resolve().TryConsumeOneUseDamageShield(attack, amount);
}
