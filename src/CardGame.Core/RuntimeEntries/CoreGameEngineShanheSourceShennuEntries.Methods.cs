#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceShennuEntries
{
    internal bool CanReceiveShanheSourceShennuDamage(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanReceiveShanheSourceShennuDamage(context);
    internal bool CaptureShanheShennuOutgoing(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageId) => _resolve().CaptureShanheShennuOutgoing(attack, damageId);
    internal void CompleteShanheShennuOutgoing(global::CardGame.Core.AfterDamageEvent completed) => _resolve().CompleteShanheShennuOutgoing(completed);
    internal void ReceiveShanheSourceShennuDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReceiveShanheSourceShennuDamage(supplied);
    internal int ShanheShennuDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShanheShennuDamageBonus(attack);
    internal int ShanheShennuPublicDamageBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack) => _resolve().ShanheShennuPublicDamageBonus(view, sourceSeat, sourceLess, chain, attack);
    internal bool IsShanheSourceShennuMode { get => _resolve().IsShanheSourceShennuMode; }
}
