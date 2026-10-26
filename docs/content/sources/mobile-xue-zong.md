# 薛综（普通移动版，2025重制）

核查日期：2026-10-26。身份 `mobile:xue-zong`；吴、男、3体力；移动版乱世英杰2017，图鉴沿一将2017分组。注册/将池顺序80899，两项技能80800/80801。立绘建议 `mobile-xue-zong`，[官网经典原画39400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/39400.jpg)，画师秋呆呆。

## 正式版本来源

- [官网人物231](https://www.sanguosha.cn/pc/hero-detail-231.html)：当前技能正文列两级复难及四色诫训，和本实现一致。人物特点仍提方块、玩法说明仍提把原使用牌给对方，属于未同步的旧介绍；本实现采用同页的当前技能正文。
- [官方《1月28日春节活动》](https://www.sanguosha.cn/pc/news-detail-2001.html)：发布于2025-01-27，明确宣布薛综重制上线，并列出1月28日开始的正式活动及获取方式。实现的是已上线身份版，不是根据测试服爆料推测。
- [移动BWIKI薛综](https://wiki.biligame.com/msgs/薛综)及[固定版本31235](https://wiki.biligame.com/msgs/index.php?title=薛综&oldid=31235)：普通经典身份栏为上述重制版，更新日期2025-06-24。该日期不是上线日期。
- [移动BWIKI复难](https://wiki.biligame.com/msgs/复难)、[诫训](https://wiki.biligame.com/msgs/诫训)与[历史修改记录](https://wiki.biligame.com/msgs/武将修改相关记录页面#薛综)：现行正文与官网相同，历史记录中的旧正式服规则不作为当前身份版规则。

## 采用规则及与classic的差异

一级复难在他人响应自己实际使用的牌时，可直接取得对方响应所使用或打出的实体牌。原牌继续原生结算，不交给响应者，也不产生旧版禁用。升级后保留此能力，并在自己使用取得的牌结算完毕、没有其他角色响应该次用牌时摸一张。

诫训在结束阶段选择一个真实花色及一名其他角色，按存活角色装备区和判定区的有效花色统计场上牌数，最多摸五张，再弃本技能本次累计发动次数的牌。第一次即弃一张。实际弃置包含目标当时所有非空手牌便升级复难，装备可以留下；诫训本身继续保留。目标本来没有手牌、只弃装备，不作为“因此弃置了所有手牌”。

`classic:funan`/`classic:jiexun`的原定义分别包含原牌交付禁用、固定方块、不封顶摸牌、按此前次数弃牌、弃尽HE及移除诫训，无法完整等价复用。因此新增两项移动技能，复用其实际响应实体主张、原帧移牌等待、真实逻辑弃置及来源配对的升级事实。

## 生产契约

- `exchangeRespondedCardEntities.responseExchangeMode=claimResponseOnly`只改变原牌交付策略。仍要求真实直接被响应动作由技能所有者使用、响应者为他人、响应实体可在Processing/Discard被主张且未被其他主张消费。无懈链直接上一张无懈沿已有`ActualRespondedAction`解析。省略策略保持经典交换行为。
- `drawPublicSuitThenEscalatingDiscard.publicSuitDiscardMode=chooseSuitCappedCurrentCountRetain`是固定的一整套重制规则；定义必须给四个真实花色。选择、摸牌数、累计弃牌数保留在同一个`ProgramPublicSuitDiscardDraft`，真实弃牌付款时冻结手牌集合，随后按原逻辑弃置及移牌子窗返回，不能在观察者回调后重新看空手牌判断升级。
- 持久升级经`CommitProgramSkillChangeFact`，继续发原`ProgramResponseExchangeUpgradedEvent`，只修改与本诫训同一实际技能来源配对的复难实例。重复弃尽手牌不会重复升级，也不会移除诫训。
- 取得实体再次使用的资格在原生`CardActionAcceptedEvent`边界冻结，包含实际ActionId、使用者、技能实例/内容指纹、实体ID、原取得动作及真实取得移动序号。完成时不按当前手牌、牌名或所有者布尔值猜测资格。新事件均为标量，不新增集合事件投影。
- 基本牌、装备牌的实体使用同样订阅已有原生目标确定/接受入口；它们不依赖“有人能响应此类别”来建立资格。经典交换原本只要求普通锦囊的接受身份，新版在人物模块通过`ProgramTargetWindow`声明额外类别，不更改经典默认。
- 原生响应动作仍保留`Response`类型；只有原生类型化的用闪/无懈才按“使用”处理。决斗打出杀、群体锦囊打出牌及代提供不会冒充自己使用取得牌。Dodge/Nullification奖励在原`CompletedResponseReturn`与父帧上结算，标准Draw及`awaitBoundCardMovements`保留已付款子窗，不复制使用或响应流程。
- “无人响应”按实际直接响应边判断：其他角色真正响应才阻止奖励；拒绝响应不阻止，技能所有者自己响应不符合“其他角色”。无懈链的直接响应事实在接受响应时冻结，不以外层锦囊的兼容`ParentActionId`替代。
- 原生无懈的普通`CardUseCompleted`在下一次反无懈询问之前通知，因此仅将复难的对应候选延期到原`NullificationWindowFrame`上的类型化完成回执；其他技能照原顺序完成。全链直接响应关系确定后，通用`BeforeNullificationFinished`挂点从该原窗压入仅本来源的标准`ProgramCardTriggerWindowFrame`，原`CompletedResponseReturn(Nullification)`将已付款奖励返回同一无懈窗，再完成原锦囊。不重发其他完成观察者，不在外层锦囊完成时替代结算，也不建立平行pending对象。

官网未提供转换、多实体组成和取得后转移再取得的专项FAQ。本实现按实体身份解释：真实使用含有至少一张该复难实例取得的实体，就算使用了取得的牌；每次用牌只摸一次。升级前取得的实体在升级后真实使用同样合格；文字未声明取得实体标记的回合到期或离手清除，因此不自行添加此类期限。代提供材料者与真正使用者分别按原生动作身份判断，只有技能所有者实际使用才发给其奖励。这些是实现采用的解释，不宣称为额外官方裁定。

## 验证范围

保留既有`Fame2017XueZongChecks.SlashDuelAndCounterspellEntityExchange`覆盖经典真实杀、决斗、无懈/铁索、单双实体、Processing主张和代表冷恢复，不复制该矩阵。

新增`MobileXueZongChecks.DirectClaimHandOnlyUpgradeRetainedAndUnansweredEntityUsesCold`覆盖未升级直接取得、非方块零摸首次弃一、来源配对升级并保留诫训、后续累计次数、取得实体的无人响应Equipment奖励/被响应Duel不奖励、Duel打出排除、原生类型化用闪奖励及真实取牌/弃牌/摸牌子窗冷恢复。

新增`MobileXueZongChecks.ChosenPublicSuitCapAndHandOnlyUpgradeWithEquipmentRemaining`覆盖六张真实场上同花色牌只摸五张，已付款摸牌/弃置冷恢复，以及弃尽手牌但留装备仍升级。验证由集成任务串行运行，定向检查不代表全量通过。

新增`MobileXueZongChecks.AcquiredCounterspellDirectAnswerAndSettledRewardCold`复用同一个真实升级准备过程，覆盖取得实体转换为无懈后被另一角色反无懈时不奖励、随后使用刚取得的响应实体且无人反无懈时才摸一，以及原无懈待响应回执和最终已付款Draw子窗的代表冷恢复。
