# 移动版界庞统

核对日期：2026-10-25。独立身份 `boundary:pang-tong-mobile`，蜀、男性、三体力，画像键 `boundary-pang-tong-mobile`。

- [移动版官网武将详情268](https://www.sanguosha.cn/index.php/pc/hero-detail-268.html)：连环限定一张梅花手牌，可转铁索连环使用或重铸，铁索额外一名目标。涅槃可在出牌阶段主动使用，也可于本人濒死时发动；本局只用一次，依次弃置本人区域全部牌、摸三、回复到三并复原武将牌。
- [移动版Wiki界庞统](https://wiki.biligame.com/msgs/%E7%95%8C%E5%BA%9E%E7%BB%9F)，2025-10-21更新：完整当前身份正文与上述两技能一致，涅槃没有获得八阵、火计或看破。页面列出的标准、SP、友、谋版不用于本人物。
- [2019-09-30移动版上线公告685](https://www.sanguosha.cn/index.php/pc/news-detail-685.html)可交叉定位该移动界版上线。核查同站后续公告未找到取代官网268当前完整规则的技能修订。

两项技能分别登记 `boundary:lianhuan-mobile`、`boundary:niepan-mobile`。现有OL界连环允许装备区梅花牌，不能直接复用其转化范围；本连环只改为手牌来源，原生三目标铁索和重铸仍沿相同通用执行器。原重铸不是用牌，不制造锦囊使用或额外用牌次数。

涅槃与现有OL的区别还包括主动入口、恢复顺序及没有获得卧龙技能。出牌和濒死入口使用完全相同的弃牌、摸牌、回复、解横置和翻正节点，并共享已有原生游戏次数仓库的一条额度：主动 `usageGroup` 为 `named-group:rebirth`，濒死 `namedUsageGroup` 为 `rebirth`，后者由既有运行时生成同一键。失去、再获得该技能不会恢复已消费的限定次数。

牌的弃置涵盖本人手牌、装备和判定区，真实弃牌孩子返回前不摸牌；回复以三点为目标并由通用节点限制到实际体力上限。已接受的限定效果在孩子内失去来源时沿原帧继续，不重新弃牌或重付次数。未新增pending、技能替换实现或版本标记。

最少新检查入口 `BoundaryPangTongMobileChecks.HandOnlyChainAndProactiveNiepanShareDyingLimitWithPaidDiscardCold`：固定实体场景验证梅花装备不能转化/重铸而梅花手牌可真实重铸、额外目标资格、主动HEJ全弃子窗冷恢复、真实失技后的同帧完成，以及再授技后主动与濒死入口均不能重复限定。继续复用 `Program Iron Chain third target owned recast cold replay` 与 `schema-22 Niepan clears owned state and replays` 的原生铁索、重铸和濒死主体覆盖，不复制其组合矩阵。

官方原始立绘取自官网268的首张[43600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/43600.jpg)，530×750 JPEG，原图保存，没有裁切、缩放或重绘。
