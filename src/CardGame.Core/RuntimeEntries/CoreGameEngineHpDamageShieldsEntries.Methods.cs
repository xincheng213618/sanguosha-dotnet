#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpDamageShieldsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHpDamageShieldPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().BeginHpDamageShieldPayment(frame, marker);
    internal bool CanContinuePaidHpDamageShield(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidHpDamageShield(frame);
    internal double GetHpDamageShieldAiTargetValue(global::CardGame.Core.CharacterState owner) => _resolve().GetHpDamageShieldAiTargetValue(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HpDamageShieldChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HpDamageShieldChoices(frame);
    internal int IndependentMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source) => _resolve().IndependentMarkerCount(target, marker, source);
    internal global::CardGame.Core.ProgramDamageInterception InterceptOneUseDamageShield(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().InterceptOneUseDamageShield(attack, amount, nature);
    internal void ResolveHpDamageShieldPayment(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHpDamageShieldPayment(choice);
    internal bool ResumeHpDamageShieldPayment(long frameId) => _resolve().ResumeHpDamageShieldPayment(frameId);
    internal global::CardGame.Core.PromptChoice SelectAiHpDamageShield(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiHpDamageShield(decision);
    internal bool TryConsumeOneUseDamageShield(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().TryConsumeOneUseDamageShield(attack, amount);
}
