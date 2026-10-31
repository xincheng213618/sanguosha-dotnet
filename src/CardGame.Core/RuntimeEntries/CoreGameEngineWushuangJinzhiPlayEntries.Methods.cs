#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiPlayEntries
{
    internal IEnumerable<LegalAction> BuildWushuangJinzhiPlayActions(CharacterState owner) => _resolve().BuildWushuangJinzhiPlayActions(owner);
    internal bool TryExecuteWushuangJinzhiPlay(CharacterState actor, LegalAction selected, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().TryExecuteWushuangJinzhiPlay(actor, selected, ids, targets);
    internal bool TrySubmitWushuangJinzhiPlay(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().TrySubmitWushuangJinzhiPlay(command, out result);
    internal CommandError? ValidateWushuangJinzhiPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateWushuangJinzhiPlay(owner, action, ids, targets);
    internal bool WushuangJinzhiAlcoholAllowed(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().WushuangJinzhiAlcoholAllowed(owner, cards);
    internal IReadOnlyList<int> WushuangJinzhiPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().WushuangJinzhiPlayTargets(owner, selection);
}
