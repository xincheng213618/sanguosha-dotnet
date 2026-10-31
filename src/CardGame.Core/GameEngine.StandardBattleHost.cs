using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IStandardBattleHost CreateStandardBattleHost()
    {
        if (_options.ShanheBattle is not null && _contentRegistry.Skills.TryGetValue(ShanheSourceDongdangRules.SkillId, out var definition))
            ShanheSourceDongdangRules.ValidateDefinition(definition);
        return this.ComposeNativeStandardBattleHost();
    }
}
