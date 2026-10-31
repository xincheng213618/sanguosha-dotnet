#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKunlunMirrorEntries
{
    internal bool CanOfferKunlunMirror(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferKunlunMirror(context);
    internal bool HasEffectiveKunlunMirror(int owner, string instance) => _resolve().HasEffectiveKunlunMirror(owner, instance);
    internal bool KnowsKunlunMirrorName(int owner, string name) => _resolve().KnowsKunlunMirrorName(owner, name);
    internal void ResolveKunlunMirror(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveKunlunMirror(supplied);
    internal bool TryKunlunMirrorOrigin(global::CardGame.Core.GameEngine.IDamageAttempt? attack, scoped out global::CardGame.Core.KunlunMirrorDamageOrigin origin) => _resolve().TryKunlunMirrorOrigin(attack, out origin);
}
