# 移动版界吕蒙

核对日期：2026-10-25。独立身份 `boundary:lu-meng-mobile`，吴、男性、四体力，画像键 `boundary-lu-meng-mobile`。

- [移动版官网武将详情159](https://www.sanguosha.cn/index.php/pc/hero-detail-159.html)完整列出克己、勤学，并在勤学内说明获得的攻心。克己沿出牌阶段未使用或打出杀的条件跳过弃牌；勤学仅准备阶段，手牌比体力多三张或更多时觉醒，游戏人数至少七人改为多两张或更多，减一点体力上限后获得攻心，没有回复或额外摸牌。
- [移动版Wiki界吕蒙](https://wiki.biligame.com/msgs/%E7%95%8C%E5%90%95%E8%92%99)，2025-11-12更新：身份技能仍只有克己、勤学及其衍生攻心，没有博图。此页把勤学条件写作手牌“大于体力+3”（至少七人改+2），与官网“多3或更多/改为2”有一张的差异；本登记明确采用官网包含等号的门槛，不把两种表述伪称完全一致。
- [移动版Wiki攻心条目](https://wiki.biligame.com/msgs/%E6%94%BB%E5%BF%83/%E7%95%8C%E5%90%95%E8%92%99)，2025-06-03更新：每个出牌阶段一次，观看其他角色全部手牌，再可展示其中一张红桃并弃置或置顶，与官网159嵌入的攻心全文一致。核查未发现移动版后续官方修订增添博图或改成结束阶段觉醒。

克己直接复用 `boundary:keji`，保留实际回合中所有出牌阶段的原生使用/打出杀记录。勤学另登记 `boundary:qinxue-mobile`，只在准备阶段触发；固定开局座位数决定二/三张门槛，不随死亡人数增减。达到官网门槛后沿通用 `changeMaximumHp`、`grantSkills` 和全局一次觉醒额度，获得完整既有 `classic:gongxin`。不新增支付pending或独立的技能替换实现。

与现有OL界吕蒙明确区分：本人物不使用 `boundary:qinxue` 的准备/结束双时机、固定多两张、摸两张或回复一点的收益；也不附带 `boundary:botu`。由勤学取得攻心仍受既有私有观看、可放弃展示、红桃资格及弃置/置顶原生区域和恢复合同约束。

新增最少资格行为检查用原生支持的四人和八人桌，覆盖官网小桌与游戏人数至少七人时的门槛差别、仅准备阶段觉醒、实际减体力上限后获得攻心及原生子窗冷恢复。共有克己、觉醒技能变更和攻心窗口沿现有机制覆盖；不复制OL博图或谋版克己的不同玩法夹具。

入口为 `BoundaryLuMengMobileChecks.PreparationThresholdsGrantOnlyGongxinAfterNativeMaximumChildCold`。已有过滤器 `Skill changes: shared cap state grants and paid child cold` 和 `Hand reveal privacy: FireAttack and Gongxin close before opaque Dismantlement cold` 继续覆盖共同技能变更与攻心窗口。
