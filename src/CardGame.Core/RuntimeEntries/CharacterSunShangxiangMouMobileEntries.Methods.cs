#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShangxiangMouMobileEntries
{
    internal void ChangeMouSsxFailureFaction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChangeMouSsxFailureFaction(f);
    internal void ChangeMouSsxZhu(global::CardGame.Core.ProgramSkillFrame f, int previous, int current, bool initial) => _resolve().ChangeMouSsxZhu(f, previous, current, initial);
    internal void GrantMouSsxArmor(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().GrantMouSsxArmor(f, seat);
    internal bool IsIssuedMouSsxZhuang(global::CardGame.Core.PublicPersistentPileSource source) => _resolve().IsIssuedMouSsxZhuang(source);
    internal bool MouSsxParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouSsxParentMatches(f);
    internal void SealMouSsxMove(long id, global::CardGame.Core.CardMovementRecord move, bool isPublic) => _resolve().SealMouSsxMove(id, move, isPublic);
    internal bool HasMouSunShangxiangMobile { get => _resolve().HasMouSunShangxiangMobile; }
    internal global::CardGame.Core.MouSunShangxiangRuntime MouSunShangxiang { get => _resolve().MouSunShangxiang; }
}
