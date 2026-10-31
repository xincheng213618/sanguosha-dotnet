#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShangxiangMouMobileEntries
{
    internal void ChangeMouSsxFailureFaction(ProgramSkillFrame f) => _resolve().ChangeMouSsxFailureFaction(f);
    internal void ChangeMouSsxZhu(ProgramSkillFrame f, int previous, int current, bool initial) => _resolve().ChangeMouSsxZhu(f, previous, current, initial);
    internal void GrantMouSsxArmor(ProgramSkillFrame f, int seat) => _resolve().GrantMouSsxArmor(f, seat);
    internal bool IsIssuedMouSsxZhuang(PublicPersistentPileSource source) => _resolve().IsIssuedMouSsxZhuang(source);
    internal bool MouSsxParentMatches(ProgramSkillFrame f) => _resolve().MouSsxParentMatches(f);
    internal void SealMouSsxMove(long id, CardMovementRecord move, bool isPublic) => _resolve().SealMouSsxMove(id, move, isPublic);
    internal bool HasMouSunShangxiangMobile { get => _resolve().HasMouSunShangxiangMobile; }
    internal MouSunShangxiangRuntime MouSunShangxiang { get => _resolve().MouSunShangxiang; }
}
