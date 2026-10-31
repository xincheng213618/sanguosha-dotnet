#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiPlayEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildWushuangJinzhiPlayActions(global::CardGame.Core.CharacterState owner) => _resolve().BuildWushuangJinzhiPlayActions(owner);
    internal bool TryExecuteWushuangJinzhiPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction selected, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryExecuteWushuangJinzhiPlay(actor, selected, ids, targets);
    internal bool TrySubmitWushuangJinzhiPlay(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitWushuangJinzhiPlay(command, out result);
    internal global::CardGame.Core.CommandError? ValidateWushuangJinzhiPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateWushuangJinzhiPlay(owner, action, ids, targets);
    internal bool WushuangJinzhiAlcoholAllowed(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().WushuangJinzhiAlcoholAllowed(owner, cards);
    internal global::System.Collections.Generic.IReadOnlyList<int> WushuangJinzhiPlayTargets(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().WushuangJinzhiPlayTargets(owner, selection);
}
