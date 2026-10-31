namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeBattleRulesHost CreateActivityBattleRulesHost() => this.ComposeNativeActivityBattleRulesHost();
    private IWenheRuleFlowHost CreateWenheRuleFlowHost() => this.ComposeNativeWenheRuleFlowHost();
    private IGuanduEventFlowHost CreateGuanduEventFlowHost() => this.ComposeNativeGuanduEventFlowHost();
}
